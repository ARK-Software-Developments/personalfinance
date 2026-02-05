namespace PersonalFinance.Helper
{
#pragma warning disable CS8601 // Posible asignación de referencia nula
#pragma warning disable CS8604 // Posible argumento de referencia nulo

    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    using PersonalFinance.Models;
    using PersonalFinance.Models.Balance;
    using PersonalFinanceApiNetCoreModel;
    using PersonalFinance.Models.Enums;
    using PersonalFinance.Models.Gastos;
    using PersonalFinance.Models.IngresosMensuales;
    using PersonalFinance.Models.IngresosTipo;
    using PersonalFinance.Models.Pagos;
    using PersonalFinance.Models.TarjetaConsumos;
    using PersonalFinance.Models.Tarjetas;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text.RegularExpressions;

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

                case "Ingreso":
                    var ing = ((T)entidad as Ingreso);
                    meses.Enero = ing!.Enero;
                    meses.Febrero = ing!.Febrero;
                    meses.Marzo = ing!.Marzo;
                    meses.Abril = ing!.Abril;
                    meses.Mayo = ing!.Mayo;
                    meses.Junio = ing!.Junio;
                    meses.Julio = ing!.Julio;
                    meses.Agosto = ing!.Agosto;
                    meses.Septiembre = ing!.Septiembre;
                    meses.Octubre = ing!.Octubre;
                    meses.Noviembre = ing!.Noviembre;
                    meses.Diciembre = ing!.Diciembre;
                    break;

                case "Balance":
                    var b = ((T)entidad as Balance);
                    meses.Enero = b!.Enero;
                    meses.Febrero = b!.Febrero;
                    meses.Marzo = b!.Marzo;
                    meses.Abril = b!.Abril;
                    meses.Mayo = b!.Mayo;
                    meses.Junio = b!.Junio;
                    meses.Julio = b!.Julio;
                    meses.Agosto = b!.Agosto;
                    meses.Septiembre = b!.Septiembre;
                    meses.Octubre = b!.Octubre;
                    meses.Noviembre = b!.Noviembre;
                    meses.Diciembre = b!.Diciembre;
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
                case ServicioEnum.GastosMensuales:

                    object gasto = new Gasto()
                    {
                        Id = form.ContainsKey("Id") && !string.IsNullOrEmpty(form["Id"]) ? int.Parse(form["Id"]) : 0,
                        Enero = form.ContainsKey("Enero") && !string.IsNullOrEmpty(form["Enero"]) ? ConvertirMonto(form["Enero"]) : 0,
                        Febrero = form.ContainsKey("Febrero") && !string.IsNullOrEmpty(form["Febrero"]) ? ConvertirMonto(form["Febrero"]) : 0,
                        Marzo = form.ContainsKey("Marzo") && !string.IsNullOrEmpty(form["Marzo"]) ? ConvertirMonto(form["Marzo"]) : 0,
                        Abril = form.ContainsKey("Abril") && !string.IsNullOrEmpty(form["Abril"]) ? ConvertirMonto(form["Abril"]) : 0,
                        Mayo = form.ContainsKey("Mayo") && !string.IsNullOrEmpty(form["Mayo"]) ? ConvertirMonto(form["Mayo"]) : 0,
                        Junio = form.ContainsKey("Junio") && !string.IsNullOrEmpty(form["Junio"]) ? ConvertirMonto(form["Junio"]) : 0,
                        Julio = form.ContainsKey("Julio") && !string.IsNullOrEmpty(form["Julio"]) ? ConvertirMonto(form["Julio"]) : 0,
                        Agosto = form.ContainsKey("Agosto") && !string.IsNullOrEmpty(form["Agosto"]) ? ConvertirMonto(form["Agosto"]) : 0,
                        Septiembre = form.ContainsKey("Septiembre") && !string.IsNullOrEmpty(form["Septiembre"]) ? ConvertirMonto(form["Septiembre"]) : 0,
                        Octubre = form.ContainsKey("Octubre") && !string.IsNullOrEmpty(form["Octubre"]) ? ConvertirMonto(form["Octubre"]) : 0,
                        Noviembre = form.ContainsKey("Noviembre") && !string.IsNullOrEmpty(form["Noviembre"]) ? ConvertirMonto(form["Noviembre"]) : 0,
                        Diciembre = form.ContainsKey("Diciembre") && !string.IsNullOrEmpty(form["Enero"]) ? ConvertirMonto(form["Diciembre"]) : 0,
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
                        Id = form.ContainsKey("Id") && !string.IsNullOrEmpty(form["Id"]) ? int.Parse(form["Id"]) : 0,
                        Enero = form.ContainsKey("Enero") && !string.IsNullOrEmpty(form["Enero"]) ? ConvertirMonto(form["Enero"]) : 0,
                        Febrero = form.ContainsKey("Febrero") && !string.IsNullOrEmpty(form["Febrero"]) ? ConvertirMonto(form["Febrero"]) : 0,
                        Marzo = form.ContainsKey("Marzo") && !string.IsNullOrEmpty(form["Marzo"]) ? ConvertirMonto(form["Marzo"]) : 0,
                        Abril = form.ContainsKey("Abril") && !string.IsNullOrEmpty(form["Abril"]) ? ConvertirMonto(form["Abril"]) : 0,
                        Mayo = form.ContainsKey("Mayo") && !string.IsNullOrEmpty(form["Mayo"]) ? ConvertirMonto(form["Mayo"]) : 0,
                        Junio = form.ContainsKey("Junio") && !string.IsNullOrEmpty(form["Junio"]) ? ConvertirMonto(form["Junio"]) : 0,
                        Julio = form.ContainsKey("Julio") && !string.IsNullOrEmpty(form["Julio"]) ? ConvertirMonto(form["Julio"]) : 0,
                        Agosto = form.ContainsKey("Agosto") && !string.IsNullOrEmpty(form["Agosto"]) ? ConvertirMonto(form["Agosto"]) : 0,
                        Septiembre = form.ContainsKey("Septiembre") && !string.IsNullOrEmpty(form["Septiembre"]) ? ConvertirMonto(form["Septiembre"]) : 0,
                        Octubre = form.ContainsKey("Octubre") && !string.IsNullOrEmpty(form["Octubre"]) ? ConvertirMonto(form["Octubre"]) : 0,
                        Noviembre = form.ContainsKey("Noviembre") && !string.IsNullOrEmpty(form["Noviembre"]) ? ConvertirMonto(form["Noviembre"]) : 0,
                        Diciembre = form.ContainsKey("Diciembre") && !string.IsNullOrEmpty(form["Enero"]) ? ConvertirMonto(form["Diciembre"]) : 0,
                        Ano = form.ContainsKey("Ano") && !string.IsNullOrEmpty(form["Ano"]) ? int.Parse(form["Ano"]) : DateTime.Now.Year,
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
                        Id = form.ContainsKey("Id") && !string.IsNullOrEmpty(form["Id"]) ? int.Parse(form["Id"]) : 0,
                        CodigoRegistro = form["CodigoRegistro"].ToString(),
                        FechaPago = DateTime.ParseExact(form["FechaPago"], "yyyy-MM-dd", cultureInfor),
                        FechaRegistro = DateTime.ParseExact(form["FechaRegistro"], "yyyy-MM-dd", cultureInfor),
                        MontoPagado = ConvertirMonto(form["MontoPagado"]),
                        MontoPresupuestado = ConvertirMonto(form["MontoPresupuestado"]),
                        TipoDePago = form["TipoDePago"].ToString(),
                        RecursoDelPago = form.ContainsKey("RecursoDelPagoSel") ?
                        new Entidad()
                        {
                            Id = int.Parse(form["RecursoDelPagoSel"]),
                        } : null,
                        TipoDeGasto = form.ContainsKey("TipoDeGastoSel") ?
                        new TipoGasto()
                        {
                            Id = int.Parse(form["TipoDeGastoSel"]),
                        } : null,
                    };

                    return (T)pago;

                case ServicioEnum.Ingresos:
                    object ingreso = new Ingreso()
                    {
                        Id = form.ContainsKey("Id") && !string.IsNullOrEmpty(form["Id"]) ? int.Parse(form["Id"]) : 0,
                        Enero = form.ContainsKey("Enero") && !string.IsNullOrEmpty(form["Enero"]) ? ConvertirMonto(form["Enero"]) : 0,
                        Febrero = form.ContainsKey("Febrero") && !string.IsNullOrEmpty(form["Febrero"]) ? ConvertirMonto(form["Febrero"]) : 0,
                        Marzo = form.ContainsKey("Marzo") && !string.IsNullOrEmpty(form["Marzo"]) ? ConvertirMonto(form["Marzo"]) : 0,
                        Abril = form.ContainsKey("Abril") && !string.IsNullOrEmpty(form["Abril"]) ? ConvertirMonto(form["Abril"]) : 0,
                        Mayo = form.ContainsKey("Mayo") && !string.IsNullOrEmpty(form["Mayo"]) ? ConvertirMonto(form["Mayo"]) : 0,
                        Junio = form.ContainsKey("Junio") && !string.IsNullOrEmpty(form["Junio"]) ? ConvertirMonto(form["Junio"]) : 0,
                        Julio = form.ContainsKey("Julio") && !string.IsNullOrEmpty(form["Julio"]) ? ConvertirMonto(form["Julio"]) : 0,
                        Agosto = form.ContainsKey("Agosto") && !string.IsNullOrEmpty(form["Agosto"]) ? ConvertirMonto(form["Agosto"]) : 0,
                        Septiembre = form.ContainsKey("Septiembre") && !string.IsNullOrEmpty(form["Septiembre"]) ? ConvertirMonto(form["Septiembre"]) : 0,
                        Octubre = form.ContainsKey("Octubre") && !string.IsNullOrEmpty(form["Octubre"]) ? ConvertirMonto(form["Octubre"]) : 0,
                        Noviembre = form.ContainsKey("Noviembre") && !string.IsNullOrEmpty(form["Noviembre"]) ? ConvertirMonto(form["Noviembre"]) : 0,
                        Diciembre = form.ContainsKey("Diciembre") && !string.IsNullOrEmpty(form["Enero"]) ? ConvertirMonto(form["Diciembre"]) : 0,
                        Ano = form.ContainsKey("Ano") ? int.Parse(form["Ano"]) : DateTime.Now.Year,                        
                        TipoIngreso = new IngresosTipo()
                        {
                            Id = int.Parse(form["TipoIngresoSel"]),
                        },
                    };

                    return (T)ingreso;

                case ServicioEnum.Inversiones:
                    object inversion = new Inversion()
                    {
                        Id = form.ContainsKey("Id") && !string.IsNullOrEmpty(form["Id"]) ? int.Parse(form["Id"]) : 0,
                        FechaOperacion = form.ContainsKey("FechaOperacion") ? DateTime.ParseExact(form["FechaOperacion"], "yyyy-MM-dd", cultureInfor) : DateTime.Now,
                        FechaActualizacion = form.ContainsKey("FechaActualizacion") ? DateTime.ParseExact(form["FechaActualizacion"], "yyyy-MM-dd", cultureInfor) : DateTime.Now,
                        MontoGanado = form.ContainsKey("MontoGanado") && !string.IsNullOrEmpty(form["MontoGanado"]) ? ConvertirMonto(form["MontoGanado"]) : 0,
                        MontoInvertido = form.ContainsKey("MontoInvertido") && !string.IsNullOrEmpty(form["MontoInvertido"]) ? ConvertirMonto(form["MontoInvertido"]) : 0,
                        Estado = form["Estado"].ToString() == "1" ? "ACTIVO" : "COMPLETADO",
                        Entidad = new Entidad()
                        {
                            Id = int.Parse(form["Entidad"]),
                        },
                        Tipo = new InversionTipo()
                        {
                            Id = int.Parse(form["InversionTipo"]),
                        },
                    };

                    return (T)inversion;

                case ServicioEnum.InversionesElementos:
                    object inversionElemento = new InversionElemento()
                    {
                        Id = form.ContainsKey("Id") && !string.IsNullOrEmpty(form["Id"]) ? int.Parse(form["Id"]) : 0,
                        FechaOperacion = form.ContainsKey("FechaOperacion") ? DateTime.ParseExact(form["FechaOperacion"], "yyyy-MM-dd", cultureInfor) : DateTime.Now,
                        Cantidad = form.ContainsKey("Cantidad") && !string.IsNullOrEmpty(form["Cantidad"]) ? int.Parse(form["Cantidad"]) : 0,
                        MontoImpuestos = form.ContainsKey("MontoImpuestos") && !string.IsNullOrEmpty(form["MontoImpuestos"]) ? ConvertirMonto(form["MontoImpuestos"]) : 0,
                        MontoInvertido = form.ContainsKey("MontoInvertido") && !string.IsNullOrEmpty(form["MontoInvertido"]) ? ConvertirMonto(form["MontoInvertido"]) : 0,
                        MontoResultado = form.ContainsKey("MontoResultado") && !string.IsNullOrEmpty(form["EMontoResultadonero"]) ? ConvertirMonto(form["MontoResultado"]) : 0,
                        MontoUnitario = form.ContainsKey("MontoUnitario") && !string.IsNullOrEmpty(form["MontoUnitario"]) ? ConvertirMonto(form["MontoUnitario"]) : 0,
                        NumeroOperacion = form.ContainsKey("NumeroOperacion") && !string.IsNullOrEmpty(form["NumeroOperacion"]) ? form["NumeroOperacion"].ToString() : string.Empty,
                        Instrumento = new InversionInstrumento()
                        {
                            Id = form.ContainsKey("Instrumento") && !string.IsNullOrEmpty(form["Instrumento"]) ? int.Parse(form["Instrumento"]) : 0,
                        },
                        Inversion = new Inversion()
                        {
                            Id = form.ContainsKey("InversionId") && !string.IsNullOrEmpty(form["InversionId"]) ? int.Parse(form["InversionId"]) : 0,
                        },
                        Estado = form.ContainsKey("Estado") && !string.IsNullOrEmpty(form["Estado"]) ? (form["Estado"].ToString() == "1" ? "ACTIVO" : "COMPLETADO") : "ACTIVO",
                    };

                    return (T)inversionElemento;

                default:
                    return (T)new object();
            }
            
        }
        
        public static string LetraCapital(string cadena)
        {
            var result = Regex.Replace(cadena.ToLower(), @"\b(\w)", m => m.Value.ToUpper());
            return Regex.Replace(result, @"(\s(of|in|by|and)|\'[st])\b", m => m.Value.ToLower(), RegexOptions.IgnoreCase);
        }

        public static int GetYear(HttpContext httpContext, int selectYear = 0)
        {
            int year = 0;
            if(selectYear == 0)
            {
                if (CacheAdmin.Existe(httpContext, "Year"))
                {
                    year = int.Parse(httpContext.Session.GetString("Year"));
                }
                else
                {
                    year = DateTime.Now.Year;
                    httpContext.Session.SetString("Year", year.ToString());
                }
            }
            else
            {
                year = selectYear;
                httpContext.Session.SetString("Year", year.ToString());
            }

            return year;
        }
    }
}