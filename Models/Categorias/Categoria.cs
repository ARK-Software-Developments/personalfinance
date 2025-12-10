using Newtonsoft.Json;

namespace PersonalFinance.Models.Categorias;

public class Categoria
{
    public Categoria() { }

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("nombre")]
    public string Nombre { get; set; }
}
