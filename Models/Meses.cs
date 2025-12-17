using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace PersonalFinance.Models
{
    /// <summary>
    /// Clase Meses.
    /// </summary>
    public class Meses : AbstractModelExternder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Meses"/> class.
        /// </summary>
        public Meses()
        {
        }

        /// <summary>
        /// Gets or sets propiedad Categoria.
        /// </summary>
        public decimal Enero { get; set; }

        /// <summary>
        /// Gets or sets propiedad Febrero.
        /// </summary>
        public decimal Febrero { get; set; }

        /// <summary>
        /// Gets or sets propiedad Marzo.
        /// </summary>
        public decimal Marzo { get; set; }

        /// <summary>
        /// Gets or sets propiedad Abril.
        /// </summary>
        public decimal Abril { get; set; }

        /// <summary>
        /// Gets or sets propiedad Mayo.
        /// </summary>
        public decimal Mayo { get; set; }

        /// <summary>
        /// Gets or sets propiedad Junio.
        /// </summary>
        public decimal Junio { get; set; }

        /// <summary>
        /// Gets or sets propiedad Julio.
        /// </summary>
        public decimal Julio { get; set; }

        /// <summary>
        /// Gets or sets propiedad Agosto.
        /// </summary>
        public decimal Agosto { get; set; }

        /// <summary>
        /// Gets or sets propiedad Septiembre.
        /// </summary>
        public decimal Septiembre { get; set; }

        /// <summary>
        /// Gets or sets propiedad Octubre.
        /// </summary>
        public decimal Octubre { get; set; }

        /// <summary>
        /// Gets or sets propiedad Noviembre.
        /// </summary>
        public decimal Noviembre { get; set; }

        /// <summary>
        /// Gets or sets propiedad Diciembre.
        /// </summary>
        public decimal Diciembre { get; set; }

        /// <summary>
        /// Gets or sets propiedad Año.
        /// </summary>
        public int Ano { get; set; }
    }
}
