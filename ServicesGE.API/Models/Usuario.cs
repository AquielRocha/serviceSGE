namespace ServicesGE.API.Models
{
public class Usuario
{
    public int usuarioId { get; set; }
    public string? nome { get; set; }
    public string? email { get; set; }
    public string? senha_hash { get; set; }
        public string permissao { get; set; } = "Operador"; // Valor padrão para novos usuários
}

}
