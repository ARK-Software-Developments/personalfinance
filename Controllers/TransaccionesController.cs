namespace PersonalFinance.Controllers;

using Microsoft.AspNetCore.Mvc;
using PersonalFinance.Helper;
using PersonalFinance.Models;
using PersonalFinance.Models.Enums;
using PersonalFinance.Models.Tarjetas;
using PersonalFinance.Models.Transacciones;
using PersonalFinance.Service;
using System.Diagnostics;
using System.Net.Http;

public class TransaccionesController : BaseController
{
    private readonly string Gestion = "Administrar";
    private readonly string Modulo = "Transacciones";
    private readonly ILogger<TransaccionesController> _logger;

    public TransaccionesController(ILogger<TransaccionesController> logger)
    {
        _logger = logger;
        this.httpClientHandler = new()
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
        };
    }

    public async Task<IActionResult> Index([FromForm] Transaccion entidad, string action)
    {
        this.Inicialized();

        _logger.LogInformation("Inicializando TarjetasController => Index()");
        ViewBag.Modulo = Modulo;
        ViewBag.Title = $"{Gestion}";
        ViewBag.Message = $"Gestión de {Modulo}";

        try
        {
            transaccionesResponse = await this.serviceCaller.ObtenerRegistros<TransaccionesResponse>(ServicioEnum.Transacciones);

            ViewBag.Transacciones = transaccionesResponse.Transacciones;

            return View(ViewBag);

        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.ToString());

            return View(new List<Transaccion>());
        }
    }

    public async Task<IActionResult> Transacciones([FromForm] Transaccion transaccion, string action, int TarjetaSel)
    {
        this.Inicialized();

        _logger.LogInformation("Inicializando TarjetasController => Index()");
        
        ViewBag.Modulo = Modulo;
        ViewBag.Title = "Transacciones de Tarjetas";
        ViewBag.Message = $"{Gestion} {Modulo}";

        try
        {
            if (action == "generar" || action == "actualizar")
            {
                tarjetasResponse = await this.serviceCaller.ObtenerRegistros<TarjetasResponse>(ServicioEnum.Tarjetas);

                generalRequest = new()
                {
                    Parametros =
                        [
                         new Parametro()
                         {
                             Nombre = "pCardId",
                             Valor = TarjetaSel,
                         },
                         new Parametro()
                         {
                             Nombre = "pTransactionCode",
                             Valor = transaccion.CodigoTransaccion,
                         },
                         new Parametro()
                         {
                             Nombre = "pPurchaseOrder",
                             Valor = transaccion.OrdenCompra,
                         },
                         new Parametro()
                         {
                             Nombre = "pAssociatedEntity",
                             Valor = transaccion.EntidadAsociada,
                         },
                         new Parametro()
                         {
                             Nombre = "pTransactionDate",
                             Valor = transaccion.FechaTransaccion.ToString("yyyy-MM-dd"),
                         },
                         new Parametro()
                         {
                             Nombre = "pSummary",
                             Valor = transaccion.Resumen,
                         },
                         new Parametro()
                         {
                             Nombre = "pObservations",
                             Valor = transaccion.Observaciones,
                         },
                     ],
                };

                if (action == "actualizar")
                {
                    generalRequest.Parametros.Add(
                        new Parametro()
                        {
                            Nombre = "pId",
                            Valor = transaccion.Id
                        });

                    generalDataResponse = await this.serviceCaller.ActualizarRegistro<GeneralDataResponse>(ServicioEnum.Transacciones, generalRequest);

                }
                else
                {
                    generalDataResponse = await this.serviceCaller.GenerarRegistro<GeneralDataResponse>(ServicioEnum.Transacciones, generalRequest);

                    if (this.Request.Form.ContainsKey("ConsumoTarjeta"))
                    {
                        int ConsumoTarjetaId = int.Parse(this.Request.Form["ConsumoTarjeta"]);

                        int transactionCodeId = int.Parse(generalDataResponse.Data[0].ToString());

                        generalRequest = new()
                        {
                            Parametros =
                        [
                         new Parametro()
                         {
                             Nombre = "pId",
                             Valor = ConsumoTarjetaId,
                         },
                         new Parametro()
                         {
                             Nombre = "pTransactionCodeId",
                             Valor = transactionCodeId,
                         }
                     ],
                        };

                        await this.serviceCaller.ActualizarRegistro<GeneralDataResponse>(ServicioEnum.ConsumosTarjeta, generalRequest, MetodoEnum.ActualizarTransId);
                    }


                }

                CacheAdmin.Remove(HttpContext, ServicioEnum.Transacciones);
            }


            transaccionesResponse = await this.serviceCaller.ObtenerRegistros<TransaccionesResponse>(ServicioEnum.Transacciones);

            ViewBag.Transacciones = transaccionesResponse?.Transacciones;
            
            return await Task.FromResult<IActionResult>(View("Index", ViewBag));

        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.ToString());

            return View(new List<Tarjeta>());
        }
    }

    [HttpPost]
    public async Task<IActionResult> TransaccionFormAdd([FromForm] Transaccion transaccion, string action)
    {
        this.Inicialized();

        // Procesa los datos del formulario que están en el objeto 'model'
        // Por ejemplo, guarda en una base de datos  
        ViewBag.Modulo = Modulo;
        ViewBag.Action = action;
        ViewBag.Transaccion = transaccion;
        ViewBag.Tarjetas = new List<Tarjeta>();

        tarjetasResponse = await this.serviceCaller.ObtenerRegistros<TarjetasResponse>(ServicioEnum.Tarjetas);

        ViewBag.TarjetaConsumoId = this.Request.Form.ContainsKey("TarjetaConsumoId") ? int.Parse(this.Request.Form["TarjetaConsumoId"]) : 0;
        ViewBag.TarjetaSel = this.Request.Form.ContainsKey("TarjetaSel") ? int.Parse(this.Request.Form["TarjetaSel"]) : 0; ;
        
        ViewBag.Tarjetas = tarjetasResponse?.Tarjetas;

        return await Task.FromResult<IActionResult>(View(ViewBag)); // Redirige a otra página
    }

    [HttpPost]
    public async Task<IActionResult> TransaccionFormEdit([FromForm] Transaccion transaccion, string action)
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

                transaccionesResponse = await this.serviceCaller.ObtenerRegistros<TransaccionesResponse>(ServicioEnum.Transacciones);

                tarjetasResponse = await this.serviceCaller.ObtenerRegistros<TarjetasResponse>(ServicioEnum.Tarjetas);

                ViewBag.ModeView = action == "openFormView" ? true : false;
                ViewBag.Transaccion = transaccionesResponse.Transacciones.Find(t => t.Id == transaccion.Id);
                ViewBag.Tarjetas = tarjetasResponse?.Tarjetas;

                return await Task.FromResult<IActionResult>(View(ViewBag)); // Redirige a otra página

            default:

                return await Task.FromResult<IActionResult>(View("Index", transaccion));
        }

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}