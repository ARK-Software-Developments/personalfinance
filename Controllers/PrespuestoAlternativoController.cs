namespace PersonalFinance.Controllers;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PersonalFinance.Helper;
using PersonalFinance.Models;
using PersonalFinance.Models.Balance;
using PersonalFinance.Models.Enums;
using PersonalFinance.Models.Gastos;
using PersonalFinance.Models.Notificaciones;
using PersonalFinance.Models.Pedidos;
using PersonalFinance.Service;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

public class PrespuestoAlternativoController : BaseController
{
    private readonly ILogger<PrespuestoAlternativoController> _logger;
    private readonly HttpClient _httpClient;
    private readonly string _urlEstados = "https://localhost:443/api/v1/status/getall";

    public PrespuestoAlternativoController(ILogger<PrespuestoAlternativoController> logger)
    {
        _logger = logger;
        this.httpClientHandler = new()
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
        };
        _httpClient = new HttpClient(httpClientHandler);
    }

    public async Task<IActionResult> Index([FromForm] Balance balance, string action)
    {
        this.Inicialized();

        int year = Utils.GetYear(HttpContext);


        //await this.CargarPresupuestoAlternativo(year);

        //ViewBag.PresupuestoAlternativo = presupuestoAlternativo;

        return await Task.FromResult<IActionResult>(View("Index", ViewBag));
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
