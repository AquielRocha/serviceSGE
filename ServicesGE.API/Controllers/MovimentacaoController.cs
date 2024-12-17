using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServicesGE.API.Data;
using ServicesGE.API.Models;

namespace ServicesGE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimentacaoEstoqueController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MovimentacaoEstoqueController(AppDbContext context)
        {
            _context = context;
        }

        // Registrar movimentação de estoque (Entrada ou Saída)
        [HttpPost]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<IActionResult> RegistrarMovimentacao([FromBody] MovimentacaoEstoque movimentacao)
        {
            var produto = await _context.produtos.FindAsync(movimentacao.produtoid);

            if (produto == null)
                return NotFound("Produto não encontrado.");

            if (movimentacao.tipomovimento == "Entrada")
            {
                produto.quantidadeestoque += movimentacao.quantidade;
            }
            else if (movimentacao.tipomovimento == "Saída")
            {
                if (produto.quantidadeestoque < movimentacao.quantidade)
                    return BadRequest("Estoque insuficiente.");

                produto.quantidadeestoque -= movimentacao.quantidade;
            }
            else
            {
                return BadRequest("Tipo de movimentação inválido. Use 'Entrada' ou 'Saída'.");
            }

            _context.movimentacaoestoque.Add(movimentacao);
            _context.Entry(produto).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Movimentação registrada com sucesso.", produto.quantidadeestoque });
        }

        // Listar movimentações de estoque
        [HttpGet]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<ActionResult<IEnumerable<MovimentacaoEstoque>>> GetMovimentacoes()
        {
            var movimentacoes = await _context.movimentacaoestoque
                .Include(m => m.Produto)
                .ToListAsync();

            return Ok(movimentacoes);
        }
    }
}
