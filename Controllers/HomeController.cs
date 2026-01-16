namespace PersonalFinance.Controllers;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PersonalFinance.Helper;
using PersonalFinance.Models;
using PersonalFinance.Models.Balance;
using PersonalFinance.Models.Categorias;
using PersonalFinance.Models.Entidades;
using PersonalFinance.Models.Enums;
using PersonalFinance.Models.Gastos;
using PersonalFinance.Models.Notificaciones;
using PersonalFinance.Models.Pedidos;
using PersonalFinance.Models.TarjetaConsumos;
using PersonalFinance.Service;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;

public class HomeController : BaseController
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

        var ano = this.HttpContext.Request.Query["area"];
        ano = string.IsNullOrEmpty(ano) ? DateTime.Now.Year.ToString() : ano;
        int year = Utils.GetYear(HttpContext, int.Parse(ano));
        ViewBag.Year = year;


        await this.CargarNotificaciones();
        ViewBag.Notificaciones = JsonConvert.SerializeObject(this.notificacionResponse.Notificaciones);

        await this.CargarEstados();

        await this.CargarCalendariosVencimientos();

        await this.CargarBalance();

        var balance = this.balanceResponse.Balances;

        var mesActual = DateTime.Now.Month;

        var difMeses = Utils.CalcularDiferenciasMeses(mesActual, Utils.CargarMeses<Balance>(balance.Find(b => b.Concepto == "PRESUPUESTO")));
        decimal presupuestoAnterior = 2826819.19m;
        decimal presupuestoActual = Utils.ConvertirMonto(difMeses["actual"].ToString());
        decimal variacion = (presupuestoActual - presupuestoAnterior) / presupuestoAnterior * 100;
        ViewBag.PresupuestoAnterior = presupuestoAnterior;
        ViewBag.Presupuesto = presupuestoActual;
        ViewBag.PresupuestoV = variacion < 0 ? $"{variacion.ToString("F2")}" : $"{variacion.ToString("F2")}";


        difMeses = Utils.CalcularDiferenciasMeses(mesActual, Utils.CargarMeses<Balance>(balance.Find(b => b.Concepto == "INGRESO")));
        decimal ingresosAnterior = 2086174.60m;
        decimal ingresoActual = Utils.ConvertirMonto(difMeses["actual"].ToString());
        variacion = (ingresoActual - ingresosAnterior) / ingresosAnterior * 100;
        ViewBag.IngresosAnterior = ingresosAnterior;
        ViewBag.Ingresos = ingresoActual;
        ViewBag.IngresosV = variacion < 0 ? $"{variacion.ToString("F2")}" : $"{variacion.ToString("F2")}";


        difMeses = Utils.CalcularDiferenciasMeses(mesActual, Utils.CargarMeses<Balance>(balance?.Find(b => b.Concepto == "EGRESO")));
        decimal gastosAnterior = 599500.00m;
        decimal gastosActual = Utils.ConvertirMonto(difMeses["actual"].ToString());
        variacion = (gastosActual - gastosAnterior) / gastosAnterior * 100;
        ViewBag.GastosAnterior = gastosAnterior;
        ViewBag.Gastos = gastosActual;
        ViewBag.GastosV = variacion < 0 ? $"{variacion.ToString("F2")}" : $"{variacion.ToString("F2")}";


        var balanceActual = ingresoActual - presupuestoActual;
        ViewBag.BalanceActual = balanceActual;

        ViewBag.PendientePago = presupuestoActual - gastosActual;

        await this.CargarGastosMensuales();

        var g = this.gastosResponse?.Gastos?.FindAll(x => x.Pagado == false);

        foreach (var i in g)
        {
            i.Vencimientos = [];
            i.Vencimientos = this.calendarioVencimientoResponse?.CalendarioVencimientos?.FindAll(c => c.TipoGastoId == i.TipoGasto.Id);
        }

        ViewBag.GastosMensualesPendientes = g
            .OrderBy(o => o.Vencimientos != null && o.Vencimientos.Any()
                ? o.Vencimientos.Min(v => v.FechaVencimiento.GetValueOrDefault(DateTime.MaxValue))
                : DateTime.MaxValue)
            .ToList();

        return await Task.FromResult<IActionResult>(View("Index", ViewBag));
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

    private async Task CargarBalance()
    {
        // Hacer la solicitud GET a la API
        this.keyValuePairs.Add("year", Utils.GetYear(HttpContext));
        HttpResponseMessage response = await this._httpClient.GetAsync(Microservicios.get(ServicioEnum.Balance, MetodoEnum.Todos, keyValuePairs));

        // Ensure the request was successful
        response.EnsureSuccessStatusCode();

        if (response.IsSuccessStatusCode)
        {
            // Leer el contenido de la respuesta como una cadena JSON
            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Deserializar la cadena JSON a un objeto o lista de objetos
            this.balanceResponse = JsonConvert.DeserializeObject<BalanceResponse>(jsonResponse);
        }
    }

    private async Task CargarNotificaciones()
    {
        // Hacer la solicitud GET a la API
        HttpResponseMessage response = await this._httpClient.GetAsync(Microservicios.get(ServicioEnum.Notificacion, MetodoEnum.Todos));

        // Ensure the request was successful
        response.EnsureSuccessStatusCode();

        if (response.IsSuccessStatusCode)
        {
            // Leer el contenido de la respuesta como una cadena JSON
            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Deserializar la cadena JSON a un objeto o lista de objetos
            this.notificacionResponse = JsonConvert.DeserializeObject<NotificacionesResponse>(jsonResponse);
        }
    }

    private async Task CargarGastosMensuales()
    {
        // Hacer la solicitud GET a la API
        this.keyValuePairs.Clear();
        this.keyValuePairs.Add("year", Utils.GetYear(HttpContext));
        HttpResponseMessage response = await this._httpClient.GetAsync(Microservicios.get(ServicioEnum.GastosMensuales, MetodoEnum.Todos, this.keyValuePairs));

        // Ensure the request was successful
        response.EnsureSuccessStatusCode();

        if (response.IsSuccessStatusCode)
        {
            // Leer el contenido de la respuesta como una cadena JSON
            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Deserializar la cadena JSON a un objeto o lista de objetos
            this.gastosResponse = JsonConvert.DeserializeObject<GastosResponse>(jsonResponse);
        }
    }

    private async Task CargarCalendariosVencimientos()
    {
        // Hacer la solicitud GET a la API
        HttpResponseMessage response = await this._httpClient.GetAsync(Microservicios.get(ServicioEnum.CalendariosVencimientos, MetodoEnum.Todos));

        // Ensure the request was successful
        response.EnsureSuccessStatusCode();

        if (response.IsSuccessStatusCode)
        {
            // Leer el contenido de la respuesta como una cadena JSON
            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Deserializar la cadena JSON a un objeto o lista de objetos
            this.calendarioVencimientoResponse = JsonConvert.DeserializeObject<CalendarioVencimientoResponse>(jsonResponse);
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
