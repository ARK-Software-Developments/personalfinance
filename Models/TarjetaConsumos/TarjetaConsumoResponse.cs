using Newtonsoft.Json;

namespace PersonalFinance.Models.TarjetaConsumos;

public class TarjetaConsumoResponse : GeneralResponse
{
    public TarjetaConsumoResponse() { }

    [JsonProperty("data")]
    public List<TarjetaConsumo>? TarjetaConsumos { get; set; }
}
