namespace PersonalFinance.Controllers;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using PersonalFinance.Helper;
using PersonalFinance.Models;
using PersonalFinance.Models.Entidades;
using PersonalFinance.Models.Enums;
using PersonalFinance.Models.Gastos;
using PersonalFinance.Models.Pagos;
using PersonalFinance.Models.TarjetaConsumos;
using PersonalFinance.Models.Tarjetas;
using PersonalFinance.Service;
using System.Diagnostics;
using System.Net.Http;

public class PagosController : BaseController
{
    private readonly string Gestion = "Administrar";
    private readonly string Modulo = "Pagos";
    private readonly ILogger<PagosController> _logger;

    public PagosController(ILogger<PagosController> logger)
    {
        _logger = logger;
        this.httpClientHandler = new()
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
        };
    }

    [HttpPost]
    [HttpGet]
    public async Task<IActionResult> Index([FromForm] Pago pago, string action)
    {
        this.Inicialized();

        ViewBag.Modulo = Modulo;
        ViewBag.Title = $"{Gestion}";
        ViewBag.Message = $"Gestión de {Modulo}";

        try
        {

            return await Task.FromResult<IActionResult>(View());
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.ToString());
            return await Task.FromResult<IActionResult>(View(new List<Pago>()));
        }
    }

    public async Task<IActionResult> PagosPedidos([FromForm] Pago pago, string action)
    {
        this.Inicialized();

        _logger.LogInformation("Inicializando TarjetasController => Index()");
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
                    var gasto = pagosResponse?.Pagos?.Find(p => p.Id == pago.Id);

                    tarjetaConsumoResponse = await this.serviceCaller.ObtenerRegistros<TarjetaConsumoResponse>(ServicioEnum.ConsumosTarjeta, keyValuePairs);
                    var tarjetaConsumos = tarjetaConsumoResponse?.TarjetaConsumos?.FindAll(c => c.TransaccionesAsociadas == 0);

                    ViewBag.Gasto = gasto;
                    ViewBag.Entidades = entidadesResponse.Entidades;
                    ViewBag.TarjetaConsumos = tarjetaConsumos;
                    return await Task.FromResult<IActionResult>(View("PagosPedidosFormAdd", ViewBag));

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

    public void HandleChange(ChangeEventArgs args)
    {
        var algo = args;
    }
}
