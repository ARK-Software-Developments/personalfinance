using Newtonsoft.Json;

namespace PersonalFinance.Models
{
#pragma warning disable CS8618
#pragma warning disable SA1600

    /// <summary>
    /// Clase Parametros.
    /// </summary>
    public class Parametro
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Parametro"/> class.
        /// </summary>
        public Parametro()
        {
        }

        [JsonProperty("Nombre")]
        public string Nombre { get; set; }

        [JsonProperty("Valor")]
        public object Valor { get; set; }
    }
}