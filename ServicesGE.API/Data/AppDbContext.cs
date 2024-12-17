using Microsoft.EntityFrameworkCore;
using ServicesGE.API.Models;

namespace ServicesGE.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> usuarios { get; set; }

        public DbSet<Cliente> clientes { get; set; }

        public DbSet<Fornecedor> fornecedores { get; set; }

        public DbSet<Produto> produtos { get; set; }

        public DbSet<MovimentacaoEstoque> movimentacaoestoque { get; set; }

        public DbSet<NotaFiscal> notasfiscais { get; set; }
        public DbSet<ItemNotaFiscal> itensnotafiscal { get; set; }
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // MovimentacaoEstoque
    modelBuilder.Entity<MovimentacaoEstoque>()
        .HasKey(m => m.movimentacaoid);

    // ItemNotaFiscal
    modelBuilder.Entity<ItemNotaFiscal>(entity =>
    {
        entity.HasKey(i => i.itemid);
        entity.Property(i => i.precounitario)
            .HasPrecision(18, 2); // Define precisão e escala para o campo decimal
    });

    // NotaFiscal
    modelBuilder.Entity<NotaFiscal>(entity =>
    {
        entity.HasKey(n => n.notafiscalid);
        entity.Property(n => n.valortotal)
            .HasPrecision(18, 2); // Define precisão e escala para o campo decimal
    });

    // Produto
    modelBuilder.Entity<Produto>(entity =>
    {
        entity.HasKey(p => p.produtoid);
        entity.Property(p => p.preco)
            .HasPrecision(18, 2); // Define precisão e escala para o campo decimal
    });

    base.OnModelCreating(modelBuilder);
}

    }
}