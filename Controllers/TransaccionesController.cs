namespace PersonalFinance.Controllers;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PersonalFinance.Models;
using PersonalFinance.Models.Entidades;
using PersonalFinance.Models.Enums;
using PersonalFinance.Models.Tarjetas;
using PersonalFinance.Models.Transacciones;
using PersonalFinance.Service;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

public class TransaccionesController : Controller
{
    private readonly string Gestion = "Administrar";
    private readonly string Modulo = "Transacciones";
    private readonly string cacheNameDataTransacciones = "cacheDataTransacciones";
    private readonly string cacheNameDataTarjetas = "cacheDataTarjetas";
    private readonly ILogger<TransaccionesController> _logger;
    private readonly ServiceCaller serviceCaller;

    private GeneralDataResponse generalDataResponse = new();
    private TransaccionesResponse response = new();
    private TarjetasResponse tarjetasResponse = new();
    private GeneralRequest generalRequest = new();
    private string cacheTransacciones = string.Empty;
    private string cacheTarjetas = string.Empty;

    public TransaccionesController(ILogger<TransaccionesController> logger)
    {
        _logger = logger;
        HttpClientHandler httpClientHandler = new()
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
        };
        this.serviceCaller = new ServiceCaller(new HttpClient(httpClientHandler));
    }


    public async Task<IActionResult> Index([FromForm] Transaccion entidad, string action)
    {
        _logger.LogInformation("Inicializando TarjetasController => Index()");
        TransaccionesResponse response = new();
        ViewBag.Modulo = Modulo;
        ViewBag.Title = $"{Gestion}";
        ViewBag.Message = $"Gestión de {Modulo}";

        try
        {
            var cacheTransacciones = HttpContext.Session.GetString(cacheNameDataTransacciones);
            if (cacheTransacciones == null)
            {
                response = await this.serviceCaller.ObtenerRegistros<TransaccionesResponse>(ServicioEnum.Transacciones);
            }
            else
            {
                response = JsonConvert.DeserializeObject<TransaccionesResponse>(cacheTransacciones);
            }

            ViewBag.Transacciones = response.Transacciones;

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
        _logger.LogInformation("Inicializando TarjetasController => Index()");
        
        ViewBag.Modulo = Modulo;
        ViewBag.Title = "Transacciones de Tarjetas";
        ViewBag.Message = $"{Gestion} {Modulo}";

        try
        {
            if (action == "generar" || action == "actualizar")
            {
                cacheTarjetas = HttpContext.Session.GetString(cacheNameDataTarjetas);

                if (cacheTarjetas == null)
                {
                    tarjetasResponse = await this.serviceCaller.ObtenerRegistros<TarjetasResponse>(ServicioEnum.Tarjetas);
                }
                else
                {
                    tarjetasResponse = JsonConvert.DeserializeObject<TarjetasResponse>(cacheTarjetas);
                }

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
                }
                
                HttpContext.Session.Remove(cacheNameDataTransacciones);

            }
                       

            cacheTransacciones = HttpContext.Session.GetString(cacheNameDataTransacciones);

            if (cacheTransacciones == null)
            {
                response = await this.serviceCaller.ObtenerRegistros<TransaccionesResponse>(ServicioEnum.Transacciones);

                HttpContext.Session.SetString(cacheNameDataTransacciones, JsonConvert.SerializeObject(response));
            }
            else
            {
                response = JsonConvert.DeserializeObject<TransaccionesResponse>(cacheTransacciones);
            }

            ViewBag.Transacciones = response?.Transacciones;
            
            //return View("Index");
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
        // Procesa los datos del formulario que están en el objeto 'model'
        // Por ejemplo, guarda en una base de datos  
        ViewBag.Modulo = Modulo;
        ViewBag.Action = action;
        ViewBag.Transaccion = transaccion;
        ViewBag.Tarjetas = new List<Tarjeta>();

        cacheTarjetas = HttpContext.Session.GetString(cacheNameDataTarjetas);

        if (cacheTarjetas == null)
        {
            tarjetasResponse = await this.serviceCaller.ObtenerRegistros<TarjetasResponse>(ServicioEnum.Tarjetas);
        }
        else
        {
            tarjetasResponse = JsonConvert.DeserializeObject<TarjetasResponse>(cacheTarjetas);
        }

        ViewBag.Tarjetas = tarjetasResponse?.Tarjetas;

        return await Task.FromResult<IActionResult>(View(ViewBag)); // Redirige a otra página
    }

    [HttpPost]
    public async Task<IActionResult> TransaccionFormEdit([FromForm] Transaccion transaccion, string action)
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

                cacheTransacciones = HttpContext.Session.GetString(cacheNameDataTransacciones);

                if (cacheTransacciones == null)
                {
                    response = await this.serviceCaller.ObtenerRegistros<TransaccionesResponse>(ServicioEnum.Transacciones);

                    HttpContext.Session.SetString(cacheNameDataTransacciones, JsonConvert.SerializeObject(response));
                }
                else
                {
                    response = JsonConvert.DeserializeObject<TransaccionesResponse>(cacheTransacciones);
                }

                cacheTarjetas = HttpContext.Session.GetString(cacheNameDataTarjetas);
                
                if (cacheTarjetas == null)
                {
                    tarjetasResponse = await this.serviceCaller.ObtenerRegistros<TarjetasResponse>(ServicioEnum.Tarjetas);
                }
                else
                {
                    tarjetasResponse = JsonConvert.DeserializeObject<TarjetasResponse>(cacheTarjetas);
                }

                ViewBag.ModeView = action == "openFormView" ? true : false;
                ViewBag.Transaccion = response.Transacciones.Find(t => t.Id == transaccion.Id);
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