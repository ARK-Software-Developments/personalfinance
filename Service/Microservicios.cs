using PersonalFinance.Models.Enums;
using System.Runtime.CompilerServices;

namespace PersonalFinance.Service
{
    public static class Microservicios
    {
        private readonly static string url = "https://localhost:443/api/v1/{0}/{1}";
        public static string get(ServicioEnum servicio, MetodoEnum metodo)
        {
            return url.Replace("{0}", servicio.ToRoute()).Replace("{1}", metodo.ToMethod());
        }
    }
}