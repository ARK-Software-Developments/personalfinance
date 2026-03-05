namespace PersonalFinance.Models.Gastos;

using Newtonsoft.Json;
using PersonalFinanceApiNetCoreModel;

public class TiposGastosResponse : GeneralResponse
{
    public TiposGastosResponse() { }

    [JsonProperty("data")]
    public List<TipoGasto>? TiposGastos { get; set; }
}
