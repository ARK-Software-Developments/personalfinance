using Newtonsoft.Json;
using PersonalFinanceApiNetCoreModel;

namespace PersonalFinance.Models.Prestamos;

public class PrestamoResponse : GeneralResponse
{
    public PrestamoResponse() { }

    [JsonProperty("data")]
    public List<Prestamo>? Prestamos { get; set; }
}
