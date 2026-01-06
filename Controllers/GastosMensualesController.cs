namespace PersonalFinance.Controllers;

#pragma warning disable CS8601 // Posible asignación de referencia nula

using Microsoft.AspNetCore.Mvc;
using PersonalFinance.Helper;
using PersonalFinance.Models;
using PersonalFinance.Models.Entidades;
using PersonalFinance.Models.Enums;
using PersonalFinance.Models.Gastos;
using PersonalFinance.Service;
using System.Diagnostics;
using System.Net.Http;

public class GastosMensualesController : BaseController
{
    private readonly string Gestion = "Administrar";
    private readonly string Modulo = "GastosMensuales";
    private readonly ILogger<GastosMensualesController> _logger;

    public GastosMensualesController(ILogger<GastosMensualesController> logger)
    {
        _logger = logger;
        this.httpClientHandler = new()
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
        };
    }

    public async Task<IActionResult> Index([FromForm] Gasto gasto, string action)
    {
        this.Inicialized();

        _logger.LogInformation("Inicializando TarjetasController => Index()");
        ViewBag.Modulo = Modulo;
        ViewBag.Title = $"{Gestion}";
        ViewBag.Message = $"Gestión de {Modulo}";
        @ViewBag.Year = Utils.GetYear(httpContext);
        try
        {
            gastosResponse = await this.serviceCaller.ObtenerRegistros<GastosResponse>(ServicioEnum.GastosMensuales, keyValuePairs);

            ViewBag.Gastos = gastosResponse.Gastos;

            return View(ViewBag);

        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.ToString());

            return View(new List<Gasto>());
        }
    }

    [HttpPost]
    public async Task<IActionResult> GastosMensuales([FromForm] Gasto gasto, string action, int VilleteraSel, int TipoGastoSel)
    {
        this.Inicialized();
        ViewBag.Modulo = Modulo;
        ViewBag.Title = "Gastos";
        ViewBag.Message = $"{Gestion} {Modulo}";

        try
        {
            if (action == "generar" || action == "actualizar")
            { 
                gasto = Utils.MapRequest<Gasto>(this.Request.Form, ServicioEnum.GastosMensuales);

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
                             Valor = DateTime.Now.Year,
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

                    generalDataResponse = await this.serviceCaller.ActualizarRegistro<GeneralDataResponse>(ServicioEnum.GastosMensuales, generalRequest);

                }
                else
                {
                    generalDataResponse = await this.serviceCaller.GenerarRegistro<GeneralDataResponse>(ServicioEnum.GastosMensuales, generalRequest);
                }

                CacheAdmin.Remove(HttpContext, ServicioEnum.GastosMensuales);
            }

            gastosResponse = await this.serviceCaller.ObtenerRegistros<GastosResponse>(ServicioEnum.GastosMensuales, keyValuePairs);

            ViewBag.Gastos = gastosResponse?.Gastos;
            
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
    public async Task<IActionResult> GastosMensualesFormAdd([FromForm] Gasto gasto, string action)
    {
        this.Inicialized();
        // Procesa los datos del formulario que están en el objeto 'model'
        // Por ejemplo, guarda en una base de datos  
        ViewBag.Modulo = Modulo;
        ViewBag.Action = action;
        ViewBag.Gasto = gasto;
        ViewBag.Villeteras = new List<Entidad>();

        // Obtener Villeteras o Entidades
        entidadesResponse = await this.serviceCaller.ObtenerRegistros<EntidadesResponse>(ServicioEnum.Entidades, keyValuePairs);
        ViewBag.Villeteras = entidadesResponse?.Entidades;

        // Obtener Tipo de Gastos
        tiposGastosResponse = await this.serviceCaller.ObtenerRegistros<TiposGastosResponse>(ServicioEnum.TipoGastos, keyValuePairs);

        ViewBag.TiposGastos = tiposGastosResponse?.TiposGastos;

        return await Task.FromResult<IActionResult>(View(ViewBag)); // Redirige a otra página
    }

    [HttpPost]
    public async Task<IActionResult> GastosMensualesFormEdit([FromForm] Gasto gasto, string action)
    {
        this.Inicialized();
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
                gastosResponse = await this.serviceCaller.ObtenerRegistros<GastosResponse>(ServicioEnum.GastosMensuales, keyValuePairs);

                // Obtener Villeteras o Entidades
                entidadesResponse = await this.serviceCaller.ObtenerRegistros<EntidadesResponse>(ServicioEnum.Entidades, keyValuePairs);
                ViewBag.Villeteras = entidadesResponse?.Entidades;

                // Obtener Tipo de Gastos
                tiposGastosResponse = await this.serviceCaller.ObtenerRegistros<TiposGastosResponse>(ServicioEnum.TipoGastos, keyValuePairs);
                ViewBag.TiposGastos = tiposGastosResponse?.TiposGastos;

                ViewBag.ModeView = action == "openFormView" ? true : false;
                ViewBag.Gasto = gastosResponse.Gastos.Find(t => t.Id == gasto.Id);

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