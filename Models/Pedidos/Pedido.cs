using Newtonsoft.Json;

namespace PersonalFinance.Models.Pedidos;

public class Pedido
{
    public Pedido() { }

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("numero")]
    public int Numero { get; set; }

    [JsonProperty("fechaPedido")]
    public DateTime? FechaPedido { get; set; }

    [JsonProperty("fechaRecibido")]
    public DateTime? FechaRecibido { get; set; }

    [JsonProperty("montoTotal")]
    public decimal MontoTotal { get; set; }

    [JsonProperty("tipoRecurso")]
    public string TipoRecurso { get; set; }

    [JsonProperty("estado")]
    public Estado Estado { get; set; }

    [JsonProperty("detalles")]
    public List<PedidoDetalle> Detalles { get; set; } = [];
}
