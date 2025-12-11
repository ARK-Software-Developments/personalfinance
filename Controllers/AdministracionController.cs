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

public class AdministracionController : Controller
{
    private readonly string Gestion = "Administrar";
    private readonly string Modulo = "Administracion";
    private readonly string cacheData = "dataAdministracion";
    private readonly ILogger<AdministracionController> _logger;
    private readonly HttpClient _httpClient;
    private readonly CategoriasService _service;

    public AdministracionController(ILogger<AdministracionController> logger)
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
        _logger.LogInformation("Inicializando AdministracionController => Index()");
        CategoriasResponse response = new();
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

            return View(new List<Entidad>());
        }
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
