using Newtonsoft.Json;

namespace PersonalFinance.Models.Balance;

public class CalendarioVencimientoResponse : GeneralResponse
{
    public CalendarioVencimientoResponse() { }

    [JsonProperty("data")]
    public List<CalendarioVencimiento>? CalendarioVencimientos { get; set; }
}
