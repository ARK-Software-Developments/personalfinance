using Newtonsoft.Json;

namespace PersonalFinance.Models.Entidades;

public class Entidad
{
    public Entidad() { }

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("nombre")]
    public string Nombre { get; set; }

    [JsonProperty("tipo")]
    public string Tipo { get; set; }
}
