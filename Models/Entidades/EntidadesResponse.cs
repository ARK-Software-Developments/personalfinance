
namespace PersonalFinance.Models.Entidades;

using Newtonsoft.Json;
using PersonalFinanceApiNetCoreModel;
public class EntidadesResponse : GeneralResponse
{
    public EntidadesResponse() { }

    [JsonProperty("data")]
    public List<Entidad>? Entidades { get; set; }
}
