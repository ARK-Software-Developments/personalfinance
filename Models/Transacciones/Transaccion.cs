namespace PersonalFinance.Models.Transacciones
{
    using Newtonsoft.Json;
    using PersonalFinance.Models.Tarjetas;
#pragma warning disable CS8618

    using System.Text.Json.Serialization;

    /// <summary>
    /// Clase Transaccion.
    /// </summary>
    public class Transaccion : AbstractModelExternder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Transaccion"/> class.
        /// </summary>
        public Transaccion()
        {
        }

        /// <summary>
        /// Gets or sets propiedad CodigoTransaccion.
        /// </summary>
        [JsonProperty("codigoTransaccion")]
        public string CodigoTransaccion { get; set; }

        /// <summary>
        /// Gets or sets propiedad OrdenCompra.
        /// </summary>
        [JsonProperty("ordenCompra")]
        public string OrdenCompra { get; set; }

        /// <summary>
        /// Gets or sets propiedad EntidadAsociada.
        /// </summary>
        [JsonProperty("entidadAsociada")]
        public string EntidadAsociada { get; set; }

        /// <summary>
        /// Gets or sets propiedad FechaTransaccion.
        /// </summary>
        [JsonProperty("fechaTransaccion")]
        public DateTime FechaTransaccion { get; set; }

        /// <summary>
        /// Gets or sets propiedad Resumen.
        /// </summary>
        [JsonProperty("resumen")]
        public string Resumen { get; set; }

        /// <summary>
        /// Gets or sets propiedad Observaciones.
        /// </summary>
        [JsonProperty("observaciones")]
        public string Observaciones { get; set; }

        /// <summary>
        /// Gets or sets propiedad Tarjeta.
        /// </summary>
        [JsonProperty("tarjeta")]
        public Tarjeta Tarjeta { get; set; }

        /// <summary>
        /// Gets or sets propiedad TarjetaConsumoId.
        /// </summary>
        [JsonProperty("tarjetaConsumoId")]
        public int TarjetaConsumoId { get; set; }
    }
}