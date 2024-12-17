using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServicesGE.API.Data;
using ServicesGE.API.Models;

namespace ServicesGE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutoController(AppDbContext context)
        {
            _context = context;
        }

        // Listar todos os produtos
        [HttpGet]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
        {
            return await _context.produtos.Include(p => p.Fornecedor).ToListAsync();
        }

        // Buscar produto por ID
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<ActionResult<Produto>> GetProduto(int id)
        {
            var produto = await _context.produtos.Include(p => p.Fornecedor)
                                                 .FirstOrDefaultAsync(p => p.produtoid == id);

            if (produto == null)
                return NotFound("Produto não encontrado.");

            return Ok(produto);
        }

        // Cadastrar produto
        [HttpPost]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<IActionResult> CreateProduto([FromBody] Produto produto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.produtos.Add(produto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduto), new { id = produto.produtoid }, produto);
        }

        // Atualizar produto
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<IActionResult> UpdateProduto(int id, [FromBody] Produto produto)
        {
            if (id != produto.produtoid)
                return BadRequest("ID informado não corresponde ao produto.");

            _context.Entry(produto).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Excluir produto
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            var produto = await _context.produtos.FindAsync(id);
            if (produto == null)
                return NotFound("Produto não encontrado.");

            _context.produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Listar produtos com baixo estoque
        [HttpGet("alerta-estoque")]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutosBaixoEstoque()
        {
            var produtos = await _context.produtos
                .Where(p => p.quantidadeestoque < 5)
                .ToListAsync();

            return Ok(produtos);
        }
    }
}
