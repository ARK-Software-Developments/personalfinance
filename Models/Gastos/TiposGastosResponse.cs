namespace PersonalFinance.Models.Gastos;

using Newtonsoft.Json;

public class TiposGastosResponse : GeneralResponse
{
    public TiposGastosResponse() { }

    [JsonProperty("data")]
    public List<TipoGasto>? TiposGastos { get; set; }
}
