namespace PersonalFinance.Controllers;

#pragma warning disable CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL

using Microsoft.AspNetCore.Mvc;
using PersonalFinance.Helper;
using PersonalFinance.Models;
using PersonalFinance.Models.Categorias;
using PersonalFinance.Models.Entidades;
using PersonalFinance.Models.Enums;
using PersonalFinance.Models.Gastos;
using PersonalFinance.Models.Pagos;
using PersonalFinance.Models.Prestamos;
using PersonalFinanceApiNetCoreModel;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;

public class PrestamosController : BaseController
{
    private readonly string Gestion = "Administrar";
    private readonly string Modulo = "Prestamos";
    private readonly ILogger<PrestamosController> _logger;

    public PrestamosController(ILogger<PrestamosController> logger)
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
            this.prestamoResponse = await this.serviceCaller.ObtenerRegistros<PrestamoResponse>(ServicioEnum.Prestamos);

            prestamos = this.prestamoResponse.Prestamos;

            if (this.Request.Query.ContainsKey("search") && !string.IsNullOrEmpty(this.Request.Query["search"]))
            {
                string search = this.Request.Query["search"].ToString().ToUpper();

                prestamos = this.prestamoResponse.Prestamos?.FindAll(x => !string.IsNullOrEmpty(x.Beneficiario) &&  x.Beneficiario.Contains(search));

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

            ViewBag.Prestamos = prestamos;

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
        ViewBag.Title = "Pagos";
        ViewBag.Message = $"{Gestion} {Modulo}";

        try
        {

            pagosResponse = await this.serviceCaller.ObtenerRegistros<PagosResponse>(ServicioEnum.Pagos);


            switch (action)
            {
                case "openFormAdd":

                    entidadesResponse = await this.serviceCaller.ObtenerRegistros<EntidadesResponse>(ServicioEnum.Entidades);
                    var gasto = pagosResponse?.Pagos?.Find(p => p.Id == prestamo.Id);

                    tiposGastosResponse = await this.serviceCaller.ObtenerRegistros<TiposGastosResponse>(ServicioEnum.TipoGastos, keyValuePairs);

                    ViewBag.Gasto = gasto;
                    ViewBag.Entidades = entidadesResponse.Entidades;
                    ViewBag.TiposGastos = tiposGastosResponse.TiposGastos;
                    return await Task.FromResult<IActionResult>(View("PagosPedidosFormAdd", ViewBag));

                case "generar":

                    var mapPago = Utils.MapRequest<Pago>(this.Request.Form, ServicioEnum.Pagos);

                    this.generalRequest = new GeneralRequest()
                    {
                        Parametros =
                        [
                           new Parametro()
                           {
                               Nombre = "pRegistrationDate",
                               Valor = mapPago.FechaRegistro?.ToString("yyyy-MM-dd"),
                           },
                            new Parametro()
                           {
                               Nombre = "pDateOfPayment",
                               Valor = mapPago.FechaPago?.ToString("yyyy-MM-dd"),
                           },
                            new Parametro()
                           {
                               Nombre = "pRegistrationCode",
                               Valor = mapPago.CodigoRegistro,
                           },
                            new Parametro()
                           {
                               Nombre = "pPaymentResourceId",
                               Valor = mapPago.RecursoDelPago.Id,
                           },
                            new Parametro()
                           {
                               Nombre = "pPaymentType",
                               Valor = mapPago.TipoDePago,
                           },
                            new Parametro()
                           {
                               Nombre = "pBudgetedAmount",
                               Valor = mapPago.MontoPresupuestado,
                           },
                           new Parametro()
                           {
                               Nombre = "pAmountPaid",
                               Valor = mapPago.MontoPagado,
                           },
                           new Parametro()
                           {
                               Nombre = "pReasonForPayment",
                               Valor = mapPago.TipoDeGasto.Id,
                           },
                        ]
                    };

                    generalDataResponse = new GeneralDataResponse();
                    generalDataResponse = await this.serviceCaller.GenerarRegistro<GeneralDataResponse>(ServicioEnum.Pagos, this.generalRequest);

                    CacheAdmin.Remove(HttpContext, ServicioEnum.Pagos);
                    pagosResponse = await this.serviceCaller.ObtenerRegistros<PagosResponse>(ServicioEnum.Pagos);

                    ViewBag.Pagos = pagosResponse.Pagos;

                    return await Task.FromResult<IActionResult>(View(ViewBag));

                default:
                    ViewBag.Pagos = pagosResponse.Pagos;

                    return await Task.FromResult<IActionResult>(View(ViewBag));
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
}
