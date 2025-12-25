namespace PersonalFinance.Controllers;

using Microsoft.AspNetCore.Mvc;
using PersonalFinance.Models;
using PersonalFinance.Models.Categorias;
using PersonalFinance.Models.Entidades;
using PersonalFinance.Models.Enums;
using PersonalFinance.Service;
using System.Diagnostics;
using System.Net.Http;

public class CategoriasController : BaseController
{
    private readonly string Modulo = "Categorias";
    private readonly ILogger<CategoriasController> _logger;
    
    public CategoriasController(ILogger<CategoriasController> logger)
    {
        _logger = logger;
        this.httpClientHandler = new()
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
        };
    }

    public async Task<IActionResult> Index([FromForm] Categoria entidad, string action)
    {
        _logger.LogInformation("Inicializando CategoriasController => Index()");
        
        this.Inicialized();

        ViewBag.Modulo = Modulo;
        ViewBag.Message = "Gestión de Categorias";

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
                     Nombre = "pCategoria",
                     Valor = entidad.Nombre,
                 },
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

                    await this.serviceCaller.ActualizarRegistro<GeneralDataResponse>(ServicioEnum.Categorias, generalRequest);
                }
                else
                {
                    await this.serviceCaller.GenerarRegistro<GeneralDataResponse>(ServicioEnum.Categorias, generalRequest);
                }
            }

            this.categoriasResponse = await this.serviceCaller.ObtenerRegistros<CategoriasResponse>(ServicioEnum.Categorias);

            ViewBag.Categorias = this.categoriasResponse.Categoria;

            return await Task.FromResult<IActionResult>(View(ViewBag.Categorias));

        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.ToString());

            return await Task.FromResult<IActionResult>(View(new List<Categoria>()));
        }
    }

    [HttpPost]
    public async Task<IActionResult> FormAdd([FromForm] Categoria entidad, string action)
    {
        this.Inicialized();

        // Procesa los datos del formulario que están en el objeto 'model'
        // Por ejemplo, guarda en una base de datos  
        ViewBag.Modulo = Modulo;
        ViewBag.Categoria = entidad;

        return await Task.FromResult<IActionResult>(View(ViewBag)); // Redirige a otra página
    }

    [HttpPost]
    public async Task<IActionResult> FormEdit([FromForm] Categoria entidad, string action)
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


            case "openFormEdit":

                ViewBag.Categoria = entidad;

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
