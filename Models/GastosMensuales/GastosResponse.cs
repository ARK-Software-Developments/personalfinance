namespace PersonalFinance.Models.Gastos;

using Newtonsoft.Json;
using PersonalFinanceApiNetCoreModel;

public class GastosResponse : GeneralResponse
{
    public GastosResponse() { }

    [JsonProperty("data")]
    public List<Gasto>? Gastos { get; set; }
}
