using Trabalho_Fruteira.Data;
using Trabalho_Fruteira.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Trabalho_Fruteira.Controllers
{

    [ApiController]
    [Route("api/clientes")]
    public class ClienteController : ControllerBase
    {

        private readonly AppDbContext _context;


        public ClienteController(
            AppDbContext context
        )
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Listar()
        {

            return Ok(
                await _context.Cliente
                .ToListAsync()
            );

        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Buscar(
            int id
        )
        {

            var cliente =
                await _context.Cliente
                .FindAsync(id);


            if (cliente == null)
                return NotFound();

            return Ok(cliente);

        }

        [HttpPost]
        public async Task<ActionResult> Cadastrar(
            Cliente cliente
        )
        {

            cliente.Id = 0;

            _context.Cliente.Add(cliente);

            await _context.SaveChangesAsync();

            return Ok(cliente);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Alterar(
            int id,
            Cliente cliente
        )
        {

            var banco =
                await _context.Cliente
                .FindAsync(id);


            if (banco == null)
                return NotFound();


            banco.Nome =
                cliente.Nome;


            banco.Preferencia =
                cliente.Preferencia;

            await _context.SaveChangesAsync();

            return Ok(banco);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Excluir(
            int id
        )
        {

            var cliente =
                await _context.Cliente
                .FindAsync(id);


            if (cliente == null)
                return NotFound();

            _context.Cliente.Remove(cliente);

            await _context.SaveChangesAsync();

            return NoContent();

        }

    }
}