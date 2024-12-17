namespace ServicesGE.API.Models
{
    public class Cliente
    {
        public int ClienteId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? CPF_CNPJ { get; set; }
        public string? Endereco { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.Now;
    }
}
