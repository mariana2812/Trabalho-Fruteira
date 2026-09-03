using System.Text.Json.Serialization;


namespace Trabalho_Fruteira.Models
{
    public class ItemPedido
    {
        public int Id { get; set; }

        public int PedidoId { get; set; }

        public int FrutaId { get; set; }

        public int Quantidade { get; set; }

        public decimal PrecoUnitario { get; set; }

        public decimal Subtotal { get; set; }

        [JsonIgnore]
        public Pedido? Pedido { get; set; }

        [JsonIgnore]
        public Fruta? Fruta { get; set; }

    }
}