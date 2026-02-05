namespace PersonalFinance.Controllers;

using Microsoft.AspNetCore.Mvc;
using PersonalFinance.Helper;
using PersonalFinance.Models;
using PersonalFinance.Models.Entidades;
using PersonalFinance.Models.Enums;
using PersonalFinance.Models.TarjetaConsumos;
using PersonalFinance.Models.Tarjetas;
using PersonalFinanceApiNetCoreModel;
using System.Diagnostics;
using System.Net.Http;

public class TarjetasController : BaseController
{
    private readonly string Gestion = "Administrar";
    private readonly string Modulo = "Tarjetas";
    private readonly ILogger<TarjetasController> _logger;

    public TarjetasController(ILogger<TarjetasController> logger)
    {
        _logger = logger;
        this.httpClientHandler = new()
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
        };
    }

    public async Task<IActionResult> Index([FromForm] Tarjeta entidad, string action)
    {
        this.Inicialized();

        _logger.LogInformation("Inicializando TarjetasController => Index()");
        TarjetasResponse response = new();
        ViewBag.Modulo = Modulo;
        ViewBag.Title = $"{Gestion}";
        ViewBag.Message = $"Gestión de {Modulo}";

        try
        {
            /*
            switch (action)
            {
                case "generar":
                    await this._service.Generar(entidad);
                    HttpContext.Session.Remove(cacheData);
                    break;

                case "actualizar":
                    await this._service.Actualizar(entidad);
                    HttpContext.Session.Remove(cacheData);
                    break;

                default:
                    response = await this.Obtener();
                    
                    break;
            }

            var dataPedidos = HttpContext.Session.GetString(cacheData);

            if (dataPedidos == null)
            {
                response = await this.Obtener();
            }
            else
            {
                response = JsonConvert.DeserializeObject<CategoriasResponse>(dataPedidos);
            }

            ViewBag.Categorias = response.Categoria;
            */

            return await Task.FromResult<IActionResult>(View());
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.ToString());
            return await Task.FromResult<IActionResult>(View(new List<Tarjeta>()));
        }
    }

    public async Task<IActionResult> TarjetasCredito([FromForm] Tarjeta tarjeta, string action, int EntidadSel)
    {
        this.Inicialized();

        _logger.LogInformation("Inicializando TarjetasController => Index()");        
        ViewBag.Modulo = Modulo;
        ViewBag.Title = "Tarjetas de Crétitos";
        ViewBag.Message = $"{Gestion} {Modulo}";
        Entidad entidad;

        try
        {
            this.entidadesResponse = await this.serviceCaller.ObtenerRegistros<EntidadesResponse>(ServicioEnum.Entidades);
            entidad = entidadesResponse.Entidades.Find(e => e.Id == EntidadSel);

            switch (action)
            {
                case "generar":

                    this.generalRequest = new()
                    {
                        Parametros =
                        [
                         new Parametro()
                         {
                             Nombre = "pCardName",
                             Valor = tarjeta.Nombre,
                         },
                         new Parametro()
                         {
                             Nombre = "pClosingDate",
                             Valor = tarjeta.FechaCierre.ToString("yyyy-MM-dd"),
                         },
                         new Parametro()
                         {
                             Nombre = "pExpirationDate",
                             Valor = tarjeta.FechaVencimiento.ToString("yyyy-MM-dd"),
                         },
                         new Parametro()
                         {
                             Nombre = "pEntityId",
                             Valor = entidad.Id,
                         },
                         new Parametro()
                         {
                             Nombre = "pActive",
                             Valor = 1,
                         },
                     ],
                    };
                    
                    await this.serviceCaller.GenerarRegistro<GeneralDataResponse>(ServicioEnum.Tarjetas, generalRequest);

                    CacheAdmin.Remove(HttpContext, ServicioEnum.Tarjetas);
                    break;
                    
                case "actualizar":

                    this.generalRequest = new()
                    {
                        Parametros =
                        [
                         new Parametro()
                         {
                             Nombre = "pId",
                             Valor = tarjeta.Id,
                         },
                         new Parametro()
                         {
                             Nombre = "pCardName",
                             Valor = tarjeta.Nombre,
                         },
                         new Parametro()
                         {
                             Nombre = "pClosingDate",
                             Valor = tarjeta.FechaCierre.ToString("yyyy-MM-dd"),
                         },
                         new Parametro()
                         {
                             Nombre = "pExpirationDate",
                             Valor = tarjeta.FechaVencimiento.ToString("yyyy-MM-dd"),
                         },
                         new Parametro()
                         {
                             Nombre = "pEntityId",
                             Valor = entidad.Id,
                         },
                         new Parametro()
                         {
                             Nombre = "pActive",
                             Valor = 1,
                         },
                     ],
                    };

                    await this.serviceCaller.ActualizarRegistro<GeneralDataResponse>(ServicioEnum.Tarjetas, generalRequest);
                    CacheAdmin.Remove(HttpContext, ServicioEnum.Tarjetas);
                    break;

            }

            this.tarjetasResponse = await this.serviceCaller.ObtenerRegistros<TarjetasResponse>(ServicioEnum.Tarjetas);

            ViewBag.Tarjetas = this.tarjetasResponse?.Tarjetas;
            
            return View(ViewBag);

        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.ToString());

            return View(new List<Tarjeta>());
        }
    }

    [HttpPost]
    public async Task<IActionResult> TarjetaFormAdd([FromForm] Tarjeta entidad, string action)
    {
        this.Inicialized();

        // Procesa los datos del formulario que están en el objeto 'model'
        // Por ejemplo, guarda en una base de datos  
        ViewBag.Modulo = Modulo;
        ViewBag.Tarjeta = entidad;

        this.entidadesResponse = await this.serviceCaller.ObtenerRegistros<EntidadesResponse>(ServicioEnum.Entidades);

        ViewBag.Entidades = this.entidadesResponse.Entidades;

        return await Task.FromResult<IActionResult>(View(ViewBag)); // Redirige a otra página
    }

    [HttpPost]
    public async Task<IActionResult> TarjetaFormEdit([FromForm] Tarjeta entidad, string action)
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

                this.tarjetasResponse = await this.serviceCaller.ObtenerRegistros<TarjetasResponse>(ServicioEnum.Tarjetas);

                entidad = this.tarjetasResponse.Tarjetas.Find(t => t.Id == entidad.Id);

                this.entidadesResponse = await this.serviceCaller.ObtenerRegistros<EntidadesResponse>(ServicioEnum.Entidades);

                this.generalRequest = new()
                {
                    Parametros =
                        [
                         new Parametro()
                         {
                             Nombre = "pYear",
                             Valor = Utils.GetYear(HttpContext),
                         },
                         new Parametro()
                         {
                             Nombre = "pCardsId",
                             Valor = entidad.Id,
                         },
                     ],
                };

                this.tarjetaConsumoResumenResponse = await this.serviceCaller.ObtenerRegistros<TarjetaConsumoResumenResponse>(ServicioEnum.ConsumosTarjetaResumen, this.generalRequest, MetodoEnum.TarjetaConsumoResumen);


                ViewBag.ModeView = action == "openFormView" ? true : false;
                ViewBag.Tarjeta = entidad;
                ViewBag.TarjetaResumen = this.tarjetaConsumoResumenResponse.TarjetaConsumoResumenes;
                ViewBag.Entidades = entidadesResponse.Entidades;

                return await Task.FromResult<IActionResult>(View(ViewBag)); // Redirige a otra página

            default:

                return await Task.FromResult<IActionResult>(View("Index", entidad));
        }

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
