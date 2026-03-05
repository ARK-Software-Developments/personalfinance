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
using PersonalFinance.Models.Pagos;
using PersonalFinance.Models.Pedidos;
using PersonalFinance.Models.Prestamos;
using PersonalFinanceApiNetCoreModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;

public class PrestamosController : BaseController
{
    private readonly string Gestion = "Administrar";
    private readonly string Modulo = "Prestamos";
    private readonly ILogger<PrestamosController> _logger;
    private readonly List<SelectListItem> lstEstados = [];

    public PrestamosController(ILogger<PrestamosController> logger)
    {
        _logger = logger;
        this.httpClientHandler = new()
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
        };

        this.prestamoResponse = new();

        lstEstados.Add(
            new SelectListItem()
            {
                Value = "0",
                Text = "PENDIENTE",
                Selected = false,
            });

        lstEstados.Add(
            new SelectListItem()
            {
                Value = "1",
                Text = "PAGO AL DIA",
                Selected = false,
            });

        lstEstados.Add(
            new SelectListItem()
            {
                Value = "2",
                Text = "PAGOS ATRAZADOS",
                Selected = false,
            });

        lstEstados.Add(
            new SelectListItem()
            {
                Value = "3",
                Text = "COMPLETADO",
                Selected = false,
            });

        lstEstados.Add(
            new SelectListItem()
            {
                Value = "4",
                Text = "SIN PROYECCION",
                Selected = false,
            });

        lstEstados.Add(
            new SelectListItem()
            {
                Value = "5",
                Text = "PERDIDO",
                Selected = false,
            });
    }

    [HttpPost]
    [HttpGet]
    public async Task<IActionResult> Index([FromForm] Prestamo pago, string action)
    {
        this.Inicialized();

        ViewBag.Modulo = Modulo;
        ViewBag.Title = $"{Gestion}";
        ViewBag.Message = $"Gestión de {Modulo}";
        ViewBag.Ruta = $"{ViewBag.Modulo} > {ViewBag.Title}";
        ViewBag.Buscar = "Buscar";

        List<Prestamo> prestamos = [];

        try
        {
            this.entidadesResponse = await this.serviceCaller.ObtenerRegistros<EntidadesResponse>(ServicioEnum.Entidades);

            var lstEntidades = new List<SelectListItem>();
            foreach (var e in this.entidadesResponse?.Entidades)
            {
                lstEntidades.Add(
                new SelectListItem()
                {
                    Value = e.Id.ToString(),
                    Text = e.Nombre,
                    Selected = false
                });
            }

            this.prestamoResponse = await this.serviceCaller.ObtenerRegistros<PrestamoResponse>(ServicioEnum.Prestamos);
            this.prestamoDetalleResponse = await this.serviceCaller.ObtenerRegistros<PrestamoDetalleResponse>(ServicioEnum.PrestamoDetalles);
            
            prestamos = this.prestamoResponse.Prestamos;

            if (this.Request.Query.ContainsKey("search") && !string.IsNullOrEmpty(this.Request.Query["search"]))
            {
                string search = this.Request.Query["search"].ToString().ToUpper();

                prestamos = this.prestamoResponse.Prestamos?.FindAll(x => !string.IsNullOrEmpty(x.Beneficiario) &&  x.Beneficiario.Contains(search));

                if (prestamos?.Count == 0)
                {
                    prestamos = this.prestamoResponse.Prestamos?.FindAll(x => !string.IsNullOrEmpty(x.Numero) && x.Numero.Contains(search));
                }

                if (prestamos?.Count == 0)
                {
                    prestamos = this.prestamoResponse.Prestamos?.FindAll(x => !string.IsNullOrEmpty(x.Resumen) && x.Resumen.Contains(search));
                }

                if (prestamos?.Count == 0)
                {
                    prestamos = this.prestamoResponse.Prestamos?.FindAll(x => !string.IsNullOrEmpty(x.Razon) && x.Razon.Contains(search));
                }

                if (prestamos?.Count == 0)
                {
                    prestamos = this.prestamoResponse.Prestamos?.FindAll(x => !string.IsNullOrEmpty(x.Estado) && x.Estado.Contains(search));
                }

                ViewBag.Buscar = "Limpiar";
            }

            ViewBag.Entidades = lstEntidades;
            ViewBag.Estados = lstEstados;
            ViewBag.Prestamos = prestamos?.FindAll(p => p.Estado != "PERDIDO").ToList();
            ViewBag.PrestamosDetalles = this.prestamoDetalleResponse.PrestamoDetalles;

            return await Task.FromResult<IActionResult>(View());
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.ToString());
            return await Task.FromResult<IActionResult>(View(new List<Prestamo>()));
        }
    }

    public async Task<IActionResult> Prestamos([FromForm] Prestamo prestamo, string action)
    {
        this.Inicialized();

        _logger.LogInformation("Inicializando PrestamosController => Index()");
        ViewBag.Modulo = Modulo;
        ViewBag.Title = "Prestamos";
        ViewBag.Message = $"{Gestion} {Modulo}";
        ViewBag.ModeView = false;
        ViewBag.Buscar = "Buscar";

        ViewBag.Estados = lstEstados;
        string view = "Index";
        List<Prestamo> prestamos = [];
        List<PrestamoDetalle> prestamoDetalles = [];

        try
        {
            this.prestamoResponse = await this.serviceCaller.ObtenerRegistros<PrestamoResponse>(ServicioEnum.Prestamos);
            prestamos = this.prestamoResponse.Prestamos;

            this.prestamoDetalleResponse = await this.serviceCaller.ObtenerRegistros<PrestamoDetalleResponse>(ServicioEnum.PrestamoDetalles);
            
            this.entidadesResponse = await this.serviceCaller.ObtenerRegistros<EntidadesResponse>(ServicioEnum.Entidades);
            ViewBag.Entidades = this.entidadesResponse.Entidades;

            switch (action)
            {
                case "openFormAdd":
                case "openFormEdit":
                case "openFormView":

                    if (action == "openFormEdit" || action == "openFormView")
                    {
                        if (action == "openFormView")
                        {
                            ViewBag.ModeView = true;
                        }

                        ViewBag.Prestamo = prestamos?.Find(p => p.Id == int.Parse(this.Request.Form["Id"]));
                        prestamoDetalles = this.prestamoDetalleResponse.PrestamoDetalles?.FindAll(p => p.PrestamoId == int.Parse(this.Request.Form["Id"]));

                        view = "PrestamoFormEdit";
                    }
                    else
                    {
                        view = "PrestamoFormAdd";
                    }
                    
                    decimal totalMontos = prestamoDetalles?
                                                    .FindAll(p => p.Estado == "COMPLETADO")
                                                    .Sum(p => p.MontoCuota) ?? 0m;

                    ViewBag.PrestamosDetalles = prestamoDetalles
                                                    .OrderBy(o => o.Cuota)
                                                    .ToList();
                    ViewBag.TotalPagado = totalMontos;

                    return await Task.FromResult<IActionResult>(View(view, ViewBag));

                case "actualizar":
                case "generar":

                    var mapPrestamo = Utils.MapRequest<Prestamo>(this.Request.Form, ServicioEnum.Prestamos);

                    this.generalRequest = new GeneralRequest()
                    {
                        Parametros =
                        [
                           new Parametro()
                           {
                               Nombre = "inNumber",
                               Valor = mapPrestamo.Numero,
                           },
                            new Parametro()
                           {
                               Nombre = "inBeneficiary",
                               Valor = mapPrestamo.Beneficiario,
                           },
                            new Parametro()
                           {
                               Nombre = "inDepositDate",
                               Valor = mapPrestamo.FechaDeposito?.ToString("yyyy-MM-dd"),
                           },
                            new Parametro()
                           {
                               Nombre = "inReason",
                               Valor = mapPrestamo.Razon,
                           },
                            new Parametro()
                           {
                               Nombre = "inSummary",
                               Valor = mapPrestamo.Resumen,
                           },
                           new Parametro()
                           {
                               Nombre = "inCapitalAmount",
                               Valor = mapPrestamo.TotalCapital,
                           },
                           new Parametro()
                           {
                               Nombre = "inTotalAmount",
                               Valor = mapPrestamo.TotalDeuda,
                           },
                           new Parametro()
                           {
                               Nombre = "inFirstInstallmentAmount",
                               Valor = mapPrestamo.MontoCuota,
                           },
                           new Parametro()
                           {
                               Nombre = "inNumberOfInstallments",
                               Valor = mapPrestamo.Cuotas,
                           },
                            new Parametro()
                           {
                               Nombre = "inState",
                               Valor = mapPrestamo.Estado,
                           },
                           new Parametro()
                           {
                               Nombre = "inEntityId",
                               Valor = mapPrestamo.Entidad.Id,
                           },
                           new Parametro()
                           {
                               Nombre = "inTransactionCode",
                               Valor = mapPrestamo.CodigoTransaccion,
                           },
                        ]
                    };

                    generalDataResponse = new GeneralDataResponse();


                    if (action == "actualizar")
                    {
                        var parametro = new Parametro()
                        {
                            Nombre = "inId",
                            Valor = mapPrestamo.Id,
                        };
                        this.generalRequest.Parametros.Add(parametro);

                        this.generalDataResponse = await this.serviceCaller.ActualizarRegistro<GeneralDataResponse>(ServicioEnum.Prestamos, this.generalRequest);
                    }
                    else
                    {
                        this.generalDataResponse = await this.serviceCaller.GenerarRegistro<GeneralDataResponse>(ServicioEnum.Prestamos, this.generalRequest);
                    }                        

                    CacheAdmin.Remove(HttpContext, ServicioEnum.Prestamos);
                    CacheAdmin.Remove(HttpContext, ServicioEnum.PrestamoDetalles);

                    this.prestamoResponse = await this.serviceCaller.ObtenerRegistros<PrestamoResponse>(ServicioEnum.Prestamos);
                    prestamos = this.prestamoResponse.Prestamos;

                    ViewBag.Prestamos = prestamos;

                    return await Task.FromResult<IActionResult>(View(view, ViewBag));

                case "addDetail":
                    view = "PrestamoDetalleFormAdd";
                    ViewBag.Title = "Detalle Prestamo";
                    ViewBag.Message = $"{Gestion} Detalle Prestamo";
                    ViewBag.Prestamo = prestamos?.Find(p => p.Id == int.Parse(this.Request.Form["prestamoId"])); ;

                    return await Task.FromResult<IActionResult>(View(view, ViewBag));

                case "editDetail":
                    view = "PrestamoDetalleFormEdit";
                    ViewBag.Title = "Detalle Prestamo";
                    ViewBag.Message = $"{Gestion} Detalle Prestamo";

                    prestamoDetalles = this.prestamoDetalleResponse.PrestamoDetalles?.FindAll(p => p.Id == int.Parse(this.Request.Form["detalleId"]));

                    ViewBag.PrestamoDetalle = prestamoDetalles[0];

                    return await Task.FromResult<IActionResult>(View(view, ViewBag));

                case "actualizarDetail":
                case "generarDetail":
                    /*
                     logica para actualizar el detalle del prestamo
                     */

                    var mapPrestamoDetalle = Utils.MapRequest<PrestamoDetalle>(this.Request.Form, ServicioEnum.PrestamoDetalles);

                    this.generalRequest = new GeneralRequest()
                    {
                        Parametros =
                        [
                           new Parametro()
                           {
                               Nombre = "inLoansAssignedId",
                               Valor = mapPrestamoDetalle.PrestamoId,
                           },
                            new Parametro()
                           {
                               Nombre = "inNumberInstallment",
                               Valor = mapPrestamoDetalle.Cuota,
                           },
                            new Parametro()
                           {
                               Nombre = "inFeeAmount",
                               Valor = mapPrestamoDetalle.MontoCuota,
                           },
                            new Parametro()
                           {
                               Nombre = "inPaymentDate",
                               Valor = mapPrestamoDetalle.FechaPagado?.ToString("yyyy-MM-dd"),
                           },
                            new Parametro()
                           {
                               Nombre = "inProofOfPayment",
                               Valor = mapPrestamoDetalle.ComprobantePago,
                           },
                           new Parametro()
                           {
                               Nombre = "inPaymentMethod",
                               Valor = mapPrestamoDetalle.MetodoPago,
                           },
                           new Parametro()
                           {
                               Nombre = "inStatus",
                               Valor = mapPrestamoDetalle.Estado,
                           },
                           new Parametro()
                           {
                               Nombre = "inObservations",
                               Valor = mapPrestamoDetalle.Observaciones,
                           }
                        ]
                    };

                    generalDataResponse = new GeneralDataResponse();

                    if (action == "actualizarDetail")
                    {
                        var parametro = new Parametro()
                        {
                            Nombre = "inId",
                            Valor = mapPrestamoDetalle.Id,
                        };
                        this.generalRequest.Parametros.Add(parametro);

                        this.generalDataResponse = await this.serviceCaller.ActualizarRegistro<GeneralDataResponse>(ServicioEnum.PrestamoDetalles, this.generalRequest);
                    }
                    else
                    {
                        this.generalDataResponse = await this.serviceCaller.GenerarRegistro<GeneralDataResponse>(ServicioEnum.PrestamoDetalles, this.generalRequest);
                    }
                    
                    CacheAdmin.Remove(HttpContext, ServicioEnum.PrestamoDetalles);

                    this.CargarVariablesIniciales();
                    this.prestamoDetalleResponse = await this.serviceCaller.ObtenerRegistros<PrestamoDetalleResponse>(ServicioEnum.PrestamoDetalles);

                    view = "PrestamoFormEdit";

                    ViewBag.Prestamo = prestamos?.Find(p => p.Id == int.Parse(this.Request.Form["PrestamoId"]));
                    prestamoDetalles = this.prestamoDetalleResponse.PrestamoDetalles?.FindAll(p => p.PrestamoId == int.Parse(this.Request.Form["PrestamoId"]));
                    ViewBag.PrestamosDetalles = prestamoDetalles
                                                    .OrderBy(o => o.Cuota)
                                                    .ToList();

                    return await Task.FromResult<IActionResult>(View(view, ViewBag));

                default:
                    ViewBag.Prestamos = prestamos;

                    return await Task.FromResult<IActionResult>(View(view, ViewBag));
            }



        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.ToString());

            return await Task.FromResult<IActionResult>(View(new List<Pago>()));
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private void CargarVariablesIniciales()
    {
        ViewBag.Modulo = Modulo;
        ViewBag.Title = "Prestamos";
        ViewBag.Message = $"{Gestion} {Modulo}";
        ViewBag.ModeView = false;
        ViewBag.Buscar = "Buscar";
    }
}
