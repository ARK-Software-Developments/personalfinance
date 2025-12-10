
namespace PersonalFinance.Models.Categorias;

using Newtonsoft.Json;

public class CategoriasResponse : GeneralResponse
{
    public CategoriasResponse() { }

    [JsonProperty("data")]
    public List<Categoria>? Categoria { get; set; }
}
