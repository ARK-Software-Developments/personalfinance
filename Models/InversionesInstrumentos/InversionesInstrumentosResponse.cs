using Newtonsoft.Json;
using PersonalFinanceApiNetCoreModel;

namespace PersonalFinance.Models.InversionesInstrumentos;

public class InversionesInstrumentosResponse : GeneralResponse
{
    public InversionesInstrumentosResponse() { }

    [JsonProperty("data")]
    public List<InversionInstrumento>? InversionInstrumentos { get; set; }
}
