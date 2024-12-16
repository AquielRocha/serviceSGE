using Microsoft.EntityFrameworkCore;
using ServicesGE.API.Models;

namespace ServicesGE.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> usuarios { get; set; }
    }
}
