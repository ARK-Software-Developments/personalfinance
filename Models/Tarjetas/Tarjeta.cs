using Newtonsoft.Json;
using PersonalFinanceApiNetCoreModel;

namespace PersonalFinance.Models.Tarjetas
{
    public class Tarjeta
    {
        public Tarjeta() { }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }

        [JsonProperty("fechaCierre")]
        public DateTime FechaCierre { get; set; }

        [JsonProperty("fechaVencimiento")]
        public DateTime FechaVencimiento { get; set; }

        [JsonProperty("entidad")]
        public Entidad Entidad { get; set; }

        [JsonProperty("activo")]
        public bool Activo { get; set; }
    }
}