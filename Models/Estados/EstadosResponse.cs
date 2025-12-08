
namespace PersonalFinance.Models.Pedidos;

using Newtonsoft.Json;

public class EstadosResponse : GeneralResponse
{
    public EstadosResponse () { }

    [JsonProperty("data")]
    public List<Estado>? Estados { get; set; }
}
