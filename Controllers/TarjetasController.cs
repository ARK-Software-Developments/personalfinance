namespace PersonalFinance.Controllers;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PersonalFinance.Models;
using PersonalFinance.Models.Entidades;
using PersonalFinance.Models.Enums;
using PersonalFinance.Models.Tarjetas;
using PersonalFinance.Service;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

public class TarjetasController : Controller
{
    private readonly string Gestion = "Administrar";
    private readonly string Modulo = "Tarjetas";
    private readonly string cacheData = "dataTarjetas";
    private readonly ILogger<TarjetasController> _logger;
    private readonly ServiceCaller serviceCaller;    

    public TarjetasController(ILogger<TarjetasController> logger)
    {
        _logger = logger;
        HttpClientHandler httpClientHandler = new()
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
        };
        this.serviceCaller = new ServiceCaller(new HttpClient(httpClientHandler));
    }


    public async Task<IActionResult> Index([FromForm] Tarjeta entidad, string action)
    {
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
            return View();

        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.ToString());

            return View(new List<Tarjeta>());
        }
    }

    public async Task<IActionResult> TarjetasCredito([FromForm] Tarjeta tarjeta, string action, int EntidadSel)
    {
        _logger.LogInformation("Inicializando TarjetasController => Index()");
        GeneralDataResponse generalDataResponse = new ();
        TarjetasResponse response = new ();
        EntidadesResponse entidadesResponse = new ();
        GeneralRequest generalRequest = new ();
        Entidad entidad = new ();
        string cacheEstidades = string.Empty;
        string dataCache = string.Empty;
        ViewBag.Modulo = Modulo;
        ViewBag.Title = "Tarjetas de Crétitos";
        ViewBag.Message = $"{Gestion} {Modulo}";

        try
        {
            switch (action)
            {
                case "generar":

                    cacheEstidades = HttpContext.Session.GetString("dataEntidades");
                    if (cacheEstidades == null)
                    {
                        entidadesResponse = await this.serviceCaller.ObtenerRegistros<EntidadesResponse>(ServicioEnum.Entidades);
                    }
                    else
                    {
                        entidadesResponse = JsonConvert.DeserializeObject<EntidadesResponse>(cacheEstidades);
                    }

                    entidad = entidadesResponse.Entidades.Find(e => e.Id == EntidadSel);

                    generalRequest = new()
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
                    generalDataResponse = await this.serviceCaller.GenerarRegistro<GeneralDataResponse>(ServicioEnum.Tarjetas, generalRequest);
                    HttpContext.Session.Remove(cacheData);
                    break;
                    
                case "actualizar":
                    cacheEstidades = HttpContext.Session.GetString("dataEntidades");
                    if (cacheEstidades == null)
                    {
                        entidadesResponse = await this.serviceCaller.ObtenerRegistros<EntidadesResponse>(ServicioEnum.Entidades);
                    }
                    else
                    {
                        entidadesResponse = JsonConvert.DeserializeObject<EntidadesResponse>(cacheEstidades);
                    }

                    entidad = entidadesResponse.Entidades.Find(e => e.Id == EntidadSel);

                    generalRequest = new()
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

                    generalDataResponse = await this.serviceCaller.ActualizarRegistro<GeneralDataResponse>(ServicioEnum.Tarjetas, generalRequest);
                    HttpContext.Session.Remove(cacheData);
                    break;

            }

            dataCache = HttpContext.Session.GetString(cacheData);

            if (dataCache == null)
            {
                response = await this.serviceCaller.ObtenerRegistros<TarjetasResponse>(ServicioEnum.Tarjetas);

                HttpContext.Session.SetString(cacheData, JsonConvert.SerializeObject(response));
            }
            else
            {
                response = JsonConvert.DeserializeObject<TarjetasResponse>(dataCache);
            }

            ViewBag.Tarjetas = response?.Tarjetas;
            
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
        // Procesa los datos del formulario que están en el objeto 'model'
        // Por ejemplo, guarda en una base de datos  
        ViewBag.Modulo = Modulo;
        ViewBag.Tarjeta = entidad;

        var dataEstidades = HttpContext.Session.GetString("dataEntidades");
        EntidadesResponse entidadesResponse;
        if (dataEstidades == null)
        {
            entidadesResponse = await this.serviceCaller.ObtenerRegistros<EntidadesResponse>(ServicioEnum.Entidades);
        }
        else
        {
            entidadesResponse = JsonConvert.DeserializeObject<EntidadesResponse>(dataEstidades);
        }

        ViewBag.Entidades = entidadesResponse.Entidades;

        return await Task.FromResult<IActionResult>(View(ViewBag)); // Redirige a otra página
    }

    [HttpPost]
    public async Task<IActionResult> TarjetaFormEdit([FromForm] Tarjeta entidad, string action)
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

                var dataTarjetas = HttpContext.Session.GetString(cacheData);
                var tarjetasResponse = JsonConvert.DeserializeObject<TarjetasResponse>(dataTarjetas);

                entidad = tarjetasResponse.Tarjetas.Find(t => t.Id == entidad.Id);

                var dataEstidades = HttpContext.Session.GetString("dataEntidades");
                EntidadesResponse entidadesResponse;
                if (dataEstidades == null)
                {
                    entidadesResponse = await this.serviceCaller.ObtenerRegistros<EntidadesResponse>(ServicioEnum.Entidades);
                }
                else
                {
                    entidadesResponse = JsonConvert.DeserializeObject<EntidadesResponse>(dataEstidades);
                }

                ViewBag.ModeView = action == "openFormView" ? true : false;
                ViewBag.Tarjeta = entidad;
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
