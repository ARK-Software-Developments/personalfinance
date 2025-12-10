namespace PersonalFinance.Controllers;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PersonalFinance.Models;
using PersonalFinance.Models.Categorias;
using PersonalFinance.Models.Entidades;
using PersonalFinance.Models.Pedidos;
using PersonalFinance.Service;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

public class CategoriasController : Controller
{
    private readonly string Modulo = "Categorias";
    private readonly string cacheData = "dataCategorias";
    private readonly ILogger<CategoriasController> _logger;
    private readonly HttpClient _httpClient;
    private readonly CategoriasService _service;

    public CategoriasController(ILogger<CategoriasController> logger)
    {
        _logger = logger;
        HttpClientHandler httpClientHandler = new()
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
        };
        _httpClient = new HttpClient(httpClientHandler);
        _service = new CategoriasService(_httpClient);
    }

    public async Task<IActionResult> Index([FromForm] Categoria entidad, string action)
    {
        _logger.LogInformation("Inicializando CategoriasController => Index()");
        CategoriasResponse response = new();
        ViewBag.Modulo = Modulo;
        ViewBag.Message = "Gestión de Categorias";

        try
        {
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

            return View(ViewBag.Categorias);

        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.ToString());

            return View(new List<Entidad>());
        }
    }

    private async Task<CategoriasResponse> Obtener()
    {
        CategoriasResponse response = new();
        response = await _service.Obtener();

        HttpContext.Session.SetString(cacheData, JsonConvert.SerializeObject(response));

        return response;
    }

    [HttpPost]
    public Task<IActionResult> FormAdd([FromForm] Categoria entidad, string action)
    {
        // Procesa los datos del formulario que están en el objeto 'model'
        // Por ejemplo, guarda en una base de datos  
        ViewBag.Modulo = Modulo;
        ViewBag.Categoria = entidad;

        return Task.FromResult<IActionResult>(View(ViewBag)); // Redirige a otra página
    }

    [HttpPost]
    public Task<IActionResult> FormEdit([FromForm] Categoria entidad, string action)
    {
        // Procesa los datos del formulario que están en el objeto 'model'
        // Por ejemplo, guarda en una base de datos  
        ViewBag.Modulo = Modulo;
        ViewBag.Action = action;

        switch (action)
        {
            case "add":
                return Task.FromResult<IActionResult>(View()); // Redirige a otra página


            case "openFormEdit":

                ViewBag.Categoria = entidad;

                return Task.FromResult<IActionResult>(View(ViewBag)); // Redirige a otra página

            default:

                return Task.FromResult<IActionResult>(View("Index", entidad));
        }

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
