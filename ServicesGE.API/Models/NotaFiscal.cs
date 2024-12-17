using System.ComponentModel.DataAnnotations;

namespace ServicesGE.API.Models
{
    public class NotaFiscal
    {
        public int notafiscalid { get; set; }
        public int clienteid { get; set; }
        public int? transportadoraid { get; set; } // Opcional
        public decimal valortotal { get; set; }
        public DateTime dataemissao { get; set; } = DateTime.Now;

        // Relacionamentos
        public Cliente Cliente { get; set; }
        public List<ItemNotaFiscal> ItensNotaFiscal { get; set; } = new List<ItemNotaFiscal>();
    }

    public class ItemNotaFiscal
    {

                [Key] // Define a chave primária
        public int itemid { get; set; }
        public int notafiscalid { get; set; }
        public int produtoid { get; set; }
        public int quantidade { get; set; }
        public decimal precounitario { get; set; }
        public decimal subtotal => quantidade * precounitario;

        // Relacionamentos
        public NotaFiscal NotaFiscal { get; set; }
        public Produto Produto { get; set; }
    }
}
