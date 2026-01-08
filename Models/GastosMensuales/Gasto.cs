namespace PersonalFinance.Models.Gastos
{
    using Microsoft.AspNetCore.Mvc;

#pragma warning disable CS8618
    
    using PersonalFinance.Models.Entidades;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Clase Gasto.
    /// </summary>
    public class Gasto : Meses
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Gasto"/> class.
        /// </summary>
        public Gasto()
        {
        }

        /// <summary>
        /// Gets or sets propiedad Resumen.
        /// </summary>
        public string Resumen { get; set; }

        /// <summary>
        /// Gets or sets propiedad Observaciones.
        /// </summary>
        public string Observaciones { get; set; }

        /// <summary>
        /// Gets or sets propiedad TipoGasto.
        /// </summary>
        public TipoGasto TipoGasto { get; set; }

        /// <summary>
        /// Gets or sets propiedad Villetera.
        /// </summary>
        public Entidad Villetera { get; set; }

        /// <summary>
        /// Gets or sets propiedad Verificado.
        /// </summary>
        public bool Verificado { get; set; }

        /// <summary>
        /// Gets or sets propiedad Reservado.
        /// </summary>
        public bool Reservado { get; set; }

        /// <summary>
        /// Gets or sets propiedad Pagado.
        /// </summary>
        public bool Pagado { get; set; }

        /// <summary>
        /// Gets or sets propiedad Activo.
        /// </summary>
        public bool Activo { get; set; }
    }
}