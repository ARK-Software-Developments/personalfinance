namespace PersonalFinance.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using PersonalFinance.Models;
    using PersonalFinance.Models.Pedidos;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Text;

    public class PedidosController : Controller
    {
        private readonly ILogger<PedidosController> _logger;
        private readonly HttpClient _httpClient;
        private readonly string _urlPedido = "https://localhost:443/api/v1/orders/getall";
        private readonly string _urlPedidoDetalles = "https://localhost:443/api/v1/ordersdetails/getorderid/";
        private readonly string _urlCreate = "https://localhost:443/api/v1/orders/create";

        public PedidosController(ILogger<PedidosController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            HttpClientHandler httpClientHandler = new()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
            };
            _httpClient = new HttpClient(httpClientHandler);
        }

        [HttpPost]
        public Task<IActionResult> FormEdit([FromForm] Pedido pedido, string action)
        {
            // Procesa los datos del formulario que están en el objeto 'model'
            // Por ejemplo, guarda en una base de datos  

            ViewBag.Action = action;

            if (action == "add")
            {                
                return Task.FromResult<IActionResult>(View()); // Redirige a otra página
            }
            else
            {
                var dataPedidos = HttpContext.Session.GetString("dataPedidos");
                var dataPedidosResponse = JsonConvert.DeserializeObject<PedidosResponse>(dataPedidos);

                var dataPedido = dataPedidosResponse.Pedidos.Find(x => x.Id == pedido.Id);

                ViewBag.Pedido = dataPedido;
                return Task.FromResult<IActionResult>(View(ViewBag)); // Redirige a otra página
            }
            
        }

        [HttpPost]
        public Task<IActionResult> FormAdd([FromForm] Pedido pedido, string action)
        {
            // Procesa los datos del formulario que están en el objeto 'model'
            // Por ejemplo, guarda en una base de datos  

            ViewBag.Pedido = pedido;

            return Task.FromResult<IActionResult>(View(ViewBag)); // Redirige a otra página
        }

        public async Task<IActionResult> Index([FromForm] Pedido pedido, string action)
        {
            ViewBag.Message = "Gestión de Pedidos";

            _logger.LogInformation("Inicializando PedidosController => Index()");

            PedidosResponse pedidosResponse = new();

            try
            {
                switch (action)
                {
                    case "add":
                        await this.Generar(pedido);
                        pedidosResponse = await this.ObtenerPedidos();
                        // Pasar los datos a la vista
                        return View(pedidosResponse.Pedidos);
                        
                    default:
                        pedidosResponse = await this.ObtenerPedidos();
                        // Pasar los datos a la vista
                        return View(pedidosResponse.Pedidos);
                }
                
            }
            catch (Exception ex) 
            {
                _logger.LogCritical("Exception PedidosController");
                _logger.LogCritical(_urlPedido);
                _logger.LogCritical(ex.ToString());

                return View(new List<Pedido>());
            }
            
        }

        private async Task Generar(Pedido pedido)
        {
            PedidosResponse pedidosResponse = new();

            var jsonContent = JsonConvert.SerializeObject(pedido);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            // Hacer la solicitud GET a la API
            HttpResponseMessage response = await _httpClient.PutAsync(_urlCreate, content);

            // Ensure the request was successful
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                // Leer el contenido de la respuesta como una cadena JSON
                var jsonResponse = await response.Content.ReadAsStringAsync();

                // Deserializar la cadena JSON a un objeto o lista de objetos
                pedidosResponse = JsonConvert.DeserializeObject<PedidosResponse>(jsonResponse);

            }
        }

        private async Task<PedidosResponse> ObtenerPedidos()
        {
            PedidosResponse pedidosResponse = new ();

            // Hacer la solicitud GET a la API
            HttpResponseMessage response = await _httpClient.GetAsync(_urlPedido);

            // Ensure the request was successful
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                // Leer el contenido de la respuesta como una cadena JSON
                var jsonResponse = await response.Content.ReadAsStringAsync();

                // Deserializar la cadena JSON a un objeto o lista de objetos
                pedidosResponse = JsonConvert.DeserializeObject<PedidosResponse>(jsonResponse);

                var pedidos = new List<Pedido>();

                foreach (var pedido in pedidosResponse.Pedidos)
                {
                    pedido.Detalles = await ObtenerDetalles(pedido.Id);
                    pedidos.Add(pedido);
                }

                HttpContext.Session.SetString("dataPedidos", JsonConvert.SerializeObject(pedidosResponse));

                return pedidosResponse;
            }
            else
            {
                // Manejar el error si la respuesta no fue exitosa
                return pedidosResponse;
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
