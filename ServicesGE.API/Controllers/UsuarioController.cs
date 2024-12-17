using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServicesGE.API.Data;
using ServicesGE.API.Models;

namespace ServicesGE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }


        // GET: api/Usuario/GetMe
       [HttpGet("GetMe")]
[Authorize]
public async Task<ActionResult<Usuario>> GetMe()
{
    // Obtém o email do usuário do token JWT
    var userEmail = User.FindFirst(ClaimTypes.Name)?.Value;

    if (string.IsNullOrEmpty(userEmail))
    {
        return Unauthorized("Token inválido ou não autorizado.");
    }

    // Busca os dados do usuário com base no email do token
    var usuario = await _context.usuarios
        .FirstOrDefaultAsync(u => u.email == userEmail);

    if (usuario == null)
    {
        return NotFound("Usuário não encontrado.");
    }

    // Retorna os dados do usuário autenticado
    return Ok(new
    {
        usuario.usuarioId,
        usuario.nome,
        usuario.email,
        usuario.permissao
    });
}


        // GET: api/Usuario
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.usuarios.ToListAsync();
        }

        // GET: api/Usuario/5
        [HttpGet("GetBy{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // PUT: api/Usuario/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("edit{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.usuarioId)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Usuario
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Add")]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            _context.usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuario", new { id = usuario.usuarioId }, usuario);
        }

        // DELETE: api/Usuario/5
        [HttpDelete("del{id}")]

        [Authorize(Roles = "Admin")] // Ambos podem acessar

        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.usuarios.Any(e => e.usuarioId == id);
        }
    }
}
