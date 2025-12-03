using PersonalFinance.Models;
using PersonalFinance.Models.Pedidos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;

namespace MiAppWeb.Controllers
{
    public class PedidosController : Controller
    {
        private readonly ILogger<PedidosController> _logger;
        private readonly HttpClient _httpClient;

        public PedidosController(ILogger<PedidosController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Message = "Gestión de Pedidos";

            var apiUrl = "https://localhost/api/v1/orders/getall";

            // Hacer la solicitud GET a la API
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

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
                return View("Error");
            }

            //return View();
        }

        private async Task<List<PedidoDetalle>> ObtenerDetalles(int idPedido)
        {

            var apiUrl = $"https://localhost/api/v1/ordersdetails/get/{idPedido}";

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
