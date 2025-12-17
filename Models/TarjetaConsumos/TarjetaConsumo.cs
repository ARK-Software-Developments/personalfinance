using PersonalFinance.Models.Tarjetas;
using PersonalFinance.Models.Transacciones;
using System.Text.Json.Serialization;

namespace PersonalFinance.Models.TarjetaConsumos
{
    public class TarjetaConsumo : Meses
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TarjetaConsumo"/> class.
        /// </summary>
        public TarjetaConsumo()
        {
        }

        /// <summary>
        /// Gets or sets propiedad Tarjeta.
        /// </summary>
        public Tarjeta Tarjeta { get; set; }

        /// <summary>
        /// Gets or sets propiedad EntidadCompra.
        /// </summary>
        public string EntidadCompra { get; set; }

        /// <summary>
        /// Gets or sets propiedad Transaccion.
        /// </summary>
        public Transaccion Transaccion { get; set; }

        /// <summary>
        /// Gets or sets propiedad Detalle.
        /// </summary>
        public string Detalle { get; set; }

        /// <summary>
        /// Gets or sets propiedad Cuotas.
        /// </summary>
        public int Cuotas { get; set; }

        /// <summary>
        /// Gets or sets propiedad Verificado.
        /// </summary>
        public bool Verificado { get; set; }

        /// <summary>
        /// Gets or sets propiedad Pagado.
        /// </summary>
        public bool Pagado { get; set; }
    }
}