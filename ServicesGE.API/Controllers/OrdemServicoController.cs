using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServicesGE.API.Data;
using ServicesGE.API.Models;

namespace ServicesGE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdemServicoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdemServicoController(AppDbContext context)
        {
            _context = context;
        }

        // Cadastrar nova ordem de serviço
        [HttpPost]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<IActionResult> CreateOrdemServico([FromBody] OrdemServico ordemServico)
        {
            // Valida cliente
            var cliente = await _context.clientes.FindAsync(ordemServico.clienteid);
            if (cliente == null) return NotFound("Cliente não encontrado.");

            decimal valorTotal = 0;

            foreach (var item in ordemServico.ItensOrdemServico)
            {
                if (item.produtoid.HasValue)
                {
                    var produto = await _context.produtos.FindAsync(item.produtoid);
                    if (produto == null) return NotFound($"Produto com ID {item.produtoid} não encontrado.");

                    if (produto.quantidadeestoque < item.quantidade)
                        return BadRequest($"Estoque insuficiente para o produto {produto.nome}.");

                    produto.quantidadeestoque -= item.quantidade.Value;
                    item.precounitario = produto.preco;
                    valorTotal += item.quantidade.Value * item.precounitario.Value;
                }
            }

            ordemServico.valortotal = valorTotal;

            _context.ordens_servico.Add(ordemServico);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrdemServico), new { id = ordemServico.osid }, ordemServico);
        }

        // Listar todas as ordens de serviço
        [HttpGet]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<ActionResult<IEnumerable<OrdemServico>>> GetOrdensServico()
        {
            return await _context.ordens_servico
                .Include(o => o.Cliente)
                .Include(o => o.StatusOrdemServico)
                .Include(o => o.ItensOrdemServico)
                    .ThenInclude(i => i.Produto)
                .ToListAsync();
        }

        // Buscar ordem de serviço por ID
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<ActionResult<OrdemServico>> GetOrdemServico(int id)
        {
            var ordemServico = await _context.ordens_servico
                .Include(o => o.Cliente)
                .Include(o => o.StatusOrdemServico)
                .Include(o => o.ItensOrdemServico)
                    .ThenInclude(i => i.Produto)
                .FirstOrDefaultAsync(o => o.osid == id);

            if (ordemServico == null)
                return NotFound("Ordem de serviço não encontrada.");

            return Ok(ordemServico);
        }
    }
}
