using Newtonsoft.Json;
using System.Globalization;

namespace PersonalFinance.Models.Pedidos;

public class PedidoDetalle
{
    private string montoUnitario = string.Empty;
    private string subtotal = string.Empty;

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
    public string MontoUnitario 
    {
        get { return this.montoUnitario; } // El 'get' devuelve el valor del campo privado
        set // El 'set' recibe un 'value'
        {
            if (value.Contains("$"))
            {
                this.montoUnitario = value;
            }
            else
            {
                // Crear un objeto CultureInfo para Argentina
                CultureInfo culturaArgentina = new CultureInfo("es-AR");
                string montounitario = string.Empty;
                decimal monto;
                if (value.Contains("."))
                {
                    montounitario = $"{value.Split(".")[0]},{value.Split(".")[1]}";
                }
                else
                {
                    montounitario = value;
                }

                if (decimal.TryParse(montounitario, out monto))
                {
                    // Formatear el número como moneda usando la cultura argentina
                    this.montoUnitario = monto.ToString("C", culturaArgentina);
                }
            }
        }
    }

    [JsonProperty("subTotal")]
    public string Subtotal 
    {
        get { return this.subtotal; } // El 'get' devuelve el valor del campo privado
        set // El 'set' recibe un 'value'
        {
            if (value.Contains("$"))
            {
                this.subtotal = value;
            }
            else
            {
                // Crear un objeto CultureInfo para Argentina
                CultureInfo culturaArgentina = new CultureInfo("es-AR");
                string subTotal = string.Empty;
                decimal monto;
                if (value.Contains("."))
                {
                    subTotal = $"{value.Split(".")[0]},{value.Split(".")[1]}";
                }
                else
                {
                    subTotal = value;
                }

                if (decimal.TryParse(subTotal, out monto))
                {
                    // Formatear el número como moneda usando la cultura argentina
                    this.subtotal = monto.ToString("C", culturaArgentina);
                }
            }
        }
    }

    [JsonProperty("para")]
    public string Para { get; set; }

    [JsonProperty("estado")]
    public Estado Estado { get; set; }

    [JsonProperty("pedidoId")]
    public int PedidoId { get; set; }
}
