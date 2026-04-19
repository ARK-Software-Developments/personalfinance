namespace PersonalFinance.Models.Gastos;

using Newtonsoft.Json;
using PersonalFinanceApiNetCoreModel;

public class GastoResumenCategoriaResponse : GeneralResponse
{
    public GastoResumenCategoriaResponse() { }

    [JsonProperty("data")]
    public List<GastoResumenCategoria>? ResumenCategorias { get; set; }
}
