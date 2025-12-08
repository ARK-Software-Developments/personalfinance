namespace PersonalFinance.Controllers;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PersonalFinance.Models;
using PersonalFinance.Models.Pedidos;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly HttpClient _httpClient;
    private readonly string _urlEstados = "https://localhost:443/api/v1/status/getall";

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        HttpClientHandler httpClientHandler = new()
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
        };
        _httpClient = new HttpClient(httpClientHandler);
    }

    public async Task<IActionResult> Index()
    {
        _logger.LogInformation("Inicializando HomeController => Index()");

        await this.CargarEstados();

        return View();
    }

    private async Task CargarEstados()
    {
        // Hacer la solicitud GET a la API
        HttpResponseMessage response = await _httpClient.GetAsync(_urlEstados);

        // Ensure the request was successful
        response.EnsureSuccessStatusCode();

        if (response.IsSuccessStatusCode)
        {
            // Leer el contenido de la respuesta como una cadena JSON
            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Deserializar la cadena JSON a un objeto o lista de objetos
            var estadosResponse = JsonConvert.DeserializeObject<EstadosResponse>(jsonResponse);


            HttpContext.Session.SetString("dataEstados", JsonConvert.SerializeObject(estadosResponse));
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
