using Newtonsoft.Json;
using PersonalFinanceApiNetCoreModel;
using PersonalFinance.Models.Gastos;

namespace PersonalFinance.Models.Pagos
{
    public class Pago
    {
        public Pago() { }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("fechaRegistro")]
        public DateTime? FechaRegistro { get; set; }

        [JsonProperty("fechaPago")]
        public DateTime? FechaPago { get; set; }

        [JsonProperty("codigoRegistro")]
        public string CodigoRegistro { get; set; }

        [JsonProperty("recursoDelPago")]
        public Entidad RecursoDelPago { get; set; }

        [JsonProperty("tipoDePago")]
        public string TipoDePago { get; set; }

        [JsonProperty("montoPresupuestado")]
        public decimal MontoPresupuestado { get; set; }

        [JsonProperty("montoPagado")]
        public decimal MontoPagado { get; set; }

        [JsonProperty("tipoDeGasto")]
        public TipoGasto TipoDeGasto { get; set; }
    }
}