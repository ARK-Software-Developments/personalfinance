
namespace PersonalFinance.Models.Entidades;

using Newtonsoft.Json;

public class EntidadesResponse : GeneralResponse
{
    public EntidadesResponse() { }

    [JsonProperty("data")]
    public List<Entidad>? Entidades { get; set; }
}
