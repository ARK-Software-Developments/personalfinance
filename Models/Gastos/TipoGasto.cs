using Microsoft.AspNetCore.Mvc;
using PersonalFinance.Models.Categorias;
using System.Text.Json.Serialization;

namespace PersonalFinance.Models.Gastos
{
    /// <summary>
    /// Clase TipoGasto.
    /// </summary>
    public class TipoGasto : AbstractModelExternder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TipoGasto"/> class.
        /// </summary>
        public TipoGasto()
        {
        }

        /// <summary>
        /// Gets or sets propiedad Tipo.
        /// </summary>
        public string? Tipo { get; set; }

        /// <summary>
        /// Gets or sets propiedad Categoria.
        /// </summary>
        public Categoria? Categoria { get; set; }
    }
}
