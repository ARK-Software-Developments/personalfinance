using Newtonsoft.Json;

namespace PersonalFinance.Models.Pedidos;

public class Estado
{
    public Estado() { }

    [JsonProperty("nombre")]
    public string Nombre { get; set; }

}
