using Newtonsoft.Json;

namespace PersonalFinance.Models
{
    public class GeneralDataResponse : GeneralResponse
    {
        [JsonProperty("data")]
        public List<object>? Data { get; set; }
    }
}
