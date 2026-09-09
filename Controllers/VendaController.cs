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


        public VendaController(
            AppDbContext context
        )
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> Registrar(
            Pedido pedido
        )
        {
            pedido.Id = 0;
            pedido.DataPedido =
                DateTime.Now;

            decimal total = 0;

            foreach (var item in pedido.Itens)
            {

                var fruta =
                    await _context.Fruta
                    .FindAsync(item.FrutaId);

                if (fruta == null)
                    return BadRequest(
                        "Fruta não encontrada."
                    );

                if (fruta.Estoque < item.Quantidade)
                    return BadRequest(
                        "Estoque insuficiente."
                    );


                item.PrecoUnitario =
                    fruta.Preco;


                item.Subtotal =
                    fruta.Preco *
                    item.Quantidade;


                fruta.Estoque -=
                    item.Quantidade;


                total += item.Subtotal;

            }

            pedido.ValorFinal =
                total;


            _context.Pedido.Add(pedido);


            await _context.SaveChangesAsync();


            return Ok(pedido);

        }

        [HttpGet]
        public async Task<ActionResult> Listar()
        {

            return Ok(
                await _context.Pedido
                .ToListAsync()
            );

        }

        [HttpGet("{id}/itens")]
        public async Task<ActionResult> Itens(
            int id
        )
        {

            var itens =
                await _context.ItemPedido
                .Where(
                    i => i.PedidoId == id
                )
                .ToListAsync();


            return Ok(itens);

        }

        [HttpGet("{id}/total")]
        public async Task<ActionResult> Total(
            int id
        )
        {

            var venda =
                await _context.Pedido
                .FindAsync(id);


            if (venda == null)
                return NotFound();


            return Ok(new
            {
                venda.Id,
                venda.ValorFinal
            });

        }

    }
}