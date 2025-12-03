using Newtonsoft.Json;

namespace PersonalFinance.Models.Pedidos;

public class PedidoDetalle
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("numeroOrden")]
    public int NumeroOrden { get; set; }

    [JsonProperty("marca")]
    public string Marca { get; set; }

    [JsonProperty("productoDetalle")]
    public string ProductoDetalle { get; set; }

    [JsonProperty("descripcion")]
    public string Descripcion { get; set; }

    [JsonProperty("codigoProducto")]
    public string CodigoProducto { get; set; }

    [JsonProperty("cantidad")]
    public int Cantidad { get; set; }

    [JsonProperty("montoUnitario")]
    public decimal MontoUnitario { get; set; }

    [JsonProperty("subTotal")]
    public decimal Subtotal { get; set; }

    [JsonProperty("para")]
    public string Para { get; set; }

    [JsonProperty("estado")]
    public string Estado { get; set; }

    [JsonProperty("pedidoId")]
    public int PedidoId { get; set; }
}
