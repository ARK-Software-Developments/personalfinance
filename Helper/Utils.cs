namespace PersonalFinance.Helper
{
#pragma warning disable CS8601 // Posible asignación de referencia nula
#pragma warning disable CS8604 // Posible argumento de referencia nulo

    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    using PersonalFinance.Models;
    using PersonalFinance.Models.Entidades;
    using PersonalFinance.Models.Enums;
    using PersonalFinance.Models.Gastos;
    using PersonalFinance.Models.Pagos;
    using PersonalFinance.Models.TarjetaConsumos;
    using PersonalFinance.Models.Tarjetas;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using static System.Runtime.InteropServices.JavaScript.JSType;

    public static class Utils
    {
        public static IFormatProvider cultureInfor = CultureInfo.InvariantCulture;
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

        public static Meses CargarMeses<T>(T entidad)
        {
            Meses meses = new ();
            switch (entidad?.GetType().Name)
            {
                case "Gasto":
                    var g = ((T)entidad as Gasto);
                    meses.Enero = g!.Enero;
                    meses.Febrero = g!.Febrero;
                    meses.Marzo = g!.Marzo;
                    meses.Abril = g!.Abril;
                    meses.Mayo = g!.Mayo;
                    meses.Junio = g!.Junio;
                    meses.Julio = g!.Julio;
                    meses.Agosto = g!.Agosto;
                    meses.Septiembre = g!.Septiembre;
                    meses.Octubre = g!.Octubre;
                    meses.Noviembre = g!.Noviembre;
                    meses.Diciembre = g!.Diciembre;
                    break;

                case "TarjetaConsumo":
                    var tc = ((T)entidad as TarjetaConsumo);
                    meses.Enero = tc!.Enero;
                    meses.Febrero = tc!.Febrero;
                    meses.Marzo = tc!.Marzo;
                    meses.Abril = tc!.Abril;
                    meses.Mayo = tc!.Mayo;
                    meses.Junio = tc!.Junio;
                    meses.Julio = tc!.Julio;
                    meses.Agosto = tc!.Agosto;
                    meses.Septiembre = tc!.Septiembre;
                    meses.Octubre = tc!.Octubre;
                    meses.Noviembre = tc!.Noviembre;
                    meses.Diciembre = tc!.Diciembre;
                    break;
            }
            

            return meses;
        }
        public static Dictionary<string, decimal> CalcularDiferenciasMeses(int mesActual, Meses entidad)
        {
            Dictionary<string, decimal> result = new ();

            switch (mesActual)
            {
                case 1:
                    result.Add("anterior", 0);
                    result.Add("actual", entidad.Enero);
                    result.Add("siguiente", entidad.Febrero);
                    break;

                case 2:
                    result.Add("anterior", entidad.Enero);
                    result.Add("actual", entidad.Febrero);
                    result.Add("siguiente", entidad.Marzo);
                    break;

                case 3:
                    result.Add("anterior", entidad.Febrero);
                    result.Add("actual", entidad.Marzo);
                    result.Add("siguiente", entidad.Abril);
                    break;

                case 4:
                    result.Add("anterior", entidad.Marzo);
                    result.Add("actual", entidad.Abril);
                    result.Add("siguiente", entidad.Mayo);

                    break;

                case 5:
                    result.Add("anterior", entidad.Abril);
                    result.Add("actual", entidad.Mayo);
                    result.Add("siguiente", entidad.Junio);
                    break;

                case 6:
                    result.Add("anterior", entidad.Mayo);
                    result.Add("actual", entidad.Junio);
                    result.Add("siguiente", entidad.Julio);
                    break;

                case 7:
                    result.Add("anterior", entidad.Junio);
                    result.Add("actual", entidad.Julio);
                    result.Add("siguiente", entidad.Agosto);
                    break;

                case 8:
                    result.Add("anterior", entidad.Julio);
                    result.Add("actual", entidad.Agosto);
                    result.Add("siguiente", entidad.Septiembre);
                    break;

                case 9:
                    result.Add("anterior", entidad.Agosto);
                    result.Add("actual", entidad.Septiembre);
                    result.Add("siguiente", entidad.Octubre);
                    break;

                case 10:
                    result.Add("anterior", entidad.Septiembre);
                    result.Add("actual", entidad.Octubre);
                    result.Add("siguiente", entidad.Noviembre);
                    break;

                case 11:
                    result.Add("anterior", entidad.Octubre);
                    result.Add("actual", entidad.Noviembre);
                    result.Add("siguiente", entidad.Diciembre);
                    break;

                case 12:
                    result.Add("anterior", entidad.Noviembre);
                    result.Add("actual", entidad.Diciembre);
                    result.Add("siguiente", 0);
                    break;
            }
            
            return result;
        }       
   
        public static T MapRequest<T>(IFormCollection form, ServicioEnum servicioEnum)
        {
            switch (servicioEnum)
            {
                case ServicioEnum.Gastos:

                    object gasto = new Gasto()
                    {
                        Id = int.Parse(form["Id"]),
                        Enero = decimal.Parse(form["Enero"], cultureInfor),
                        Febrero = decimal.Parse(form["Febrero"], cultureInfor),
                        Marzo = decimal.Parse(form["Marzo"], cultureInfor),
                        Abril = decimal.Parse(form["Abril"], cultureInfor),
                        Mayo = decimal.Parse(form["Mayo"], cultureInfor),
                        Junio = decimal.Parse(form["Junio"], cultureInfor),
                        Julio = decimal.Parse(form["Julio"], cultureInfor),
                        Agosto = decimal.Parse(form["Agosto"], cultureInfor),
                        Septiembre = decimal.Parse(form["Septiembre"], cultureInfor),
                        Octubre = decimal.Parse(form["Octubre"], cultureInfor),
                        Noviembre = decimal.Parse(form["Noviembre"], cultureInfor),
                        Diciembre = decimal.Parse(form["Diciembre"], cultureInfor),
                        Resumen = form["Resumen"],
                        Observaciones = form["Observaciones"],
                        Villetera = new Entidad()
                        {
                            Id = int.Parse(form["VilleteraSel"]),
                        },
                        TipoGasto = new TipoGasto()
                        {
                            Id = int.Parse(form["TipoGastoSel"]),
                        },
                        Pagado = form.ContainsKey("Pagado") ? bool.Parse(form["Pagado"]) : false,
                        Reservado = form.ContainsKey("Reservado") ? bool.Parse(form["Reservado"]) : false,
                        Verificado = form.ContainsKey("Verificado") ? bool.Parse(form["Verificado"]) : false,
                        Activo = true,
                    };

                    return (T)gasto;
    
                case ServicioEnum.ConsumosTarjeta:
                    object consumo = new TarjetaConsumo()
                    {
                        Id = form.ContainsKey("Id") ? int.Parse(form["Id"]) : 0,
                        Enero = decimal.Parse(form["Enero"], cultureInfor),
                        Febrero = decimal.Parse(form["Febrero"], cultureInfor),
                        Marzo = decimal.Parse(form["Marzo"], cultureInfor),
                        Abril = decimal.Parse(form["Abril"], cultureInfor),
                        Mayo = decimal.Parse(form["Mayo"], cultureInfor),
                        Junio = decimal.Parse(form["Junio"], cultureInfor),
                        Julio = decimal.Parse(form["Julio"], cultureInfor),
                        Agosto = decimal.Parse(form["Agosto"], cultureInfor),
                        Septiembre = decimal.Parse(form["Septiembre"], cultureInfor),
                        Octubre = decimal.Parse(form["Octubre"], cultureInfor),
                        Noviembre = decimal.Parse(form["Noviembre"], cultureInfor),
                        Diciembre = decimal.Parse(form["Diciembre"], cultureInfor),
                        Ano = form.ContainsKey("Ano") ? int.Parse(form["Ano"]) : DateTime.Now.Year,
                        Cuotas = int.Parse(form["Cuotas"]),
                        Detalle = form["Detalle"].ToString(),
                        EntidadCompra = form["EntidadCompra"].ToString(),
                        Pagado = form.ContainsKey("Pagado") ? bool.Parse(form["Pagado"]) : false,
                        Verificado = form.ContainsKey("Verificado") ? bool.Parse(form["Verificado"]) : false,
                        Tarjeta = form.ContainsKey("Tarjeta") ? 
                        new Tarjeta()
                        {
                            Id = int.Parse(form["Tarjeta"]),
                        } : null,
                    };

                    return (T)consumo;

                case ServicioEnum.Pagos:
                    object pago = new Pago()
                    { 
                        Id = form.ContainsKey("Id") ? int.Parse(form["Id"]) : 0,
                        CodigoRegistro = form["CodigoRegistro"].ToString(),
                        FechaPago = DateTime.ParseExact(form["FechaPago"], "yyyy-MM-dd", cultureInfor),
                        FechaRegistro = DateTime.ParseExact(form["FechaRegistro"], "yyyy-MM-dd", cultureInfor),
                        MontoPagado = decimal.Parse(form["MontoPagado"], cultureInfor),
                        MontoPresupuestado = decimal.Parse(form["MontoPresupuestado"], cultureInfor),
                        TipoDePago = form["TipoDePago"].ToString(),
                        RecursoDelPago = form.ContainsKey("EntidadSel") ?
                        new Entidad()
                        {
                            Id = int.Parse(form["EntidadSel"]),
                        } : null,
                        TipoDeGasto = form.ContainsKey("RecursoDelPagoSel") ?
                        new TipoGasto()
                        {
                            Id = int.Parse(form["RecursoDelPagoSel"]),
                        } : null,
                    };

                    return (T)pago;

                default:
                    return (T)new object();
            }
            
        }
        
        public static string LetraCapital(string cadena)
        {
            var result = Regex.Replace(cadena.ToLower(), @"\b(\w)", m => m.Value.ToUpper());
            return Regex.Replace(result, @"(\s(of|in|by|and)|\'[st])\b", m => m.Value.ToLower(), RegexOptions.IgnoreCase);
        }
    }
}