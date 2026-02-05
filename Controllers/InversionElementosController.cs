namespace PersonalFinance.Controllers;

#pragma warning disable CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PersonalFinance.Helper;
using PersonalFinance.Models;
using PersonalFinance.Models.Entidades;
using PersonalFinance.Models.Enums;
using PersonalFinance.Models.InversionesElementos;
using PersonalFinance.Models.InversionesInstrumentos;
using PersonalFinance.Models.InversionTipo;
using PersonalFinanceApiNetCoreModel;
using System.Diagnostics;
using System.Net.Http;

public class InversionElementosController : BaseController
{
    private readonly string Gestion = "Administrar";
    private readonly string Modulo = "InversionElementos";
    private readonly ILogger<InversionElementosController> _logger;

    public InversionElementosController(ILogger<InversionElementosController> logger)
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
    public async Task<IActionResult> Index([FromForm] InversionElemento inverionElemento, string action)
    {
        this.Inicialized();

        ViewBag.Modulo = Modulo;
        ViewBag.Title = $"{Gestion}";
        ViewBag.Message = $"Gestión de {Modulo}";
        ViewBag.Ruta = $"{ViewBag.Modulo} > {ViewBag.Title}";
        ViewBag.Buscar = "Buscar";

        List<InversionElemento> inverionElementos = [];

        this.inversionesElementosResponse = await this.serviceCaller.ObtenerRegistros<InversionesElementosResponse>(ServicioEnum.InversionesElementos);

        inverionElementos = this.inversionesElementosResponse?.InversionElementos?
                    .OrderByDescending(o => o.FechaOperacion)
                    .ToList();
        ViewBag.InverionElementos = inverionElementos;

        try
        {

            if (this.Request.Query.ContainsKey("search") && !string.IsNullOrEmpty(this.Request.Query["search"]))
            {
                string search = this.Request.Query["search"].ToString().ToUpper();

                inverionElementos = this.inversionesElementosResponse?.InversionElementos?.FindAll(x => x.Instrumento != null && !string.IsNullOrEmpty(x.Instrumento.Codigo) && x.Instrumento.Codigo.Contains(search));

                if (inverionElementos?.Count == 0)
                {
                    inverionElementos = this.inversionesElementosResponse?.InversionElementos?.FindAll(x => x.Instrumento != null && !string.IsNullOrEmpty(x.Instrumento.Detalle) && x.Instrumento.Detalle.Contains(search));
                }

                if (inverionElementos?.Count == 0)
                {
                    inverionElementos = this.inversionesElementosResponse?.InversionElementos?.FindAll(x => x.Instrumento != null && x.Instrumento.Tipo != null && !string.IsNullOrEmpty(x.Instrumento.Tipo.Tipo) && x.Instrumento.Tipo.Tipo.Contains(search));
                }

                if (inverionElementos?.Count == 0)
                {
                    inverionElementos = this.inversionesElementosResponse?.InversionElementos?.FindAll(x => x.Instrumento != null && x.Instrumento.Tipo != null && !string.IsNullOrEmpty(x.Instrumento.Tipo.Nombre) && x.Instrumento.Tipo.Nombre.Contains(search));
                }

                if (inverionElementos?.Count == 0)
                {
                    inverionElementos = this.inversionesElementosResponse?.InversionElementos?.FindAll(x => !string.IsNullOrEmpty(x.NumeroOperacion) && x.Estado.Contains(search));
                }

                if (inverionElementos?.Count == 0)
                {
                    inverionElementos = this.inversionesElementosResponse?.InversionElementos?.FindAll(x => !string.IsNullOrEmpty(x.Estado) && x.Estado.Contains(search));
                }

                if (inverionElementos?.Count == 0)
                {
                    inverionElementos = this.inversionesElementosResponse?.InversionElementos?
                                .OrderByDescending(o => o.FechaOperacion)
                                .ToList();
                }

                ViewBag.Buscar = "Limpiar";
            }

            ViewBag.InverionElementos = inverionElementos;

            return await Task.FromResult<IActionResult>(View(ViewBag));
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.ToString());
            return await Task.FromResult<IActionResult>(View(ViewBag));
        }
    }

    public async Task<IActionResult> InversionElementos([FromForm] InversionElemento inversionElemento, string action)
    {
        this.Inicialized();

        _logger.LogInformation("Inicializando PrestamosController => Index()");
        ViewBag.Modulo = Modulo;
        ViewBag.Title = Modulo;
        ViewBag.Message = $"{Gestion} {Modulo}";
        List<Inversion> inversiones = [];
        List<InversionElemento> inversionElementos = [];
        List<InversionInstrumento> inversionesInstrumentos = [];
        ViewBag.Buscar = "Buscar";
        ViewBag.ModeView = action == "openFormView" ? true : false;


        this.inversionesElementosResponse = await this.serviceCaller.ObtenerRegistros<InversionesElementosResponse>(ServicioEnum.InversionesElementos);

        inversionElementos = this.inversionesElementosResponse?.InversionElementos?
                    .OrderByDescending(o => o.FechaOperacion)
                    .ToList();        

        ViewBag.Inversiones = inversiones;
        ViewBag.InversionElementos = inversionElementos;
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
                    this.inversionesInstrumentosResponse = await this.serviceCaller.ObtenerRegistros<InversionesInstrumentosResponse>(ServicioEnum.InversionesInstrumentos);

                    ViewBag.Entidades = this.entidadesResponse.Entidades;
                    ViewBag.InversionesTipos = this.inversionTipoResponse.InversionTipos;
                    ViewBag.InversionesInstrumentos = this.inversionesInstrumentosResponse.InversionInstrumentos;

                    if (action == "openFormAdd")
                    {
                        ViewBag.FromInversion = false;

                        if (this.Request.Form.ContainsKey("fromInversion"))
                        {
                            ViewBag.FromInversion = true;
                            ViewBag.InversionId = int.Parse(this.Request?.Form["InversionId"]);
                            ViewBag.EntidadId = int.Parse(this.Request?.Form["EntidadId"]);
                            ViewBag.InversionTipoId = int.Parse(this.Request?.Form["InversionTipoId"]);
                            ViewBag.InversionesInstrumentos = this.inversionesInstrumentosResponse?.InversionInstrumentos?.FindAll(x => x.Tipo != null && x.Tipo.Id == int.Parse(this.Request.Form["InversionTipoId"])).ToList();
                        }
                        return await Task.FromResult<IActionResult>(View("InversionElementosFormAdd", ViewBag));
                    }
                    else
                    {
                        ViewBag.Inversion = inversiones?.Find(i => i.Id == int.Parse(this.Request.Form["Id"]));

                        var inversionId = new Dictionary<string, object>()
                        {
                            { "year", int.Parse(this.Request.Form["Id"])}
                        };

                        this.inversionesElementosResponse = await this.serviceCaller.ObtenerRegistros<InversionesElementosResponse>(ServicioEnum.InversionesElementos, inversionId, MetodoEnum.Todos);

                        ViewBag.InversionElementos = this.inversionesElementosResponse?.InversionElementos?
                                                        .OrderByDescending(o => o.FechaOperacion)
                                                        .ToList();

                        return await Task.FromResult<IActionResult>(View("InversionElementosnFormEdit", ViewBag));
                    } 

                case "actualizar":
                case "generar":

                    var mapInversionElemento = Utils.MapRequest<InversionElemento>(this.Request.Form, ServicioEnum.InversionesElementos);

                    this.generalRequest = new GeneralRequest()
                    {
                        Parametros =
                        [
                           new Parametro()
                           {
                               Nombre = "intInvestmentId",
                               Valor = mapInversionElemento.Inversion.Id,
                           },
                           new Parametro()
                           {
                               Nombre = "intQuantity",
                               Valor = mapInversionElemento.Cantidad,
                           },
                           new Parametro()
                           {
                               Nombre = "intUnitAmount",
                               Valor = mapInversionElemento.MontoUnitario,
                           },
                           new Parametro()
                           {
                               Nombre = "intTaxAmount",
                               Valor = mapInversionElemento.MontoImpuestos,
                           },
                           new Parametro()
                           {
                               Nombre = "intOperationDate",
                               Valor = mapInversionElemento.FechaOperacion?.ToString("yyyy-MM-dd"),
                           },
                           new Parametro()
                           {
                               Nombre = "intOperationNumber",
                               Valor = mapInversionElemento.NumeroOperacion,
                           },
                           new Parametro()
                           {
                               Nombre = "intInvestmentAmount",
                               Valor = mapInversionElemento.MontoInvertido,
                           },
                           new Parametro()
                           {
                               Nombre = "intResultingAmount",
                               Valor = mapInversionElemento.MontoResultado,
                           },
                           new Parametro()
                           {
                               Nombre = "intState",
                               Valor = mapInversionElemento.Estado,
                           },
                           new Parametro()
                           {
                               Nombre = "intInvestmentInstrumentId",
                               Valor = mapInversionElemento.Instrumento.Id,
                           }
                        ]
                    };

                    this.generalDataResponse = new GeneralDataResponse();

                    if (action == "actualizar")
                    {
                        var parametro = new Parametro()
                        {
                            Nombre = "inId",
                            Valor = mapInversionElemento.Id,
                        };
                        this.generalRequest.Parametros.Add(parametro);

                        this.generalDataResponse = await this.serviceCaller.ActualizarRegistro<GeneralDataResponse>(ServicioEnum.InversionesElementos, this.generalRequest);

                    }
                    else
                    {
                        this.generalDataResponse = await this.serviceCaller.GenerarRegistro<GeneralDataResponse>(ServicioEnum.InversionesElementos, this.generalRequest);
                    }

                    CacheAdmin.Remove(HttpContext, ServicioEnum.InversionesElementos);

                    return RedirectToAction("Index", "Inversiones");

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
