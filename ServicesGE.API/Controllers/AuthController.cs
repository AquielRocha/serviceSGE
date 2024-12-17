using Microsoft.AspNetCore.Mvc;
using ServicesGE.API.Models;
using ServicesGE.API.Services;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using ServicesGE.API.Data;
using ServicesGE.API.DTOs;

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
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDTO loginRequest)
        {
            if (string.IsNullOrWhiteSpace(loginRequest.Email) || string.IsNullOrWhiteSpace(loginRequest.Senha))
            {
                return BadRequest("Email e senha são obrigatórios.");
            }

            var usuario = await _context.usuarios
                .FirstOrDefaultAsync(u => u.email == loginRequest.Email);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Senha, usuario.senha_hash))
            {
                return Unauthorized("Email ou senha inválidos.");
            }

            var token = TokenService.GenerateToken(usuario.nome, usuario.permissao, usuario.email, _configuration);

            return Ok(new
            {
                Message = "Login realizado com sucesso.",
                Token = token
            });
        }

    }
}
