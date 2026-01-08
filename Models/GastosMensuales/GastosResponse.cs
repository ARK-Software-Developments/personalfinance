namespace PersonalFinance.Models.Gastos;

using Newtonsoft.Json;

public class GastosResponse : GeneralResponse
{
    public GastosResponse() { }

    [JsonProperty("data")]
    public List<Gasto>? Gastos { get; set; }
}
