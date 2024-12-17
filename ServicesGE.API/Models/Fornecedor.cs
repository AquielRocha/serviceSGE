namespace ServicesGE.API.Models
{
    public class Fornecedor
    {
        public int fornecedorid { get; set; }
        public string nome { get; set; } = string.Empty;
        public string cnpj { get; set; } = string.Empty;
        public string? contato { get; set; }
        public string? endereco { get; set; }
        public string? telefone { get; set; }
        public string? email { get; set; }
        public DateTime datacadastro { get; set; } = DateTime.Now;
    }
}
