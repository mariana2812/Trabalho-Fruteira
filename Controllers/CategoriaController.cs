using Trabalho_Fruteira.Data;
using Trabalho_Fruteira.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Trabalho_Fruteira.Controllers
{

    [ApiController]
    [Route("api/categorias")]
    public class CategoriaController : ControllerBase
    {

        private readonly AppDbContext _context;


        public CategoriaController(
            AppDbContext context
        )
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Listar()
        {

            return Ok(
                await _context.Categoria
                .ToListAsync()
            );

        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Buscar(
            int id
        )
        {

            var categoria =
                await _context.Categoria
                .FindAsync(id);


            if (categoria == null)
                return NotFound(
                    "Categoria não encontrada."
                );


            return Ok(categoria);

        }

        [HttpPost]
        public async Task<ActionResult> Cadastrar(
            Categoria categoria
        )
        {

            categoria.Id = 0;

            _context.Categoria.Add(
                categoria
            );

            await _context.SaveChangesAsync();

            return Ok(categoria);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Alterar(
            int id,
            Categoria categoria
        )
        {

            var banco =
                await _context.Categoria
                .FindAsync(id);


            if (banco == null)
                return NotFound();


            banco.Nome =
                categoria.Nome;


            banco.Descricao =
                categoria.Descricao;


            await _context.SaveChangesAsync();

            return Ok(banco);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Excluir(
            int id
        )
        {

            var categoria =
                await _context.Categoria
                .FindAsync(id);


            if (categoria == null)
                return NotFound();


            _context.Categoria.Remove(categoria);


            await _context.SaveChangesAsync();


            return NoContent();

        }

    }
}