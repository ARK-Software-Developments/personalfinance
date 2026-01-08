using PersonalFinance.Models.Tarjetas;
using PersonalFinance.Models.Transacciones;
using System.Text.Json.Serialization;

namespace PersonalFinance.Models.TarjetaConsumos
{
    public class TarjetaConsumoResumen : Meses
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TarjetaConsumoResumen"/> class.
        /// </summary>
        public TarjetaConsumoResumen()
        {
        }

        /// <summary>
        /// Gets or sets propiedad EntidadCompra.
        /// </summary>
        public string EntidadCompra { get; set; }
    }
}