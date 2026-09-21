using System.Text.Json.Serialization;


namespace Trabalho_Fruteira.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        public string Nome { get; set; }
            = string.Empty;

        public string? Preferencia { get; set; }


        [JsonIgnore]
        public ICollection<Pedido> Pedidos { get; set; }
            = new List<Pedido>();

    }
}