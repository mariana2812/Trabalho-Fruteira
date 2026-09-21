using Trabalho_Fruteira.Data;
using Trabalho_Fruteira.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Trabalho_Fruteira.Controllers
{
    [ApiController]
    [Route("api/vendas")]
    public class VendaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VendaController(AppDbContext context)
        {
            _context = context;
        }

        // POST: cadastra uma venda.
        [HttpPost]
        public async Task<ActionResult> Registrar(Pedido dados)
        {
            var cliente = await _context.Cliente.FindAsync(dados.ClienteId);

            if (cliente == null)
                return BadRequest("Cliente não encontrado.");

            if (dados.Itens == null || dados.Itens.Count == 0)
                return BadRequest("Adicione pelo menos um item.");

            var pedido = new Pedido
            {
                ClienteId = dados.ClienteId,
                DataPedido = DateTime.Now
            };

            foreach (var item in dados.Itens)
            {
                if (item.Quantidade <= 0)
                    return BadRequest("A quantidade deve ser maior que zero.");

                var fruta = await _context.Fruta.FindAsync(item.FrutaId);

                if (fruta == null)
                    return BadRequest("Fruta não encontrada.");

                if (fruta.Estoque < item.Quantidade)
                    return BadRequest("Estoque insuficiente.");

                var novoItem = new ItemPedido
                {
                    FrutaId = fruta.Id,
                    Quantidade = item.Quantidade,
                    PrecoUnitario = fruta.Preco,
                    Subtotal = fruta.Preco * item.Quantidade
                };

                pedido.Itens.Add(novoItem);
                pedido.ValorFinal += novoItem.Subtotal;

                fruta.Estoque -= item.Quantidade;
            }

            _context.Pedido.Add(pedido);
            await _context.SaveChangesAsync();

            return Ok(pedido);
        }

        // GET: lista todas as vendas com seus itens.
        [HttpGet]
        public async Task<ActionResult> Listar()
        {
            var pedidos = await _context.Pedido
                .Include(p => p.Itens)
                .ToListAsync();

            return Ok(pedidos);
        }

        // GET: mostra os itens de uma venda.
        [HttpGet("{id}/itens")]
        public async Task<ActionResult> Itens(int id)
        {
            var pedido = await _context.Pedido
                .Include(p => p.Itens)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
                return NotFound("Venda não encontrada.");

            return Ok(pedido.Itens);
        }

        // GET: mostra o total de uma venda.
        [HttpGet("{id}/total")]
        public async Task<ActionResult> Total(int id)
        {
            var pedido = await _context.Pedido.FindAsync(id);

            if (pedido == null)
                return NotFound("Venda não encontrada.");

            return Ok(new
            {
                pedido.Id,
                pedido.ValorFinal
            });
        }

        // PUT: substitui o cliente e todos os itens da venda.
        [HttpPut("{id}")]
        public async Task<ActionResult> Alterar(int id, Pedido dados)
        {
            var pedido = await _context.Pedido
                .Include(p => p.Itens)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
                return NotFound("Venda não encontrada.");

            var cliente = await _context.Cliente.FindAsync(dados.ClienteId);

            if (cliente == null)
                return BadRequest("Cliente não encontrado.");

            if (dados.Itens == null || dados.Itens.Count == 0)
                return BadRequest("Adicione pelo menos um item.");

            foreach (var item in dados.Itens)
            {
                if (item.Quantidade <= 0)
                    return BadRequest("A quantidade deve ser maior que zero.");
            }

            // Devolve as quantidades antigas ao estoque.
            foreach (var item in pedido.Itens)
            {
                var fruta = await _context.Fruta.FindAsync(item.FrutaId);

                if (fruta == null)
                    return BadRequest("Uma fruta da venda não foi encontrada.");

                fruta.Estoque += item.Quantidade;
            }

            var novosItens = new List<ItemPedido>();
            decimal total = 0;

            // Prepara os novos itens e desconta suas quantidades.
            foreach (var item in dados.Itens)
            {
                var fruta = await _context.Fruta.FindAsync(item.FrutaId);

                if (fruta == null)
                    return BadRequest("Fruta não encontrada.");

                if (fruta.Estoque < item.Quantidade)
                    return BadRequest("Estoque insuficiente.");

                var novoItem = new ItemPedido
                {
                    FrutaId = fruta.Id,
                    Quantidade = item.Quantidade,
                    PrecoUnitario = fruta.Preco,
                    Subtotal = fruta.Preco * item.Quantidade
                };

                novosItens.Add(novoItem);
                total += novoItem.Subtotal;

                fruta.Estoque -= item.Quantidade;
            }

            // Remove os itens antigos e coloca os novos.
            _context.ItemPedido.RemoveRange(pedido.Itens);

            pedido.Itens = novosItens;
            pedido.ClienteId = dados.ClienteId;
            pedido.ValorFinal = total;

            await _context.SaveChangesAsync();

            return Ok(pedido);
        }

        // PATCH: altera somente o cliente da venda.
        [HttpPatch("{id}/cliente")]
        public async Task<ActionResult> AlterarCliente(
            int id,
            [FromQuery] int clienteId)
        {
            var pedido = await _context.Pedido.FindAsync(id);

            if (pedido == null)
                return NotFound("Venda não encontrada.");

            var cliente = await _context.Cliente.FindAsync(clienteId);

            if (cliente == null)
                return BadRequest("Cliente não encontrado.");

            pedido.ClienteId = clienteId;

            await _context.SaveChangesAsync();

            return Ok(pedido);
        }

        // DELETE: exclui a venda e devolve o estoque.
        [HttpDelete("{id}")]
        public async Task<ActionResult> Excluir(int id)
        {
            var pedido = await _context.Pedido
                .Include(p => p.Itens)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
                return NotFound("Venda não encontrada.");

            foreach (var item in pedido.Itens)
            {
                var fruta = await _context.Fruta.FindAsync(item.FrutaId);

                if (fruta == null)
                    return BadRequest("Uma fruta da venda não foi encontrada.");

                fruta.Estoque += item.Quantidade;
            }

            _context.ItemPedido.RemoveRange(pedido.Itens);
            _context.Pedido.Remove(pedido);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}