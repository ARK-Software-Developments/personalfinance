using Newtonsoft.Json;

namespace PersonalFinance.Models.Pedidos;

public class Estado
{
    public Estado() { }

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("orden")]
    public int Orden { get; set; }

    [JsonProperty("nombre")]
    public string Nombre { get; set; }

    [JsonProperty("tabla")]
    public string Tabla { get; set; }

}
