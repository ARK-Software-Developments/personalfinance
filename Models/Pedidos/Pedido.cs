using Newtonsoft.Json;
using System.Globalization;

namespace PersonalFinance.Models.Pedidos;

public class Pedido
{
    private string montoTotal = string.Empty;

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
    public string MontoTotal
    {
        get { return this.montoTotal; } // El 'get' devuelve el valor del campo privado
        set // El 'set' recibe un 'value'
        {
            if(value.Contains("$"))
            {
                this.montoTotal = value;
            }
            else
            {
                // Crear un objeto CultureInfo para Argentina
                CultureInfo culturaArgentina = new CultureInfo("es-AR");
                string montototal = string.Empty;
                decimal monto;
                if (value.Contains("."))
                {
                    montototal = $"{value.Split(".")[0]},{value.Split(".")[1]}"; 
                }
                else
                {
                    montototal = value;
                }

                if (decimal.TryParse(montototal, out monto))
                {
                    // Formatear el número como moneda usando la cultura argentina
                    this.montoTotal = monto.ToString("C", culturaArgentina);
                }
            }
        }
    }


    [JsonProperty("tipoRecurso")]
    public string TipoRecurso { get; set; }

    [JsonProperty("estado")]
    public Estado Estado { get; set; }

    [JsonProperty("detalles")]
    public List<PedidoDetalle> Detalles { get; set; } = [];
}
