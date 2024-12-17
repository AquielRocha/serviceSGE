namespace ServicesGE.API.Models
{
    public class Produto
    {
        public int produtoid { get; set; }
        public string nome { get; set; } = string.Empty;
        public string? descricao { get; set; }
        public decimal preco { get; set; }
        public int quantidadeestoque { get; set; }
        public int fornecedorid { get; set; }
        public DateTime datacadastro { get; set; } = DateTime.Now;

        // Relacionamento com Fornecedor
        public Fornecedor? Fornecedor { get; set; }
    }
}
