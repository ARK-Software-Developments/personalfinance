using Newtonsoft.Json;

namespace PersonalFinance.Models.Pagos;

public class PagosResponse : GeneralResponse
{
    public PagosResponse() { }

    [JsonProperty("data")]
    public List<Pago>? Pagos { get; set; }
}
