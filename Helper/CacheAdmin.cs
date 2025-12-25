using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PersonalFinance.Models.Enums;

namespace PersonalFinance.Helper
{
    public class CacheAdmin
    {
        private readonly HttpContext httpContext;

        public CacheAdmin(HttpContext httpContext)
        {
            this.httpContext = httpContext;
        }

        public T Obtener<T>(ServicioEnum servicioEnum)
        {
            var cache = this.httpContext.Session.GetString(servicioEnum.ToCache());
            if (cache != null)
            {
                return (T)Convert.ChangeType(JsonConvert.DeserializeObject<T>(cache), typeof(T));
            }
            else
            {
                return (T)Convert.ChangeType(null, typeof(T));
            }
        }

        public static void Remove(HttpContext httpContext, ServicioEnum servicioEnum)
        {
            httpContext.Session.Remove(servicioEnum.ToCache());
        }

        public static void Remove(HttpContext httpContext, string nombre)
        {
            httpContext.Session.Remove(nombre);
        }

        public static void Set(HttpContext httpContext, ServicioEnum servicioEnum, string jsonResponse)
        {
            httpContext.Session.SetString(servicioEnum.ToCache(), jsonResponse);
        }

        public static void Set(HttpContext httpContext, string nombre, string data)
        {
            httpContext.Session.SetString(nombre, data);
        }

        public static bool Existe(HttpContext httpContext, string nombre)
        {
            return httpContext.Session.GetString(nombre) != null ? true : false;
        }

        public static string Obtener(HttpContext httpContext, string nombre)
        {
            return httpContext.Session.GetString(nombre);
        }
    }
}
