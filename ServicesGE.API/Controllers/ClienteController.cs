using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServicesGE.API.Data;
using ServicesGE.API.Models;

namespace ServicesGE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClienteController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("getAll")]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            return await _context.clientes.ToListAsync();
        }

        [HttpGet("getById/{id}")]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var cliente = await _context.clientes.FindAsync(id);

            if (cliente == null)
                return NotFound("Cliente não encontrado.");

            return Ok(cliente);
        }

        [HttpPost("add")]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<IActionResult> CreateCliente([FromBody] Cliente cliente)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCliente), new { id = cliente.ClienteId }, cliente);
        }

        [HttpPut("edit/{id}")]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<IActionResult> UpdateCliente(int id, [FromBody] Cliente cliente)
        {
            if (id != cliente.ClienteId)
                return BadRequest("ID informado não corresponde ao cliente.");

            if (!_context.clientes.Any(c => c.ClienteId == id))
                return NotFound("Cliente não encontrado.");

            _context.Entry(cliente).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")] // Apenas Admin pode excluir
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.clientes.FindAsync(id);

            if (cliente == null)
                return NotFound("Cliente não encontrado.");

            _context.clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
