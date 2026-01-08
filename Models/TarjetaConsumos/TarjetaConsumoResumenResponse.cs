using Newtonsoft.Json;

namespace PersonalFinance.Models.TarjetaConsumos;

public class TarjetaConsumoResumenResponse : GeneralResponse
{
    public TarjetaConsumoResumenResponse() { }

    [JsonProperty("data")]
    public List<TarjetaConsumoResumen>? TarjetaConsumoResumenes { get; set; }
}
