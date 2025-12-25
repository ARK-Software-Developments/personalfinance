namespace PersonalFinance.Controllers;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PersonalFinance.Models;
using PersonalFinance.Models.Categorias;
using PersonalFinance.Models.Entidades;
using PersonalFinance.Models.Enums;
using PersonalFinance.Models.TarjetaConsumos;
using PersonalFinance.Service;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

public class EntidadesController : BaseController
{
    private readonly string cacheData = "dataEntidades";
    private readonly ILogger<EntidadesController> _logger;
    
    public EntidadesController(ILogger<EntidadesController> logger)
    {
        _logger = logger;
        this.httpClientHandler = new()
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
        };
    }

    public async Task<IActionResult> Index([FromForm] Entidad entidad, string action)
    {
        this.Inicialized();

        _logger.LogInformation("Inicializando EntidadesController => Index()");
        
        ViewBag.Message = "Gestión de Entidades";

        try
        {
            if (action == "generar" || action == "actualizar")
            {
                GeneralRequest generalRequest = new()
                {
                    Parametros =
                [
                 new Parametro()
                 {
                     Nombre = "pEntidad",
                     Valor = entidad.Nombre,
                 },
                 new Parametro()
                 {
                     Nombre = "pTipo",
                     Valor = entidad.Tipo,
                 }
                ],
                };

                if (action == "actualizar")
                {
                    generalRequest = new()
                    {
                        Parametros =
                            [
                             new Parametro()
                             {
                                 Nombre = "pId",
                                 Valor = entidad.Id,
                             }
                            ],
                    };

                    await this.serviceCaller.ActualizarRegistro<GeneralDataResponse>(ServicioEnum.Entidades, generalRequest);
                }
                else
                {
                    await this.serviceCaller.GenerarRegistro<GeneralDataResponse>(ServicioEnum.Entidades, generalRequest);
                }
            }

            entidadesResponse = await this.serviceCaller.ObtenerRegistros<EntidadesResponse>(ServicioEnum.Entidades);

            ViewBag.Entidades = entidadesResponse.Entidades;

            return await Task.FromResult<IActionResult>(View(ViewBag.Entidades));

        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.ToString());

            return await Task.FromResult<IActionResult>(View(new List<Entidad>()));
        }
    }

    [HttpPost]
    public async Task<IActionResult> FormAdd([FromForm] Entidad entidad, string action)
    {
        this.Inicialized();

        // Procesa los datos del formulario que están en el objeto 'model'
        // Por ejemplo, guarda en una base de datos  

        ViewBag.Entidad = entidad;

        return await Task.FromResult<IActionResult>(View(ViewBag)); // Redirige a otra página
    }

    [HttpPost]
    public async Task<IActionResult> FormEdit([FromForm] Entidad entidad, string action)
    {
        this.Inicialized();

        // Procesa los datos del formulario que están en el objeto 'model'
        // Por ejemplo, guarda en una base de datos  

        ViewBag.Action = action;

        switch (action)
        {
            case "add":
                return await Task.FromResult<IActionResult>(View()); // Redirige a otra página


            case "openFormEdit":

                ViewBag.Entidad = entidad;

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
