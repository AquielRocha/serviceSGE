namespace ServicesGE.API.Models
{
    public class MovimentacaoEstoque
    {
        public int movimentacaoid { get; set; }
        public int produtoid { get; set; }
        public int quantidade { get; set; }
        public string tipomovimento { get; set; } = "Entrada"; // Entrada ou Saída
        public DateTime datamovimento { get; set; } = DateTime.Now;

        // Relacionamento com Produto
        public Produto? Produto { get; set; }
    }
}
