namespace PersonalFinance.Controllers;

#pragma warning disable CS8601 // Posible asignación de referencia nula

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PersonalFinance.Helper;
using PersonalFinance.Models;
using PersonalFinance.Models.Entidades;
using PersonalFinance.Models.Enums;
using PersonalFinance.Models.Gastos;
using PersonalFinance.Models.Tarjetas;
using PersonalFinance.Models.Transacciones;
using PersonalFinance.Service;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

public class GastosController : Controller
{
    private readonly string Gestion = "Administrar";
    private readonly string Modulo = "Gastos";
    private readonly string cacheNameDataGastos = "cacheDataGastos";
    private readonly string cacheNameDataEntidades = "cacheDataEntidades";
    private readonly string cacheNameDataTiposGastos = "cacheDataTiposGastos";
    private readonly ILogger<GastosController> _logger;
    private readonly ServiceCaller serviceCaller;
    private readonly Dictionary<string, object> keyValuePairs = [];

    private GeneralDataResponse generalDataResponse = new();
    private GastosResponse response = new();
    private EntidadesResponse entidadesResponse = new();
    private TiposGastosResponse tiposGastosResponse = new();
    private GeneralRequest generalRequest = new();
    private string cacheGastos = string.Empty;
    private string cacheTiposGastos = string.Empty;
    private string cacheEntidades = string.Empty;
    

    

    public GastosController(ILogger<GastosController> logger)
    {
        _logger = logger;
        HttpClientHandler httpClientHandler = new()
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
        };
        this.serviceCaller = new ServiceCaller(new HttpClient(httpClientHandler));
        this.keyValuePairs.Add("year", 2025);
    }


    public async Task<IActionResult> Index([FromForm] Gasto gasto, string action)
    {
        _logger.LogInformation("Inicializando TarjetasController => Index()");
        ViewBag.Modulo = Modulo;
        ViewBag.Title = $"{Gestion}";
        ViewBag.Message = $"Gestión de {Modulo}";

        try
        {
            var cacheGastos = HttpContext.Session.GetString(cacheNameDataGastos);
            if (cacheGastos == null)
            {
                

                response = await this.serviceCaller.ObtenerRegistros<GastosResponse>(ServicioEnum.Gastos, keyValuePairs);
            }
            else
            {
                response = JsonConvert.DeserializeObject<GastosResponse>(cacheGastos);
            }

            ViewBag.Gastos = response.Gastos;

            return View(ViewBag);

        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.ToString());

            return View(new List<Gasto>());
        }
    }

    [HttpPost]
    public async Task<IActionResult> Gastos([FromForm] Gasto gasto, string action, int VilleteraSel, int TipoGastoSel)
    {
        ViewBag.Modulo = Modulo;
        ViewBag.Title = "Gastos";
        ViewBag.Message = $"{Gestion} {Modulo}";

        try
        {
            if (action == "generar" || action == "actualizar")
            {
                cacheGastos = HttpContext.Session.GetString(cacheNameDataGastos);

                if (cacheGastos == null)
                {
                    response = await this.serviceCaller.ObtenerRegistros<GastosResponse>(ServicioEnum.Gastos, this.keyValuePairs);

                    HttpContext.Session.SetString(cacheNameDataGastos, JsonConvert.SerializeObject(response));
                }
                else
                {
                    response = JsonConvert.DeserializeObject<GastosResponse>(cacheGastos);
                }

                gasto = Utils.MapRequest<Gasto>(this.Request.Form, ServicioEnum.Gastos);

                generalRequest = new()
                {
                    Parametros =
                        [
                         new Parametro()
                         {
                             Nombre = "pTypeofexpenseid",
                             Valor = gasto.TipoGasto.Id,
                         },
                         new Parametro()
                         {
                             Nombre = "pSummary",
                             Valor = gasto.Resumen,
                         },
                         new Parametro()
                         {
                             Nombre = "pJanuary",
                             Valor = gasto.Enero,
                         },
                         new Parametro()
                         {
                             Nombre = "pFebruary",
                             Valor = gasto.Febrero,
                         },
                         new Parametro()
                         {
                             Nombre = "pMarch",
                             Valor = gasto.Marzo,
                         },
                         new Parametro()
                         {
                             Nombre = "pApril",
                             Valor = gasto.Abril,
                         },
                         new Parametro()
                         {
                             Nombre = "pMay",
                             Valor = gasto.Mayo,
                         },
                         new Parametro()
                         {
                             Nombre = "pJune",
                             Valor = gasto.Junio,
                         },
                         new Parametro()
                         {
                             Nombre = "pJuly",
                             Valor = gasto.Julio,
                         },
                         new Parametro()
                         {
                             Nombre = "pAugust",
                             Valor = gasto.Agosto,
                         },
                         new Parametro()
                         {
                             Nombre = "pSeptember",
                             Valor = gasto.Septiembre,
                         },
                         new Parametro()
                         {
                             Nombre = "pOctober",
                             Valor = gasto.Octubre,
                         },
                         new Parametro()
                         {
                             Nombre = "pNovember",
                             Valor = gasto.Noviembre,
                         },
                         new Parametro()
                         {
                             Nombre = "pDecember",
                             Valor = gasto.Diciembre,
                         },
                         new Parametro()
                         {
                             Nombre = "pWallet",
                             Valor = gasto.Villetera.Id,
                         },
                         new Parametro()
                         {
                             Nombre = "pVerified",
                             Valor = gasto.Verificado ? 1 : 0,
                         },
                         new Parametro()
                         {
                             Nombre = "pReserved",
                             Valor = gasto.Reservado ? 1 : 0,
                         },
                         new Parametro()
                         {
                             Nombre = "pPaid",
                             Valor = gasto.Pagado ? 1 : 0,
                         },
                         new Parametro()
                         {
                             Nombre = "pYear",
                             Valor = 2025,
                         },
                         new Parametro()
                         {
                             Nombre = "pObservations",
                             Valor = gasto.Observaciones,
                         },
                         new Parametro()
                         {
                             Nombre = "pActive",
                             Valor = 1,
                         },
                     ],
                };
                
                if (action == "actualizar")
                {
                    generalRequest.Parametros.Add(
                        new Parametro()
                        {
                            Nombre = "pId",
                            Valor = gasto.Id,
                        });

                    generalDataResponse = await this.serviceCaller.ActualizarRegistro<GeneralDataResponse>(ServicioEnum.Gastos, generalRequest);

                }
                else
                {
                    generalDataResponse = await this.serviceCaller.GenerarRegistro<GeneralDataResponse>(ServicioEnum.Gastos, generalRequest);
                }
                
                HttpContext.Session.Remove(cacheNameDataGastos);

            }

            
            cacheGastos = HttpContext.Session.GetString(cacheNameDataGastos);

            if(cacheGastos == null)
            {
                response = await this.serviceCaller.ObtenerRegistros<GastosResponse>(ServicioEnum.Gastos, this.keyValuePairs);

                HttpContext.Session.SetString(cacheNameDataGastos, JsonConvert.SerializeObject(response));
            }
            else
            {
                response = JsonConvert.DeserializeObject<GastosResponse>(cacheGastos);
            }

            ViewBag.Gastos = response?.Gastos;
            
            //return View("Index");
            return await Task.FromResult<IActionResult>(View("Index", ViewBag));

        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.ToString());

            return View(new List<Gasto>());
        }
    }

    [HttpPost]
    public async Task<IActionResult> GastoFormAdd([FromForm] Gasto gasto, string action)
    {
        // Procesa los datos del formulario que están en el objeto 'model'
        // Por ejemplo, guarda en una base de datos  
        ViewBag.Modulo = Modulo;
        ViewBag.Action = action;
        ViewBag.Gasto = gasto;
        ViewBag.Villeteras = new List<Entidad>();


        // Obtener Villeteras o Entidades
        cacheEntidades = HttpContext.Session.GetString(cacheNameDataEntidades);

        if (cacheEntidades == null)
        {
            entidadesResponse = await this.serviceCaller.ObtenerRegistros<EntidadesResponse>(ServicioEnum.Entidades);
        }
        else
        {
            entidadesResponse = JsonConvert.DeserializeObject<EntidadesResponse>(cacheEntidades);
        }

        ViewBag.Villeteras = entidadesResponse?.Entidades;

        // Obtener Tipo de Gastos
        cacheTiposGastos = HttpContext.Session.GetString(cacheNameDataTiposGastos);

        if (cacheTiposGastos == null)
        {
            tiposGastosResponse = await this.serviceCaller.ObtenerRegistros<TiposGastosResponse>(ServicioEnum.TipoGastos);
        }
        else
        {
            tiposGastosResponse = JsonConvert.DeserializeObject<TiposGastosResponse>(cacheTiposGastos);
        }

        ViewBag.TiposGastos = tiposGastosResponse?.TiposGastos;

        return await Task.FromResult<IActionResult>(View(ViewBag)); // Redirige a otra página
    }

    [HttpPost]
    public async Task<IActionResult> GastoFormEdit([FromForm] Gasto gasto, string action)
    {
        // Procesa los datos del formulario que están en el objeto 'model'
        // Por ejemplo, guarda en una base de datos  
        ViewBag.Modulo = Modulo;
        ViewBag.Action = action;

        switch (action)
        {
            case "add":
                return await Task.FromResult<IActionResult>(View()); // Redirige a otra página

            case "openFormView":
            case "openFormEdit":

                // Obtener Gasto
                var cacheGastos = HttpContext.Session.GetString(cacheNameDataGastos);
                if (cacheGastos == null)
                {


                    response = await this.serviceCaller.ObtenerRegistros<GastosResponse>(ServicioEnum.Gastos, keyValuePairs);
                }
                else
                {
                    response = JsonConvert.DeserializeObject<GastosResponse>(cacheGastos);
                }

                // Obtener Villeteras o Entidades
                cacheEntidades = HttpContext.Session.GetString(cacheNameDataEntidades);

                if (cacheEntidades == null)
                {
                    entidadesResponse = await this.serviceCaller.ObtenerRegistros<EntidadesResponse>(ServicioEnum.Entidades);
                }
                else
                {
                    entidadesResponse = JsonConvert.DeserializeObject<EntidadesResponse>(cacheEntidades);
                }

                ViewBag.Villeteras = entidadesResponse?.Entidades;

                // Obtener Tipo de Gastos
                cacheTiposGastos = HttpContext.Session.GetString(cacheNameDataTiposGastos);

                if (cacheTiposGastos == null)
                {
                    tiposGastosResponse = await this.serviceCaller.ObtenerRegistros<TiposGastosResponse>(ServicioEnum.TipoGastos);
                }
                else
                {
                    tiposGastosResponse = JsonConvert.DeserializeObject<TiposGastosResponse>(cacheTiposGastos);
                }

                ViewBag.TiposGastos = tiposGastosResponse?.TiposGastos;

                ViewBag.ModeView = action == "openFormView" ? true : false;
                ViewBag.Gasto = response.Gastos.Find(t => t.Id == gasto.Id);

                return await Task.FromResult<IActionResult>(View()); // Redirige a otra página

            default:

                return await Task.FromResult<IActionResult>(View("Index", gasto));
        }

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}