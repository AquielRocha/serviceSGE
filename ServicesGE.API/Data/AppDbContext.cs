using Microsoft.EntityFrameworkCore;
using ServicesGE.API.Models;

namespace ServicesGE.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSets
        public DbSet<Usuario> usuarios { get; set; }
        public DbSet<Cliente> clientes { get; set; }
        public DbSet<Fornecedor> fornecedores { get; set; }
        public DbSet<Produto> produtos { get; set; }
        public DbSet<MovimentacaoEstoque> movimentacaoestoque { get; set; }
        public DbSet<NotaFiscal> notasfiscais { get; set; }
        public DbSet<ItemNotaFiscal> itensnotafiscal { get; set; }
        public DbSet<StatusOrdemServico> status_ordem_servico { get; set; }
        public DbSet<OrdemServico> ordens_servico { get; set; }
        public DbSet<ItemOrdemServico> itens_ordem_servico { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // MovimentacaoEstoque
            modelBuilder.Entity<MovimentacaoEstoque>()
                .HasKey(m => m.movimentacaoid);

            // Produto
            modelBuilder.Entity<Produto>(entity =>
            {
                entity.HasKey(p => p.produtoid);
                entity.Property(p => p.preco)
                    .HasPrecision(18, 2); // Define precisão e escala para preços
            });

            // NotaFiscal
            modelBuilder.Entity<NotaFiscal>(entity =>
            {
                entity.HasKey(n => n.notafiscalid);
                entity.Property(n => n.valortotal)
                    .HasPrecision(18, 2); // Define precisão e escala para valor total
            });

            // ItemNotaFiscal
            modelBuilder.Entity<ItemNotaFiscal>(entity =>
            {
                entity.HasKey(i => i.itemid);
                entity.Property(i => i.precounitario)
                    .HasPrecision(18, 2); // Define precisão e escala para preços unitários
            });

            // OrdemServico
            modelBuilder.Entity<OrdemServico>(entity =>
            {
                entity.ToTable("ordens_servico"); // Nome da tabela no banco
                entity.HasKey(o => o.osid);
                entity.HasOne(o => o.Cliente)
                      .WithMany()
                      .HasForeignKey(o => o.clienteid);
                entity.HasOne(o => o.StatusOrdemServico)
                      .WithMany()
                      .HasForeignKey(o => o.statusid);
            });

            // ItemOrdemServico
            modelBuilder.Entity<ItemOrdemServico>(entity =>
            {
                entity.ToTable("itens_ordem_servico"); // Nome da tabela no banco
                entity.HasKey(i => i.itemosid);
                entity.HasOne(i => i.OrdemServico)
                      .WithMany(o => o.ItensOrdemServico)
                      .HasForeignKey(i => i.osid);
                entity.HasOne(i => i.Produto)
                      .WithMany()
                      .HasForeignKey(i => i.produtoid);
            });
        }
    }
}
