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
    private readonly ILogger<EntidadesController> _logger;
    private readonly HttpClient _httpClient;
    private readonly EntidadesService _entidadesService;

    public EntidadesController(ILogger<EntidadesController> logger)
    {
        _logger = logger;
        HttpClientHandler httpClientHandler = new()
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
        };
        _httpClient = new HttpClient(httpClientHandler);
        _entidadesService = new EntidadesService(_httpClient);
    }

    public async Task<IActionResult> Index([FromForm] Entidad entidad, string action)
    {
        _logger.LogInformation("Inicializando EntidadesController => Index()");
        EntidadesResponse response = new();
        ViewBag.Message = "Gestión de Entidades";

        response = await this.ObtenerEntidades();
        ViewBag.Entidades = response.Entidades;
        

        return View(ViewBag.Entidades);
    }

    private async Task<EntidadesResponse> ObtenerEntidades()
    {
        EntidadesResponse response = new();
        response = await _entidadesService.Obtener();

        HttpContext.Session.SetString("dataEntidades", JsonConvert.SerializeObject(response));

        return response;
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
