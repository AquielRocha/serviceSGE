using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServicesGE.API.Data;
using ServicesGE.API.Models;

namespace ServicesGE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FornecedorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FornecedorController(AppDbContext context)
        {
            _context = context;
        }

        // Listar todos os fornecedores
        [HttpGet]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<ActionResult<IEnumerable<Fornecedor>>> GetFornecedores()
        {
            return await _context.fornecedores.ToListAsync();
        }

        // Buscar fornecedor por ID
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<ActionResult<Fornecedor>> GetFornecedor(int id)
        {
            var fornecedor = await _context.fornecedores.FindAsync(id);

            if (fornecedor == null)
                return NotFound("Fornecedor não encontrado.");

            return Ok(fornecedor);
        }

        // Cadastrar fornecedor
        [HttpPost]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<IActionResult> CreateFornecedor([FromBody] Fornecedor fornecedor)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.fornecedores.Add(fornecedor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFornecedor), new { id = fornecedor.fornecedorid }, fornecedor);
        }

        // Atualizar fornecedor
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<IActionResult> UpdateFornecedor(int id, [FromBody] Fornecedor fornecedor)
        {
            if (id != fornecedor.fornecedorid)
                return BadRequest("ID informado não corresponde ao fornecedor.");

            if (!_context.fornecedores.Any(f => f.fornecedorid == id))
                return NotFound("Fornecedor não encontrado.");

            _context.Entry(fornecedor).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Excluir fornecedor
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Apenas Admin pode excluir
        public async Task<IActionResult> DeleteFornecedor(int id)
        {
            var fornecedor = await _context.fornecedores.FindAsync(id);

            if (fornecedor == null)
                return NotFound("Fornecedor não encontrado.");

            _context.fornecedores.Remove(fornecedor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
