namespace PersonalFinance.Controllers;

#pragma warning disable CS8601 // Posible asignación de referencia nula

using Microsoft.AspNetCore.Mvc;
using PersonalFinance.Helper;
using PersonalFinance.Models;
using PersonalFinance.Models.Enums;
using PersonalFinance.Models.IngresosMensuales;
using PersonalFinance.Models.IngresosTipo;
using System.Diagnostics;
using System.Net.Http;

public class IngresosMensualesController : BaseController
{
    private readonly string Gestion = "Administrar";
    private readonly string Modulo = "IngresosMensuales";
    private readonly ILogger<IngresosMensualesController> _logger;

    public IngresosMensualesController(ILogger<IngresosMensualesController> logger)
    {
        _logger = logger;
        this.httpClientHandler = new()
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
        };
    }

    public async Task<IActionResult> Index([FromForm] Ingreso ingreso, string action)
    {
        this.Inicialized();

        _logger.LogInformation("Inicializando IngresosMensuales => Index()");
        ViewBag.Modulo = Modulo;
        ViewBag.Title = $"{Gestion}";
        ViewBag.Message = $"Gestión de {Modulo}";
        ViewBag.Year = Utils.GetYear(httpContext);
        try
        {
            ingresosResponse = await this.serviceCaller.ObtenerRegistros<IngresosResponse>(ServicioEnum.Ingresos, keyValuePairs);
            
            ViewBag.Ingresos = ingresosResponse.Ingresos;

            return await Task.FromResult<IActionResult>(View(ViewBag));
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.ToString());

            return View(new List<Ingreso>());
        }
    }

    [HttpPost]
    public async Task<IActionResult> IngresosMensuales([FromForm] Ingreso ingreso, string action)
    {
        this.Inicialized();
        ViewBag.Modulo = Modulo;
        ViewBag.Title = $"{Gestion}";
        ViewBag.Message = $"Gestión de {Modulo}";

        try
        {
            if (action == "generar" || action == "actualizar")
            {
                ingreso = Utils.MapRequest<Ingreso>(this.Request.Form, ServicioEnum.Ingresos);
                
                this.generalRequest = new()
                {
                    Parametros =
                        [
                         new Parametro()
                         {
                             Nombre = "pJanuary",
                             Valor = ingreso.Enero,
                         },
                         new Parametro()
                         {
                             Nombre = "pFebruary",
                             Valor = ingreso.Febrero,
                         },
                         new Parametro()
                         {
                             Nombre = "pMarch",
                             Valor = ingreso.Marzo,
                         },
                         new Parametro()
                         {
                             Nombre = "pApril",
                             Valor = ingreso.Abril,
                         },
                         new Parametro()
                         {
                             Nombre = "pMay",
                             Valor = ingreso.Mayo,
                         },
                         new Parametro()
                         {
                             Nombre = "pJune",
                             Valor = ingreso.Junio,
                         },
                         new Parametro()
                         {
                             Nombre = "pJuly",
                             Valor = ingreso.Julio,
                         },
                         new Parametro()
                         {
                             Nombre = "pAugust",
                             Valor = ingreso.Agosto,
                         },
                         new Parametro()
                         {
                             Nombre = "pSeptember",
                             Valor = ingreso.Septiembre,
                         },
                         new Parametro()
                         {
                             Nombre = "pOctober",
                             Valor = ingreso.Octubre,
                         },
                         new Parametro()
                         {
                             Nombre = "pNovember",
                             Valor = ingreso.Noviembre,
                         },
                         new Parametro()
                         {
                             Nombre = "pDecember",
                             Valor = ingreso.Diciembre,
                         },
                         new Parametro()
                         {
                             Nombre = "pIncomeDetailsId",
                             Valor = ingreso.TipoIngreso.Id,
                         },
                         new Parametro()
                         {
                             Nombre = "pYear",
                             Valor = ingreso.Ano,
                         },
                     ],
                };
                
                if (action == "actualizar")
                {
                    generalRequest.Parametros.Add(
                        new Parametro()
                        {
                            Nombre = "pId",
                            Valor = ingreso.Id,
                        });

                    generalDataResponse = await this.serviceCaller.ActualizarRegistro<GeneralDataResponse>(ServicioEnum.Ingresos, generalRequest);

                }
                else
                {
                    generalDataResponse = await this.serviceCaller.GenerarRegistro<GeneralDataResponse>(ServicioEnum.Ingresos, this.generalRequest);
                }

                CacheAdmin.Remove(HttpContext, ServicioEnum.Ingresos);


                // actualiza balance de ingresos mensuales
                this.generalRequest = new()
                {
                    Parametros =
                        [
                         new Parametro()
                         {
                             Nombre = "pYear",
                             Valor = ingreso.Ano,
                         },
                     ],
                };

                await this.serviceCaller.EjecutarProceso<GeneralDataResponse>(ServicioEnum.Ingresos, generalRequest, MetodoEnum.ExecuteUpdateMonthlyIncomes);

            }
            else if(action == "copyIncome")
            {
                generalDataResponse = await this.EjecutarProceso(ServicioEnum.IngresosCopiaMensual, MetodoEnum.CopiarIngresoMensual);

                CacheAdmin.Remove(HttpContext, ServicioEnum.Ingresos);
            }

            ingresosResponse = await this.serviceCaller.ObtenerRegistros<IngresosResponse>(ServicioEnum.Ingresos, keyValuePairs);

            ViewBag.Ingresos = ingresosResponse.Ingresos;

            //return View("Index");
            return await Task.FromResult<IActionResult>(View("Index", ViewBag));

        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.ToString());

            return await Task.FromResult<IActionResult>(View(new List<Ingreso>()));
        }
    }

    [HttpPost]
    public async Task<IActionResult> IngresosMensualesFormAdd([FromForm] Ingreso gasto, string action)
    {
        this.Inicialized();
        // Procesa los datos del formulario que están en el objeto 'model'
        // Por ejemplo, guarda en una base de datos  
        ViewBag.Modulo = Modulo;
        ViewBag.Title = $"{Gestion}";
        ViewBag.Message = $"Gestión de {Modulo}";
        ViewBag.Action = action;
        ViewBag.Year = Utils.GetYear(HttpContext);

        // Obtener Villeteras o Entidades
        ingresosTipoResponse = await this.serviceCaller.ObtenerRegistros<IngresosTipoResponse>(ServicioEnum.IngresoTipo);
                

        ViewBag.IngresosTipos = ingresosTipoResponse?.IngresosTipos;

        return await Task.FromResult<IActionResult>(View(ViewBag)); // Redirige a otra página
    }

    [HttpPost]
    public async Task<IActionResult> IngresosMensualesFormEdit([FromForm] Ingreso ingreso, string action)
    {
        this.Inicialized();
        // Procesa los datos del formulario que están en el objeto 'model'
        // Por ejemplo, guarda en una base de datos  
        ViewBag.Modulo = Modulo;
        ViewBag.Title = $"{Gestion}";
        ViewBag.Message = $"Gestión de {Modulo}";
        ViewBag.Action = action;

        switch (action)
        {
            case "add":
                return await Task.FromResult<IActionResult>(View()); // Redirige a otra página

            case "openFormView":
            case "openFormEdit":

                //IngresosTipos
                ingresosTipoResponse = await this.serviceCaller.ObtenerRegistros<IngresosTipoResponse>(ServicioEnum.IngresoTipo);

                var year = Utils.GetYear(HttpContext);


                // Obtener Ingresos
                ingresosResponse = await this.serviceCaller.ObtenerRegistros<IngresosResponse>(ServicioEnum.Ingresos, keyValuePairs);               
  

                ViewBag.ModeView = action == "openFormView" ? true : false;
                ViewBag.Ingreso = ingresosResponse?.Ingresos?.Find(t => t.Id == ingreso.Id);
                ViewBag.IngresosTipos = ingresosTipoResponse.IngresosTipos;

                return await Task.FromResult<IActionResult>(View(ViewBag)); // Redirige a otra página

            default:

                return await Task.FromResult<IActionResult>(View("Index", ingreso));
        }

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }    
}