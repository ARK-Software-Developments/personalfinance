using Newtonsoft.Json;
using PersonalFinanceApiNetCoreModel;

namespace PersonalFinance.Models.Prestamos;

public class PrestamoDetalleResponse : GeneralResponse
{
    public PrestamoDetalleResponse() { }

    [JsonProperty("data")]
    public List<PrestamoDetalle>? PrestamoDetalles { get; set; }
}
