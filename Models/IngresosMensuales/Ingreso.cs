namespace PersonalFinance.Models.IngresosMensuales
{
#pragma warning disable CS8618

    using Newtonsoft.Json;
    using PersonalFinance.Models.IngresosTipo;

    /// <summary>
    /// Clase Gasto.
    /// </summary>
    public class Ingreso : Meses
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Ingreso"/> class.
        /// </summary>
        public Ingreso()
        {
        }

        /// <summary>
        /// Gets or sets propiedad Tipo.
        /// </summary>
        [JsonProperty("detalle")]
        public IngresosTipo TipoIngreso { get; set; }
    }
}