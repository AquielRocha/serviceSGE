using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServicesGE.API.Data;
using ServicesGE.API.Models;

namespace ServicesGE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotaFiscalController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NotaFiscalController(AppDbContext context)
        {
            _context = context;
        }

        // Cadastrar Nota Fiscal
        [HttpPost]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<IActionResult> CreateNotaFiscal([FromBody] NotaFiscal notaFiscal)
        {
            // Valida cliente
            var cliente = await _context.clientes.FindAsync(notaFiscal.clienteid);
            if (cliente == null) return NotFound("Cliente não encontrado.");

            // Valida e adiciona os itens da nota fiscal
            decimal valorTotal = 0;

            foreach (var item in notaFiscal.ItensNotaFiscal)
            {
                var produto = await _context.produtos.FindAsync(item.produtoid);
                if (produto == null) return NotFound($"Produto com ID {item.produtoid} não encontrado.");

                // Atualiza estoque do produto
                if (produto.quantidadeestoque < item.quantidade)
                    return BadRequest($"Estoque insuficiente para o produto: {produto.nome}.");

                produto.quantidadeestoque -= item.quantidade;

                // Calcula subtotal
                item.precounitario = produto.preco;
                valorTotal += item.quantidade * item.precounitario;
            }

            notaFiscal.valortotal = valorTotal;

            // Adiciona ao banco de dados
            _context.notasfiscais.Add(notaFiscal);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNotaFiscal), new { id = notaFiscal.notafiscalid }, notaFiscal);
        }

        // Listar Notas Fiscais
        [HttpGet]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<ActionResult<IEnumerable<NotaFiscal>>> GetNotasFiscais()
        {
            return await _context.notasfiscais
                .Include(n => n.Cliente)
                .Include(n => n.ItensNotaFiscal)
                    .ThenInclude(i => i.Produto)
                .ToListAsync();
        }

        // Buscar Nota Fiscal por ID
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<ActionResult<NotaFiscal>> GetNotaFiscal(int id)
        {
            var notaFiscal = await _context.notasfiscais
                .Include(n => n.Cliente)
                .Include(n => n.ItensNotaFiscal)
                    .ThenInclude(i => i.Produto)
                .FirstOrDefaultAsync(n => n.notafiscalid == id);

            if (notaFiscal == null)
                return NotFound("Nota fiscal não encontrada.");

            return Ok(notaFiscal);
        }
    }
}
