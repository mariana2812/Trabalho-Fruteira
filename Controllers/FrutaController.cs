using Trabalho_Fruteira.Data;
using Trabalho_Fruteira.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Trabalho_Fruteira.Controllers
{
    [ApiController]
    [Route("api/frutas")]
    public class FrutaController : ControllerBase
    {

        private readonly AppDbContext _context;


        public FrutaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Listar()
        {
            var frutas = await _context.Fruta
                .Include(f => f.Categoria)
                .ToListAsync();


            return Ok(frutas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Buscar(int id)
        {
            var fruta = await _context.Fruta
                .FindAsync(id);


            if (fruta == null)
                return NotFound("Fruta não encontrada.");


            return Ok(fruta);
        }

        [HttpPost]
        public async Task<ActionResult> Cadastrar(
            Fruta fruta
        )
        {

            fruta.Id = 0;

            var categoria =
                await _context.Categoria
                .FindAsync(fruta.CategoriaId);


            if (categoria == null)
                return BadRequest(
                    "Categoria não encontrada."
                );

            _context.Fruta.Add(fruta);

            await _context.SaveChangesAsync();

            return Ok(fruta);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Alterar(
            int id,
            Fruta fruta
        )
        {

            var frutaBanco =
                await _context.Fruta
                .FindAsync(id);


            if (frutaBanco == null)
                return NotFound(
                    "Fruta não encontrada."
                );


            frutaBanco.Nome =
                fruta.Nome;


            frutaBanco.Preco =
                fruta.Preco;


            frutaBanco.Estoque =
                fruta.Estoque;


            frutaBanco.CategoriaId =
                fruta.CategoriaId;


            await _context.SaveChangesAsync();


            return Ok(frutaBanco);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Excluir(
            int id
        )
        {

            var fruta =
                await _context.Fruta
                .FindAsync(id);


            if (fruta == null)
                return NotFound(
                    "Fruta não encontrada."
                );


            _context.Fruta.Remove(fruta);

            await _context.SaveChangesAsync();

            return NoContent();

        }

        [HttpGet("{id}/estoque")]
        public async Task<ActionResult> Estoque(
            int id
        )
        {

            var fruta =
                await _context.Fruta
                .FindAsync(id);


            if (fruta == null)
                return NotFound();


            return Ok(new
            {
                fruta.Nome,
                fruta.Estoque
            });

        }

        [HttpPatch("{id}/estoque")]
        public async Task<ActionResult> AlterarEstoque(
            int id,
            string tipo,
            int quantidade
        )
        {

            var fruta =
                await _context.Fruta
                .FindAsync(id);


            if (fruta == null)
                return NotFound();


            if (tipo == "entrada")
            {
                fruta.Estoque += quantidade;
            }

            else if (tipo == "saida")
            {

                if (fruta.Estoque < quantidade)
                    return BadRequest(
                        "Estoque insuficiente."
                    );


                fruta.Estoque -= quantidade;

            }

            else
            {
                return BadRequest(
                    "Use entrada ou saida."
                );
            }


            await _context.SaveChangesAsync();


            return Ok(fruta);

        }

    }
}