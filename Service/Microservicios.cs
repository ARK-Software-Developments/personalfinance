using PersonalFinance.Models.Enums;
using System.Runtime.CompilerServices;

namespace PersonalFinance.Service
{
    public static class Microservicios
    {
        private readonly static string url = "https://localhost:443/api/v1/{0}/{1}";
        public static string get(ServicioEnum servicio, MetodoEnum metodo, Dictionary<string, object> keyValuePairs = null)
        {
            if ((servicio == ServicioEnum.GastosMensuales || servicio == ServicioEnum.ConsumosTarjeta) && metodo == MetodoEnum.Todos)
            {
                string tmp = url.Replace("{0}", servicio.ToRoute()).Replace("{1}", metodo.ToMethod());
                return $"{tmp}/{keyValuePairs["year"]}";
            }
            else if ((servicio == ServicioEnum.DetallePedido && metodo == MetodoEnum.DetallePedidoByOrderId) ||
                    (servicio == ServicioEnum.ConsumosTarjeta && metodo == MetodoEnum.Uno) ||
                    (servicio == ServicioEnum.Transacciones && metodo == MetodoEnum.Uno))
            {
                string tmp = url.Replace("{0}", servicio.ToRoute()).Replace("{1}", metodo.ToMethod());
                return $"{tmp}/{keyValuePairs["pId"]}";
            }
            else
            {
                return url.Replace("{0}", servicio.ToRoute()).Replace("{1}", metodo.ToMethod());
            }            
        }
    }
}