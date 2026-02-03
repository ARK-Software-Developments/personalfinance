using Newtonsoft.Json;
using PersonalFinanceApiNetCoreModel;

namespace PersonalFinance.Models.Prestamos;

public class InversionesResponse : GeneralResponse
{
    public InversionesResponse() { }

    [JsonProperty("data")]
    public List<Inversion>? Inversiones { get; set; }
}
