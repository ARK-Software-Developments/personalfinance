using Newtonsoft.Json;

namespace PersonalFinance.Models.InversionTipo;

public class InversionTipoResponse : GeneralResponse
{
    public InversionTipoResponse() { }

    [JsonProperty("data")]
    public List<PersonalFinanceApiNetCoreModel.InversionTipo>? InversionTipos { get; set; }
}
