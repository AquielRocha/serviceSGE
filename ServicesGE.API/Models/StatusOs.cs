using System.ComponentModel.DataAnnotations;

namespace ServicesGE.API.Models
{
    public class StatusOrdemServico
    {
                [Key]
                public int statusid { get; set; } // Chave primária
        public string descricao { get; set; } = string.Empty; // Ex.: Aberta, Em Progresso, Concluída
    }
}
