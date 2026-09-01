using System.Text.Json.Serialization;

namespace Trabalho_Fruteira.Models
{
    public class Fruta
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public int estoque { get; set; }
        public int categoriaId { get; set; }

        [JsonIgnore]
        public Categoria? Categoria { get; set; }

        [JsonIgnore]
        public ICollection<ItemPedido> ItensPedido { get; set; } 
            = new List<ItemPedido>();
    }
}
