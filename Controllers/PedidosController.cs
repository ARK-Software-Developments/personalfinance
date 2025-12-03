using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PersonalFinance.Models;
using PersonalFinance.Models.Pedidos;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;

namespace PersonalFinance.Controllers
{
    public class PedidosController : Controller
    {
        private readonly ILogger<PedidosController> _logger;
        private readonly HttpClient _httpClient;
        private readonly string _urlPedido = "https://localhost:443/api/v1/orders/getall";
        private readonly string _urlPedidoDetalles = "https://localhost:443/api/v1/ordersdetails/get/";

        public PedidosController(ILogger<PedidosController> logger, IHttpClientFactory httpClientFactory)
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
            ViewBag.Message = "Gestión de Pedidos";

            _logger.LogInformation("Inicializando PedidosController => Index()");

            try
            {  
                // Hacer la solicitud GET a la API
                HttpResponseMessage response = await _httpClient.GetAsync(_urlPedido);

                // Ensure the request was successful
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    // Leer el contenido de la respuesta como una cadena JSON
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    // Deserializar la cadena JSON a un objeto o lista de objetos
                    var pedidosResponse = JsonConvert.DeserializeObject<PedidosResponse>(jsonResponse);

                    var pedidos = new List<Pedido>();

                    foreach (var pedido in pedidosResponse.Pedidos)
                    {
                        pedido.Detalles = await ObtenerDetalles(pedido.Id);
                        pedidos.Add(pedido);
                    }

                    // Pasar los datos a la vista
                    return View(pedidosResponse.Pedidos);
                }
                else
                {
                    // Manejar el error si la respuesta no fue exitosa
                    return View(new List<Pedido>());
                }

                //return View();
            }
            catch (Exception ex) 
            {
                _logger.LogCritical("Exception PedidosController");
                _logger.LogCritical(_urlPedido);
                _logger.LogCritical(ex.ToString());

                return View(new List<Pedido>());
            }
            
        }

        private async Task<List<PedidoDetalle>> ObtenerDetalles(int idPedido)
        {
            _logger.LogInformation("Inicializando ObtenerDetalles");
            try
            {
                var apiUrl = $"{_urlPedidoDetalles}{idPedido}";

                // Hacer la solicitud GET a la API
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                // Ensure the request was successful
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    // Leer el contenido de la respuesta como una cadena JSON
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    // Deserializar la cadena JSON a un objeto o lista de objetos
                    var pedidoDetallesResponse = JsonConvert.DeserializeObject<PedidoDetallesResponse>(jsonResponse);

                    // Pasar los datos a la vista
                    return pedidoDetallesResponse?.Detalles;
                }
                else
                {
                    // Manejar el error si la respuesta no fue exitosa
                    return [];
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Exception ObtenerDetalles");
                _logger.LogCritical(_urlPedidoDetalles);
                _logger.LogCritical(ex.ToString());

                return [];
            }
            

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
