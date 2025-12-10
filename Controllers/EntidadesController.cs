namespace PersonalFinance.Controllers;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PersonalFinance.Models;
using PersonalFinance.Models.Entidades;
using PersonalFinance.Models.Pedidos;
using PersonalFinance.Service;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

public class EntidadesController : Controller
{
    private readonly string cacheData = "dataEntidades";
    private readonly ILogger<EntidadesController> _logger;
    private readonly HttpClient _httpClient;
    private readonly EntidadesService _service;

    public EntidadesController(ILogger<EntidadesController> logger)
    {
        _logger = logger;
        HttpClientHandler httpClientHandler = new()
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
        };
        _httpClient = new HttpClient(httpClientHandler);
        _service = new EntidadesService(_httpClient);
    }

    public async Task<IActionResult> Index([FromForm] Entidad entidad, string action)
    {
        _logger.LogInformation("Inicializando EntidadesController => Index()");
        EntidadesResponse response = new();
        ViewBag.Message = "Gestión de Entidades";

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
                response = JsonConvert.DeserializeObject<EntidadesResponse>(dataPedidos);
            }

            ViewBag.Entidades = response.Entidades;

            return View(ViewBag.Entidades);

        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.ToString());

            return View(new List<Entidad>());
        }
    }

    private async Task<EntidadesResponse> Obtener()
    {
        EntidadesResponse response = new();
        response = await _service.Obtener();

        HttpContext.Session.SetString(cacheData, JsonConvert.SerializeObject(response));

        return response;
    }

    [HttpPost]
    public Task<IActionResult> FormAdd([FromForm] Entidad entidad, string action)
    {
        // Procesa los datos del formulario que están en el objeto 'model'
        // Por ejemplo, guarda en una base de datos  

        ViewBag.Entidad = entidad;

        return Task.FromResult<IActionResult>(View(ViewBag)); // Redirige a otra página
    }

    [HttpPost]
    public Task<IActionResult> FormEdit([FromForm] Entidad entidad, string action)
    {
        // Procesa los datos del formulario que están en el objeto 'model'
        // Por ejemplo, guarda en una base de datos  

        ViewBag.Action = action;

        switch (action)
        {
            case "add":
                return Task.FromResult<IActionResult>(View()); // Redirige a otra página


            case "openFormEdit":

                ViewBag.Entidad = entidad;

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
