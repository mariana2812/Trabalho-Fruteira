using System.Text.Json.Serialization;


namespace Trabalho_Fruteira.Models
{
    public class Pedido
    {

        public int Id { get; set; }
        public int ClienteId { get; set; }
        public DateTime DataPedido { get; set; }
        public decimal ValorFinal { get; set; }

        [JsonIgnore]
        public Cliente? Cliente { get; set; }

        public ICollection<ItemPedido> Itens { get; set; }
            = new List<ItemPedido>();

    }
}