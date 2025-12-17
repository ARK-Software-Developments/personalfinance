using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PersonalFinance.Models
{
    /// <summary>
    /// Clase abstract AbstractModelExternder.
    /// </summary>
    public abstract class AbstractModelExternder
    {
        /// <summary>
        /// Gets or sets propiedad Id.
        /// </summary>
        [Required]
        public int Id { get; set; }
    }
}
