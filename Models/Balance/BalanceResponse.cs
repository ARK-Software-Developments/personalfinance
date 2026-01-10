using Newtonsoft.Json;

namespace PersonalFinance.Models.Balance;

public class BalanceResponse : GeneralResponse
{
    public BalanceResponse() { }

    [JsonProperty("data")]
    public List<Balance>? Balances { get; set; }
}
