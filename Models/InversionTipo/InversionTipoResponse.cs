using Newtonsoft.Json;
using PersonalFinanceApiNetCoreModel;

namespace PersonalFinance.Models.Prestamos;

public class InversionTipoResponse : GeneralResponse
{
    public InversionTipoResponse() { }

    [JsonProperty("data")]
    public List<InversionTipo>? InversionTipos { get; set; }
}
