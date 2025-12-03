using Newtonsoft.Json;

namespace PersonalFinance.Models;

public class GeneralResponse
{
    [JsonProperty("errores")]
    public List<object>? Errores { get; set; }

    [JsonProperty("meta")]
    public object? Meta { get; set; }
}
