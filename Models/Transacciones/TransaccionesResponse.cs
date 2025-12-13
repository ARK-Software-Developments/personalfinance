namespace PersonalFinance.Models.Transacciones;

using Newtonsoft.Json;

public class TransaccionesResponse : GeneralResponse
{
    public TransaccionesResponse() { }

    [JsonProperty("data")]
    public List<Transaccion>? Transacciones { get; set; }
}
