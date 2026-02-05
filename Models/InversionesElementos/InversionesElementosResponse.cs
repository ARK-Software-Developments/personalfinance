using Newtonsoft.Json;
using PersonalFinanceApiNetCoreModel;

namespace PersonalFinance.Models.InversionesElementos;

public class InversionesElementosResponse : GeneralResponse
{
    public InversionesElementosResponse() { }

    [JsonProperty("data")]
    public List<InversionElemento>? InversionElementos { get; set; }
}
