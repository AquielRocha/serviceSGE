namespace ServicesGE.API.Models
{
    public class ItemOrdemServico
    {
        public int itemosid { get; set; } // Chave primária
        public int osid { get; set; } // FK para OrdemServico
        public int? produtoid { get; set; } // FK para Produto (opcional)
        public string? descricao { get; set; } // Descrição do item/serviço
        public int? quantidade { get; set; } // Quantidade do produto
        public decimal? precounitario { get; set; } // Preço unitário do produto

        // Relacionamentos
        public OrdemServico OrdemServico { get; set; }
        public Produto? Produto { get; set; }
    }
}
