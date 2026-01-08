namespace PersonalFinance.Models.IngresosTipo;

using Newtonsoft.Json;

public class IngresosTipoResponse : GeneralResponse
{
    public IngresosTipoResponse() { }

    [JsonProperty("data")]
    public List<IngresosTipo>? IngresosTipos { get; set; }
}
