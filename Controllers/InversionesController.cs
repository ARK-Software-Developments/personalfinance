namespace PersonalFinance.Controllers;

#pragma warning disable CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PersonalFinance.Helper;
using PersonalFinance.Models;
using PersonalFinance.Models.Categorias;
using PersonalFinance.Models.Entidades;
using PersonalFinance.Models.Enums;
using PersonalFinance.Models.Gastos;
using PersonalFinance.Models.IngresosTipo;
using PersonalFinance.Models.Pagos;
using PersonalFinance.Models.Prestamos;
using PersonalFinanceApiNetCoreModel;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;

public class InversionesController : BaseController
{
    private readonly string Gestion = "Administrar";
    private readonly string Modulo = "Inversiones";
    private readonly ILogger<InversionesController> _logger;

    public InversionesController(ILogger<InversionesController> logger)
    {
        _logger = logger;
        this.httpClientHandler = new()
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
        };

        this.prestamoResponse = new();
    }

    [HttpPost]
    [HttpGet]
    public async Task<IActionResult> Index([FromForm] Inversion inverion, string action)
    {
        this.Inicialized();

        ViewBag.Modulo = Modulo;
        ViewBag.Title = $"{Gestion}";
        ViewBag.Message = $"Gestión de {Modulo}";
        ViewBag.Ruta = $"{ViewBag.Modulo} > {ViewBag.Title}";
        ViewBag.Buscar = "Buscar";

        List<Inversion> inversiones = [];
        this.inversionesResponse = await this.serviceCaller.ObtenerRegistros<InversionesResponse>(ServicioEnum.Inversiones);

        inversiones = this.inversionesResponse?.Inversiones?
                    .OrderByDescending(o => o.FechaOperacion)
                    .ToList();
        ViewBag.Inversiones = inversiones;

        try
        {

            if (this.Request.Query.ContainsKey("search") && !string.IsNullOrEmpty(this.Request.Query["search"]))
            {
                string search = this.Request.Query["search"].ToString().ToUpper();

                inversiones = this.inversionesResponse?.Inversiones?.FindAll(x => x.Tipo != null && !string.IsNullOrEmpty(x.Tipo.Nombre) && x.Tipo.Nombre.Contains(search));

                if (inversiones?.Count == 0)
                {
                    inversiones = this.inversionesResponse?.Inversiones?.FindAll(x => x.Tipo != null && !string.IsNullOrEmpty(x.Tipo.Tipo) && x.Tipo.Tipo.Contains(search));
                }

                if (inversiones?.Count == 0)
                {
                    inversiones = this.inversionesResponse?.Inversiones?.FindAll(x => !string.IsNullOrEmpty(x.Estado) && x.Estado.Contains(search));
                }

                if (inversiones?.Count == 0)
                {
                    inversiones = this.inversionesResponse?.Inversiones?.FindAll(x => x.Entidad != null && !string.IsNullOrEmpty(x.Entidad.Tipo) && x.Entidad.Tipo.Contains(search));
                }

                if (inversiones?.Count == 0)
                {
                    inversiones = this.inversionesResponse?.Inversiones?.FindAll(x => x.Entidad != null && !string.IsNullOrEmpty(x.Entidad.Nombre) && x.Entidad.Nombre.Contains(search));
                }

                if (inversiones?.Count == 0)
                {
                    inversiones = this.inversionesResponse?.Inversiones?
                                .OrderByDescending(o => o.FechaOperacion)
                                .ToList();
                }

                ViewBag.Buscar = "Limpiar";
            }

            ViewBag.Inversiones = inversiones;

            return await Task.FromResult<IActionResult>(View(ViewBag));
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.ToString());
            return await Task.FromResult<IActionResult>(View(ViewBag));
        }
    }

    public async Task<IActionResult> Inversiones([FromForm] Inversion inversion, string action)
    {
        this.Inicialized();

        _logger.LogInformation("Inicializando PrestamosController => Index()");
        ViewBag.Modulo = Modulo;
        ViewBag.Title = Modulo;
        ViewBag.Message = $"{Gestion} {Modulo}";
        List<Inversion> inversiones = [];
        ViewBag.Buscar = "Buscar";
        ViewBag.ModeView = action == "openFormView" ? true : false;
        this.inversionesResponse = await this.serviceCaller.ObtenerRegistros<InversionesResponse>(ServicioEnum.Inversiones);
        inversiones = this.inversionesResponse?.Inversiones?
                        .OrderByDescending(o => o.FechaOperacion)
                        .ToList();

        ViewBag.Inversiones = inversiones;

        var lstEstados = new List<SelectListItem>();
        lstEstados.Add(
            new SelectListItem()
            {
                Value = "1",
                Text = "ACTIVO",
                Selected = false,
            });

        lstEstados.Add(
            new SelectListItem()
            {
                Value = "2",
                Text = "COMPLETADO",
                Selected = false,
            });

        ViewBag.Estados = lstEstados;

        try
        {
            switch (action)
            {
                case "openFormAdd":
                case "openFormEdit":
                case "openFormView":

                    this.entidadesResponse = await this.serviceCaller.ObtenerRegistros<EntidadesResponse>(ServicioEnum.Entidades);
                    this.inversionTipoResponse = await this.serviceCaller.ObtenerRegistros<InversionTipoResponse>(ServicioEnum.InversionesTipos);

                    ViewBag.Entidades = entidadesResponse.Entidades;
                    ViewBag.InversionesTipos = inversionTipoResponse.InversionTipos;


                    if (action == "openFormAdd")
                    {
                        return await Task.FromResult<IActionResult>(View("InversionFormAdd", ViewBag));
                    }
                    else
                    {
                        ViewBag.Inversion = inversiones?.Find(i => i.Id == int.Parse(this.Request.Form["Id"]));

                        return await Task.FromResult<IActionResult>(View("InversionFormEdit", ViewBag));
                    } 

                case "actualizar":
                case "generar":

                    var mapInversion = Utils.MapRequest<Inversion>(this.Request.Form, ServicioEnum.Inversiones);

                    this.generalRequest = new GeneralRequest()
                    {
                        Parametros =
                        [
                           new Parametro()
                           {
                               Nombre = "inEntityId",
                               Valor = mapInversion.Entidad.Id,
                           },
                           new Parametro()
                           {
                               Nombre = "inInvestmentDate",
                               Valor = mapInversion.FechaOperacion?.ToString("yyyy-MM-dd"),
                           },
                           new Parametro()
                           {
                               Nombre = "inInvestmentAmount",
                               Valor = mapInversion.MontoInvertido,
                           },
                           new Parametro()
                           {
                               Nombre = "inInvestmentProfit",
                               Valor = mapInversion.MontoGanado,
                           },
                           new Parametro()
                           {
                               Nombre = "inUpdateDate",
                               Valor = mapInversion.FechaActualizacion?.ToString("yyyy-MM-dd"),
                           },
                           new Parametro()
                           {
                               Nombre = "inInvestmentTypeId",
                               Valor = mapInversion.Tipo.Id,
                           },
                           new Parametro()
                           {
                               Nombre = "inState",
                               Valor = mapInversion.Estado,
                           }
                        ]
                    };

                    this.generalDataResponse = new GeneralDataResponse();

                    if (action == "actualizar")
                    {
                        var parametro = new Parametro()
                        {
                            Nombre = "intId",
                            Valor = mapInversion.Id,
                        };
                        this.generalRequest.Parametros.Add(parametro);

                        this.generalDataResponse = await this.serviceCaller.ActualizarRegistro<GeneralDataResponse>(ServicioEnum.Inversiones, this.generalRequest);

                    }
                    else
                    {
                        this.generalDataResponse = await this.serviceCaller.GenerarRegistro<GeneralDataResponse>(ServicioEnum.Inversiones, this.generalRequest);
                    }

                    CacheAdmin.Remove(HttpContext, ServicioEnum.Inversiones);
                    this.inversionesResponse = await this.serviceCaller.ObtenerRegistros<InversionesResponse>(ServicioEnum.Inversiones);
                    inversiones = this.inversionesResponse?.Inversiones?
                        .OrderByDescending(o => o.FechaOperacion)
                        .ToList();
                    ViewBag.Inversiones = inversiones;

                    return await Task.FromResult<IActionResult>(View("Index", ViewBag));

                default:

                    ViewBag.Inversiones = inversiones;
                    return await Task.FromResult<IActionResult>(View("Index", ViewBag));
            }



        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.ToString());

            return await Task.FromResult<IActionResult>(View("Index", ViewBag));
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
