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

public class BalanceController : BaseController
{
    private readonly ILogger<BalanceController> _logger;
    private readonly HttpClient _httpClient;
    private readonly string _urlEstados = "https://localhost:443/api/v1/status/getall";

    public BalanceController(ILogger<BalanceController> logger)
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

        if(!string.IsNullOrEmpty(action) && action == "asentar")
        {
            this.generalRequest = new GeneralRequest()
            {
                Parametros = [
                    new Parametro()
                    {
                        Nombre = "pYear",
                        Valor = int.Parse(this.Request.Form["Ano"].ToString()),
                    },
                    new Parametro()
                    {
                        Nombre = "pMonth",
                        Valor = int.Parse(this.Request.Form["Mes"].ToString()),
                    }
                    ]
            };
            
            await this.serviceCaller.EjecutarProceso<GeneralDataResponse>(ServicioEnum.Procesos, this.generalRequest, MetodoEnum.IniciarBalanceMensual);
        }

        await this.CargarBalance(year);

        List<Balance> balances = this.balanceResponse.Balances
                        .FindAll(b => b.Concepto == "BALANCE" || b.Concepto == "INGRESO" || b.Concepto == "PRESUPUESTO")
                        .OrderByDescending(o => o.Concepto)
                        .ToList();

        ViewBag.Balances = balances;

        var labels = new[] { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
        List<int> valsBalances = [];
        List<int> valsPresupuestos = [];
        List<int> valsIngresos = [];

        foreach (var item in balances)
        {
            switch (item.Concepto)            
            {
                case "BALANCE":
                    valsBalances.Add((int)item.Enero);
                    valsBalances.Add((int)item.Febrero);
                    valsBalances.Add((int)item.Marzo);
                    valsBalances.Add((int)item.Abril);
                    valsBalances.Add((int)item.Mayo);
                    valsBalances.Add((int)item.Junio);
                    valsBalances.Add((int)item.Julio);
                    valsBalances.Add((int)item.Agosto);
                    valsBalances.Add((int)item.Septiembre);
                    valsBalances.Add((int)item.Octubre);
                    valsBalances.Add((int)item.Noviembre);
                    valsBalances.Add((int)item.Diciembre);
                    break;

                case "INGRESO":
                    valsIngresos.Add((int)item.Enero);
                    valsIngresos.Add((int)item.Febrero);
                    valsIngresos.Add((int)item.Marzo);
                    valsIngresos.Add((int)item.Abril);
                    valsIngresos.Add((int)item.Mayo);
                    valsIngresos.Add((int)item.Junio);
                    valsIngresos.Add((int)item.Julio);
                    valsIngresos.Add((int)item.Agosto);
                    valsIngresos.Add((int)item.Septiembre);
                    valsIngresos.Add((int)item.Octubre);
                    valsIngresos.Add((int)item.Noviembre);
                    valsIngresos.Add((int)item.Diciembre);
                    break;

                case "PRESUPUESTO":
                    valsPresupuestos.Add((int)item.Enero);
                    valsPresupuestos.Add((int)item.Febrero);
                    valsPresupuestos.Add((int)item.Marzo);
                    valsPresupuestos.Add((int)item.Abril);
                    valsPresupuestos.Add((int)item.Mayo);
                    valsPresupuestos.Add((int)item.Junio);
                    valsPresupuestos.Add((int)item.Julio);
                    valsPresupuestos.Add((int)item.Agosto);
                    valsPresupuestos.Add((int)item.Septiembre);
                    valsPresupuestos.Add((int)item.Octubre);
                    valsPresupuestos.Add((int)item.Noviembre);
                    valsPresupuestos.Add((int)item.Diciembre);
                    break;

            }
        }
        ViewBag.BalancesLabels = labels;
        ViewBag.BalancesVals = valsBalances;
        ViewBag.PresupuestosVals = valsPresupuestos;
        ViewBag.IngresosVals = valsIngresos;

        return await Task.FromResult<IActionResult>(View("Index", ViewBag));
    }

    private async Task CargarBalance(int year)
    {
        // Hacer la solicitud GET a la API
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


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
