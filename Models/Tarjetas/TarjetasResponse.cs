using Newtonsoft.Json;

namespace PersonalFinance.Models.Tarjetas;

public class TarjetasResponse : GeneralResponse
{
    public TarjetasResponse() { }

    [JsonProperty("data")]
    public List<Tarjeta>? Tarjetas { get; set; }
}
