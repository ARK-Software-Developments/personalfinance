namespace PersonalFinance.Controllers;

#pragma warning disable CS8601 // Posible asignación de referencia nula

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PersonalFinance.Helper;
using PersonalFinance.Models;
using PersonalFinance.Models.Entidades;
using PersonalFinance.Models.Enums;
using PersonalFinance.Models.Gastos;
using PersonalFinance.Models.TarjetaConsumos;
using PersonalFinance.Models.Tarjetas;
using PersonalFinance.Models.Transacciones;
using PersonalFinance.Service;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

public class ConsumosTarjetasController : Controller
{
    private readonly string Gestion = "Administrar";
    private readonly string Modulo = "ConsumosTarjetas";
    private readonly string cacheNameDataTarjetaConsumo = "cacheDataTarjetaConsumo";
    private readonly string cacheNameDataTransacciones = "cacheDataTransacciones";
    private readonly string cacheNameDataTarjetas = "cacheDataTarjetas";
    private readonly ILogger<ConsumosTarjetasController> _logger;
    private readonly ServiceCaller serviceCaller;
    private readonly Dictionary<string, object> keyValuePairs = [];

    private GeneralDataResponse generalDataResponse = new();
    private TarjetaConsumoResponse response = new();
    private TransaccionesResponse transaccionesResponse = new();
    private TarjetasResponse tarjetasResponse = new();
    private GeneralRequest generalRequest = new();
    private string cacheTarjetaConsumo = string.Empty;
    private string cacheTarjetas = string.Empty;
    private string cacheTransacciones = string.Empty;
    
    public ConsumosTarjetasController(ILogger<ConsumosTarjetasController> logger)
    {
        _logger = logger;
        HttpClientHandler httpClientHandler = new()
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
        };
        this.serviceCaller = new ServiceCaller(new HttpClient(httpClientHandler));
        this.keyValuePairs.Add("year", 2025);
    }


    public async Task<IActionResult> Index([FromForm] TarjetaConsumo tarjetaConsumo, string action)
    {
        _logger.LogInformation("Inicializando TarjetasController => Index()");
        ViewBag.Modulo = Modulo;
        ViewBag.Title = $"{Gestion}";
        ViewBag.Message = $"Gestión de {Modulo}";

        try
        {
            cacheTarjetaConsumo = HttpContext.Session.GetString(cacheNameDataTarjetaConsumo);
            if (cacheTarjetaConsumo == null)
            {
                response = await this.serviceCaller.ObtenerRegistros<TarjetaConsumoResponse>(ServicioEnum.ConsumosTarjeta, keyValuePairs);
            }
            else
            {
                response = JsonConvert.DeserializeObject<TarjetaConsumoResponse>(cacheTarjetaConsumo);
            }

            ViewBag.TarjetaConsumos = response.TarjetaConsumos;

            return View(ViewBag);

        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.ToString());

            return View(new List<TarjetaConsumo>());
        }
    }

    [HttpPost]
    public async Task<IActionResult> ConsumosTarjetas([FromForm] TarjetaConsumo tarjetaConsumo, string action, int VilleteraSel, int TipoGastoSel)
    {
        ViewBag.Modulo = Modulo;
        ViewBag.Title = $"Formulario de {Modulo}";
        ViewBag.Message = $"{Gestion} {Modulo}";

        try
        {
            if (action == "generar" || action == "actualizar")
            {
                cacheTarjetaConsumo = HttpContext.Session.GetString(cacheNameDataTarjetaConsumo);
                if (cacheTarjetaConsumo == null)
                {
                    response = await this.serviceCaller.ObtenerRegistros<TarjetaConsumoResponse>(ServicioEnum.ConsumosTarjeta, keyValuePairs);
                }
                else
                {
                    response = JsonConvert.DeserializeObject<TarjetaConsumoResponse>(cacheTarjetaConsumo);
                }

                tarjetaConsumo = Utils.MapRequest<TarjetaConsumo>(this.Request.Form, ServicioEnum.ConsumosTarjeta);

                generalRequest = new()
                {
                    Parametros =
                        [
                         new Parametro()
                         {
                             Nombre = "pCardsId",
                             Valor = tarjetaConsumo.Id,
                         },
                         new Parametro()
                         {
                             Nombre = "pPurchasingEntity",
                             Valor = tarjetaConsumo.EntidadCompra,
                         },
                         new Parametro()
                         {
                             Nombre = "pTransactionCodeId",
                             Valor = tarjetaConsumo.Transaccion.Id,
                         },
                         new Parametro()
                         {
                             Nombre = "pDetails",
                             Valor = tarjetaConsumo.Detalle,
                         },
                         new Parametro()
                         {
                             Nombre = "pNumberInstallments",
                             Valor = tarjetaConsumo.Cuotas,
                         },
                         new Parametro()
                         {
                             Nombre = "pJanuary",
                             Valor = tarjetaConsumo.Enero,
                         },
                         new Parametro()
                         {
                             Nombre = "pFebruary",
                             Valor = tarjetaConsumo.Febrero,
                         },
                         new Parametro()
                         {
                             Nombre = "pMarch",
                             Valor = tarjetaConsumo.Marzo,
                         },
                         new Parametro()
                         {
                             Nombre = "pApril",
                             Valor = tarjetaConsumo.Abril,
                         },
                         new Parametro()
                         {
                             Nombre = "pMay",
                             Valor = tarjetaConsumo.Mayo,
                         },
                         new Parametro()
                         {
                             Nombre = "pJune",
                             Valor = tarjetaConsumo.Junio,
                         },
                         new Parametro()
                         {
                             Nombre = "pJuly",
                             Valor = tarjetaConsumo.Julio,
                         },
                         new Parametro()
                         {
                             Nombre = "pAugust",
                             Valor = tarjetaConsumo.Agosto,
                         },
                         new Parametro()
                         {
                             Nombre = "pSeptember",
                             Valor = tarjetaConsumo.Septiembre,
                         },
                         new Parametro()
                         {
                             Nombre = "pOctober",
                             Valor = tarjetaConsumo.Octubre,
                         },
                         new Parametro()
                         {
                             Nombre = "pNovember",
                             Valor = tarjetaConsumo.Noviembre,
                         },
                         new Parametro()
                         {
                             Nombre = "pDecember",
                             Valor = tarjetaConsumo.Diciembre,
                         },
                         new Parametro()
                         {
                             Nombre = "pYear",
                             Valor = 2025,
                         },
                         new Parametro()
                         {
                             Nombre = "pVerified",
                             Valor = tarjetaConsumo.Verificado ? 1 : 0,
                         },
                         new Parametro()
                         {
                             Nombre = "pPaid",
                             Valor = tarjetaConsumo.Pagado ? 1 : 0,
                         },
                     ],
                };
                
                if (action == "actualizar")
                {
                    generalRequest.Parametros.Add(
                        new Parametro()
                        {
                            Nombre = "pId",
                            Valor = tarjetaConsumo.Id,
                        });

                    generalDataResponse = await this.serviceCaller.ActualizarRegistro<GeneralDataResponse>(ServicioEnum.ConsumosTarjeta, generalRequest);
                }
                else
                {
                    generalDataResponse = await this.serviceCaller.GenerarRegistro<GeneralDataResponse>(ServicioEnum.ConsumosTarjeta, generalRequest);
                }
                
                HttpContext.Session.Remove(cacheNameDataTarjetaConsumo);

            }


            cacheTarjetaConsumo = HttpContext.Session.GetString(cacheNameDataTarjetaConsumo);
            if (cacheTarjetaConsumo == null)
            {
                response = await this.serviceCaller.ObtenerRegistros<TarjetaConsumoResponse>(ServicioEnum.ConsumosTarjeta, keyValuePairs);
            }
            else
            {
                response = JsonConvert.DeserializeObject<TarjetaConsumoResponse>(cacheTarjetaConsumo);
            }

            ViewBag.TarjetaConsumos = response?.TarjetaConsumos;
            
            //return View("Index");
            return await Task.FromResult<IActionResult>(View("Index", ViewBag));

        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.ToString());

            return View(new List<TarjetaConsumo>());
        }
    }

    [HttpPost]
    public async Task<IActionResult> ConsumosTarjetasFormAdd([FromForm] TarjetaConsumo tarjetaConsumo, string action)
    {
        // Procesa los datos del formulario que están en el objeto 'model'
        // Por ejemplo, guarda en una base de datos  
        ViewBag.Modulo = Modulo;
        ViewBag.Action = action;
        ViewBag.TarjetaConsumo = tarjetaConsumo;
        ViewBag.Title = $"Formulario de {Modulo}";

        // Obtener Transacciones
        cacheTransacciones = HttpContext.Session.GetString(cacheNameDataTransacciones);

        if (cacheTransacciones == null)
        {
            transaccionesResponse = await this.serviceCaller.ObtenerRegistros<TransaccionesResponse>(ServicioEnum.Transacciones);

            HttpContext.Session.SetString(cacheNameDataTransacciones, JsonConvert.SerializeObject(response));
        }
        else
        {
            transaccionesResponse = JsonConvert.DeserializeObject<TransaccionesResponse>(cacheTransacciones);
        }

        ViewBag.Transacciones = transaccionesResponse?.Transacciones;

        // Obtener Consumos Tarjetas
        cacheTarjetaConsumo = HttpContext.Session.GetString(cacheNameDataTarjetaConsumo);
        if (cacheTarjetaConsumo == null)
        {
            response = await this.serviceCaller.ObtenerRegistros<TarjetaConsumoResponse>(ServicioEnum.ConsumosTarjeta, keyValuePairs);
        }
        else
        {
            response = JsonConvert.DeserializeObject<TarjetaConsumoResponse>(cacheTarjetaConsumo);
        }

        ViewBag.TarjetaConsumos = response?.TarjetaConsumos;

        return await Task.FromResult<IActionResult>(View(ViewBag)); // Redirige a otra página
    }

    [HttpPost]
    public async Task<IActionResult> ConsumosTarjetasFormEdit([FromForm] TarjetaConsumo tarjetaConsumo, string action)
    {
        // Procesa los datos del formulario que están en el objeto 'model'
        // Por ejemplo, guarda en una base de datos  
        ViewBag.Modulo = Modulo;
        ViewBag.Action = action;
        ViewBag.Title = $"Formulario de {Modulo}";

        switch (action)
        {
            case "add":
                return await Task.FromResult<IActionResult>(View()); // Redirige a otra página

            case "openFormView":
            case "openFormEdit":

                // Obtener Consumos Tarjeta
                cacheTarjetaConsumo = HttpContext.Session.GetString(cacheNameDataTarjetaConsumo);
                if (cacheTarjetaConsumo == null)
                {
                    response = await this.serviceCaller.ObtenerRegistros<TarjetaConsumoResponse>(ServicioEnum.ConsumosTarjeta, keyValuePairs);
                }
                else
                {
                    response = JsonConvert.DeserializeObject<TarjetaConsumoResponse>(cacheTarjetaConsumo);
                }

                ViewBag.TarjetaConsumo = response?.TarjetaConsumos.Find(t => t.Id == tarjetaConsumo.Id);

                // Obtener Transacciones
                cacheTransacciones = HttpContext.Session.GetString(cacheNameDataTransacciones);

                if (cacheTransacciones == null)
                {
                    transaccionesResponse = await this.serviceCaller.ObtenerRegistros<TransaccionesResponse>(ServicioEnum.Transacciones);

                    HttpContext.Session.SetString(cacheNameDataTransacciones, JsonConvert.SerializeObject(response));
                }
                else
                {
                    transaccionesResponse = JsonConvert.DeserializeObject<TransaccionesResponse>(cacheTransacciones);
                }

                ViewBag.Transacciones = transaccionesResponse?.Transacciones;

                // Obtener Tarjetas
                cacheTarjetas = HttpContext.Session.GetString(cacheNameDataTarjetas);

                if (cacheTarjetas == null)
                {
                    tarjetasResponse = await this.serviceCaller.ObtenerRegistros<TarjetasResponse>(ServicioEnum.Tarjetas);

                    HttpContext.Session.SetString(cacheNameDataTransacciones, JsonConvert.SerializeObject(response));
                }
                else
                {
                    tarjetasResponse = JsonConvert.DeserializeObject<TarjetasResponse>(cacheTarjetas);
                }

                ViewBag.Tarjetas = tarjetasResponse?.Tarjetas;

                ViewBag.ModeView = action == "openFormView" ? true : false;

                return await Task.FromResult<IActionResult>(View()); // Redirige a otra página

            default:

                return await Task.FromResult<IActionResult>(View("Index", tarjetaConsumo));
        }

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}