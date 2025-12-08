using Newtonsoft.Json;

namespace PersonalFinance.Models
{
    public class GeneralRequest
    {
        public GeneralRequest() { }

        [JsonProperty("Parametros")]
        public List<Parametro> Parametros { get; set; }
    }
}
