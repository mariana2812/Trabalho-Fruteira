using System.Text.Json.Serialization;

namespace Trabalho_Fruteira.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }

        [JsonIgnore]
        public ICollection<Fruta> Frutas { get; set; } 
            = new List<Fruta>();
    }
}
