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

        public static int GeneradorNumeroPedido()
        {

            Random random = new Random();

            // 2. Definir el rango:
            // El número mínimo de 5 cifras es 10000 (inclusivo).
            // El número máximo (exclusivo) para obtener 5 cifras es 100000.
            return random.Next(10000, 100000);
        }

        public static decimal ConvertirMonto(string valor)
        {
            var montoLimpio = valor.Replace("$ ", string.Empty);
            decimal decena = 0;
            decimal decimales = 0;

            if (montoLimpio.Contains(","))
            {
                decena = decimal.Parse(montoLimpio.Split(",")[0]);
                decimales = decimal.Parse(montoLimpio.Split(",")[1]);
            }
            else
            {
                decena = decimal.Parse(montoLimpio);
            }

            return decimal.Parse($"{decena.ToString()},{decimales.ToString()}");
        }
    }
}