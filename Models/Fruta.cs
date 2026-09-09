using System.Text.Json.Serialization;


namespace Trabalho_Fruteira.Models
{
    public class Fruta
    {
        public int Id { get; set; }

        public string Nome { get; set; }
            = string.Empty;

        public decimal Preco { get; set; }

        public int Estoque { get; set; }

        public int CategoriaId { get; set; }

        [JsonIgnore]
        public Categoria? Categoria { get; set; }

        [JsonIgnore]
        public ICollection<ItemPedido> ItensPedido { get; set; }
            = new List<ItemPedido>();

    }
}