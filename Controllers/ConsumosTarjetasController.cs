namespace PersonalFinance.Controllers;

#pragma warning disable CS8601 // Posible asignación de referencia nula

using Microsoft.AspNetCore.Mvc;
using PersonalFinance.Helper;
using PersonalFinance.Models;
using PersonalFinance.Models.Enums;
using PersonalFinance.Models.TarjetaConsumos;
using PersonalFinance.Models.Tarjetas;
using PersonalFinance.Models.Transacciones;
using System.Diagnostics;
using System.Net.Http;

public class ConsumosTarjetasController : BaseController
{
    private readonly string Gestion = "Administrar";
    private readonly string Modulo = "ConsumosTarjetas";
    private readonly ILogger<ConsumosTarjetasController> _logger;
    
    public ConsumosTarjetasController(ILogger<ConsumosTarjetasController> logger)
    {
        _logger = logger;
        this.httpClientHandler = new()
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
        }; 
    }

    public async Task<IActionResult> Index([FromForm] TarjetaConsumo tarjetaConsumo, string action)
    {
        this.Inicialized();

        _logger.LogInformation("Inicializando TarjetasController => Index()");
        ViewBag.Modulo = Modulo;
        ViewBag.Title = $"{Gestion}";
        ViewBag.Message = $"Gestión de {Modulo}";

        try
        {
            tarjetaConsumoResponse = await this.serviceCaller.ObtenerRegistros<TarjetaConsumoResponse>(ServicioEnum.ConsumosTarjeta, keyValuePairs);
            ViewBag.TarjetaConsumos = tarjetaConsumoResponse.TarjetaConsumos;

            return await Task.FromResult<IActionResult>(View(ViewBag));
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.ToString());

            return await Task.FromResult<IActionResult>(View(new List<TarjetaConsumo>()));
        }
    }

    [HttpPost]
    public async Task<IActionResult> ConsumosTarjetas([FromForm] TarjetaConsumo tarjetaConsumo, string action, int VilleteraSel, int TipoGastoSel)
    {
        this.Inicialized();
        ViewBag.Modulo = Modulo;
        ViewBag.Title = $"Formulario de {Modulo}";
        ViewBag.Message = $"{Gestion} {Modulo}";

        try
        {
            if (action == "generar" || action == "actualizar")
            {
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

                CacheAdmin.Remove(HttpContext, ServicioEnum.ConsumosTarjeta);

            }

            tarjetaConsumoResponse = await this.serviceCaller.ObtenerRegistros<TarjetaConsumoResponse>(ServicioEnum.ConsumosTarjeta, keyValuePairs);

            ViewBag.TarjetaConsumos = tarjetaConsumoResponse?.TarjetaConsumos;
            
            //return View("Index");
            return await Task.FromResult<IActionResult>(View("Index", ViewBag));

        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.ToString());

            return await Task.FromResult<IActionResult>(View(new List<TarjetaConsumo>()));
        }
    }

    [HttpPost]
    public async Task<IActionResult> ConsumosTarjetasFormAdd([FromForm] TarjetaConsumo tarjetaConsumo, string action)
    {
        this.Inicialized();

        // Procesa los datos del formulario que están en el objeto 'model'
        // Por ejemplo, guarda en una base de datos  
        ViewBag.Modulo = Modulo;
        ViewBag.Action = action;
        ViewBag.TarjetaConsumo = tarjetaConsumo;
        ViewBag.Title = $"Formulario de {Modulo}";

        // Obtener Tarjetas
        tarjetasResponse = await this.serviceCaller.ObtenerRegistros<TarjetasResponse>(ServicioEnum.Tarjetas);
        ViewBag.Tarjetas = tarjetasResponse.Tarjetas;

        // Obtener Consumos Tarjetas
        tarjetaConsumoResponse = await this.serviceCaller.ObtenerRegistros<TarjetaConsumoResponse>(ServicioEnum.ConsumosTarjeta, keyValuePairs);
        ViewBag.TarjetaConsumos = tarjetaConsumoResponse?.TarjetaConsumos;

        return await Task.FromResult<IActionResult>(View(ViewBag)); // Redirige a otra página
    }

    [HttpPost]
    public async Task<IActionResult> ConsumosTarjetasFormEdit([FromForm] TarjetaConsumo tarjetaConsumo, string action)
    {
        this.Inicialized();

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
                tarjetaConsumoResponse = await this.serviceCaller.ObtenerRegistros<TarjetaConsumoResponse>(ServicioEnum.ConsumosTarjeta, keyValuePairs);
                var tmpTarjetaConsumo = tarjetaConsumoResponse?.TarjetaConsumos.Find(t => t.Id == tarjetaConsumo.Id);
                ViewBag.TarjetaConsumo = tmpTarjetaConsumo;

                // Obtener Transacciones
                transaccionesResponse = await this.serviceCaller.ObtenerRegistros<TransaccionesResponse>(ServicioEnum.Transacciones);
                ViewBag.Transacciones = transaccionesResponse?.Transacciones.FindAll(t => t.TarjetaConsumoId == tmpTarjetaConsumo.Id);

                // Obtener Tarjetas
                tarjetasResponse = await this.serviceCaller.ObtenerRegistros<TarjetasResponse>(ServicioEnum.Tarjetas);
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