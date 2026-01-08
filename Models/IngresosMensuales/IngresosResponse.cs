namespace PersonalFinance.Models.IngresosMensuales;

using Newtonsoft.Json;

public class IngresosResponse : GeneralResponse
{
    public IngresosResponse() { }

    [JsonProperty("data")]
    public List<Ingreso>? Ingresos { get; set; }
}
