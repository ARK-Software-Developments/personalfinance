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

public class AdministracionController : BaseController
{
    private readonly string Gestion = "Administrar";
    private readonly string Modulo = "Administracion";    
    private readonly ILogger<AdministracionController> _logger;

    public AdministracionController(ILogger<AdministracionController> logger)
    {
        _logger = logger;
        HttpClientHandler httpClientHandler = new()
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
        };
    }

    public async Task<IActionResult> Index([FromForm] Categoria entidad, string action)
    {
        _logger.LogInformation("Inicializando AdministracionController => Index()");        

        ViewBag.Modulo = Modulo;
        ViewBag.Title = $"{Gestion}";
        ViewBag.Message = $"Gestión de {Modulo}";

        try
        {            
            return await Task.FromResult<IActionResult>(View());
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.ToString());

            return await Task.FromResult<IActionResult>(View());
        }
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
