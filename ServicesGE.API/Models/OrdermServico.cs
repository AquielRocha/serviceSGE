namespace ServicesGE.API.Models
{
    public class OrdemServico
    {
        public int osid { get; set; } // Chave primária
        public int clienteid { get; set; } // FK para Clientes
        public int statusid { get; set; } // FK para StatusOrdemServico
        public string descricao { get; set; } = string.Empty; // Descrição da OS
        public decimal? valortotal { get; set; } // Valor total da OS
        public DateTime datacriacao { get; set; } = DateTime.Now; // Data de criação
        public DateTime? dataconclusao { get; set; } // Data de conclusão (opcional)

        // Relacionamentos
        public Cliente Cliente { get; set; }
        public StatusOrdemServico StatusOrdemServico { get; set; }
        public List<ItemOrdemServico> ItensOrdemServico { get; set; } = new List<ItemOrdemServico>();
    }
}
