using System.Text.Json.Serialization;

namespace Trabalho_Fruteira.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public DateTime DataPediddo { get; set; }
        public decimal ValorTotal { get; set; }

        [JsonIgnore]
        public Cliente? Cliente { get; set; }
        public ICollection<ItemPedido> ItensPedido { get; set; }
            = new List<ItemPedido>();
    }
}
