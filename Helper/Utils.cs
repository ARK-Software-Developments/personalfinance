using Newtonsoft.Json;
using PersonalFinance.Models.Pedidos;

namespace PersonalFinance.Helper
{
    public static class Utils
    {
        public static T ObtenerResponseFromCache<T>(HttpContext httpContext, string key)
        {
            var cache = httpContext.Session.GetString(key);

            var response = JsonConvert.DeserializeObject<T>(cache);

            switch (key)
            {
                case "dataPedidos":
                    return (T)response;

                default:
                    object obj = null;

                    return (T)obj;

            }
            
        }

        public static string isSelected(string valor)
        {

            return "selected";
        }
    }
}