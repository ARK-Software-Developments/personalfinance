using Newtonsoft.Json;
using System.Globalization;

namespace PersonalFinance.Models.IngresosTipo;

public class IngresosTipo
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("codigo")]
    public int Codigo { get; set; }

    [JsonProperty("detalle")]
    public string Detalle { get; set; }    
}
