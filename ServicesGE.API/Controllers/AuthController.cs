using Microsoft.AspNetCore.Mvc;
using ServicesGE.API.Models;
using ServicesGE.API.Services;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using ServicesGE.API.Data;

namespace ServicesGE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Usuario usuario)
        {
            if (await _context.usuarios.AnyAsync(u => u.email == usuario.email))
                return BadRequest("Email já registrado.");

            usuario.senha_hash = BCrypt.Net.BCrypt.HashPassword(usuario.senha_hash);
            _context.usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Usuário registrado com sucesso." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Usuario loginRequest)
        {
            var usuario = await _context.usuarios
                .FirstOrDefaultAsync(u => u.email == loginRequest.email);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(loginRequest.senha_hash, usuario.senha_hash))
                return Unauthorized("Email ou senha inválidos.");

            var token = TokenService.GenerateToken(usuario.nome, usuario.permissao, _configuration);
            return Ok(new { Token = token });
        }
    }
}
