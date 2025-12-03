using Newtonsoft.Json;

namespace PersonalFinance.Models.Pedidos;

public class PedidoDetallesResponse : GeneralResponse
{
    [JsonProperty("data")]
    public List<PedidoDetalle>? Detalles { get; set; }
}
