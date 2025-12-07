using Newtonsoft.Json;

namespace PersonalFinance.Models.Pedidos;

public class PedidosResponse : GeneralResponse
{
    public PedidosResponse () { }

    [JsonProperty("data")]
    public List<Pedido>? Pedidos { get; set; }
}
