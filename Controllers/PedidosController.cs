namespace PersonalFinance.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using PersonalFinance.Helper;
    using PersonalFinance.Models;
    using PersonalFinance.Models.Pedidos;
    using PersonalFinance.Service;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Text;

    public class PedidosController : Controller
    {
        private readonly ILogger<PedidosController> _logger;
        private readonly HttpClient _httpClient;
        private readonly PedidosService _pedidoService;

        public PedidosController(ILogger<PedidosController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            HttpClientHandler httpClientHandler = new()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
            };
            _httpClient = new HttpClient(httpClientHandler);
            _pedidoService = new PedidosService(_httpClient);
        }

        [HttpPost]
        public Task<IActionResult> FormEdit([FromForm] Pedido pedido, string action)
        {
            // Procesa los datos del formulario que están en el objeto 'model'
            // Por ejemplo, guarda en una base de datos  

            ViewBag.Action = action;
            
            var dataEstados = HttpContext.Session.GetString("dataEstados");
            var dataEstadosResponse = JsonConvert.DeserializeObject<EstadosResponse>(dataEstados);
            var estados = dataEstadosResponse.Estados.FindAll(x => x.Tabla.Contains("ORDERS") || x.Tabla.Contains("ORDERDETAILS"));

            string dataPedidos = HttpContext.Session.GetString("dataPedidos");
            PedidosResponse dataPedidosResponse = JsonConvert.DeserializeObject<PedidosResponse>(dataPedidos);

            
            switch (action)
            {
                case "add":
                    return Task.FromResult<IActionResult>(View()); // Redirige a otra página


                case "openFormEdit":


                    var dataPedido = dataPedidosResponse.Pedidos.Find(x => x.Id == pedido.Id);

                    ViewBag.Estados = estados;
                    ViewBag.Pedido = dataPedido;

                    return Task.FromResult<IActionResult>(View(ViewBag)); // Redirige a otra página

                default:

                    ViewBag.Estados = estados;
                    //ViewBag.Pedido = dataPedido;

                    return Task.FromResult<IActionResult>(View("Index", dataPedidosResponse.Pedidos));
            }
            
        }

        [HttpPost]
        public Task<IActionResult> FormAdd([FromForm] Pedido pedido, string action)
        {
            // Procesa los datos del formulario que están en el objeto 'model'
            // Por ejemplo, guarda en una base de datos  

            var dataEstados = HttpContext.Session.GetString("dataEstados");
            var dataEstadosResponse = JsonConvert.DeserializeObject<EstadosResponse>(dataEstados);
            var estados = dataEstadosResponse.Estados.FindAll(x => x.Tabla.Contains("ORDERS") || x.Tabla.Contains("ORDERDETAILS"));

            pedido.Estado = dataEstadosResponse.Estados.Find(e => e.Id == 2);

            ViewBag.Pedido = pedido;
            ViewBag.Estados = estados;

            return Task.FromResult<IActionResult>(View(ViewBag)); // Redirige a otra página
        }

        public async Task<IActionResult> Index([FromForm] Pedido pedido, string action, int EstadoSel)
        {
            ViewBag.Message = "Gestión de Pedidos";

            _logger.LogInformation("Inicializando PedidosController => Index()");

            PedidosResponse pedidosResponse = new();

            var dataEstados = HttpContext.Session.GetString("dataEstados");
            var dataEstadosResponse = JsonConvert.DeserializeObject<EstadosResponse>(dataEstados);

            try
            {
                switch (action)
                {
                    case "generar":

                        pedido.Estado = dataEstadosResponse.Estados.Find(e => e.Id == EstadoSel);

                        await this._pedidoService.GenerarPedido(pedido);
                        HttpContext.Session.Remove("dataPedidos");
                        break;

                    case "editOrder":
                        
                        pedido.Estado = dataEstadosResponse.Estados.Find(e => e.Id == EstadoSel);
                        await this._pedidoService.ActualizarPedido(pedido);
                        HttpContext.Session.Remove("dataPedidos");
                        break;
                }

                var dataPedidos = HttpContext.Session.GetString("dataPedidos");

                if (dataPedidos == null)
                {
                    pedidosResponse = await this.ObtenerPedidos();
                }
                else
                {
                    pedidosResponse = JsonConvert.DeserializeObject<PedidosResponse>(dataPedidos);
                }

                    return View(pedidosResponse.Pedidos);

            }
            catch (Exception ex) 
            {
                _logger.LogCritical("Exception PedidosController");
                _logger.LogCritical(ex.ToString());

                return View(new List<Pedido>());
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> FormAddDetail([FromForm] Pedido pedido, PedidoDetalle pedidoDetalle, int EstadoSel, string action)
        {
            // Procesa los datos del formulario que están en el objeto 'model'
            // Por ejemplo, guarda en una base de datos  
            ViewBag.Message = "Gestión Detalle del Pedidos";

            string dataPedidos = dataPedidos = HttpContext.Session.GetString("dataPedidos");
            PedidosResponse dataPedidosResponse = JsonConvert.DeserializeObject<PedidosResponse>(dataPedidos);
            var dataEstados = HttpContext.Session.GetString("dataEstados");
            var dataEstadosResponse = JsonConvert.DeserializeObject<EstadosResponse>(dataEstados);
            var estados = dataEstadosResponse.Estados.FindAll(x => x.Tabla.Contains("ORDERS") || x.Tabla.Contains("ORDERDETAILS"));

            Pedido dataPedido;

            if (action == "generar")
            {
                //
                dataPedido = dataPedidosResponse.Pedidos.Find(x => x.Id == pedidoDetalle.PedidoId);
                pedidoDetalle.Estado = estados.Find(e => e.Id == EstadoSel);

                await this._pedidoService.GenerarDetalle(pedidoDetalle);

                var pedidosResponse = await this.ObtenerPedidos();

                HttpContext.Session.Remove("dataPedidos");
                HttpContext.Session.SetString("dataPedidos", JsonConvert.SerializeObject(pedidosResponse));
            }
            else
            {
                dataPedido = dataPedidosResponse.Pedidos.Find(x => x.Id == pedido.Id);
            }

            ViewBag.Pedido = dataPedido;
            ViewBag.Estados = estados;
            return View(ViewBag); // Redirige a otra página
        }

        [HttpPost]
        public async Task<IActionResult> FormEditDetail([FromForm] PedidoDetalle pedidoDetalle, int EstadoSel, int detalleId, int pedidoId, string action)
        {
            // Procesa los datos del formulario que están en el objeto 'model'
            // Por ejemplo, guarda en una base de datos  
            ViewBag.Message = "Gestión Detalle del Pedidos";

            string dataPedidos = dataPedidos = HttpContext.Session.GetString("dataPedidos");
            PedidosResponse dataPedidosResponse = JsonConvert.DeserializeObject<PedidosResponse>(dataPedidos);
            var dataEstados = HttpContext.Session.GetString("dataEstados");
            var dataEstadosResponse = JsonConvert.DeserializeObject<EstadosResponse>(dataEstados);
            var estados = dataEstadosResponse.Estados.FindAll(x => x.Tabla.Contains("ORDERS") || x.Tabla.Contains("ORDERDETAILS"));

            Pedido dataPedido;

            switch (action)
            {
                case "generar":
                    //
                    dataPedido = dataPedidosResponse.Pedidos.Find(x => x.Id == pedidoDetalle.PedidoId);
                    pedidoDetalle.Estado = estados.Find(e => e.Id == EstadoSel);

                    await this._pedidoService.GenerarDetalle(pedidoDetalle);

                    dataPedidosResponse = await this.ObtenerPedidos();

                    HttpContext.Session.Remove("dataPedidos");
                    HttpContext.Session.SetString("dataPedidos", JsonConvert.SerializeObject(dataPedidosResponse));
                    break;

                case "actualizar":
                    
                    pedidoDetalle.Estado = estados.Find(e => e.Id == EstadoSel);

                    await this._pedidoService.ActualizarDetalle(pedidoDetalle);

                    dataPedidosResponse = await this.ObtenerPedidos();
                    HttpContext.Session.Remove("dataPedidos");
                    HttpContext.Session.SetString("dataPedidos", JsonConvert.SerializeObject(dataPedidosResponse));

                    dataPedido = dataPedidosResponse.Pedidos.Find(x => x.Id == pedidoDetalle.PedidoId);

                    var dataDetalles = dataPedido.Detalles.Find(d => d.Id == pedidoDetalle.Id);

                    dataPedido.Detalles.Clear();
                    dataPedido.Detalles.Add(dataDetalles);

                    break;

                case "openFormEditDetail":

                    var dataPedidotmp = dataPedidosResponse.Pedidos.Find(x => x.Id == pedidoId);

                    var detallePedido = dataPedidotmp.Detalles.Find(d => d.Id == detalleId);
                    dataPedidotmp.Detalles.Clear();
                    dataPedidotmp.Detalles.Add(detallePedido);

                    dataPedido = dataPedidotmp;
                    break;

                default:

                    dataPedido = dataPedidosResponse.Pedidos.Find(x => x.Id == pedidoId);
                    break;
            }

            ViewBag.Pedido = dataPedido;
            ViewBag.Estados = estados;
            return View(ViewBag); // Redirige a otra página
        }

        private async Task<PedidosResponse> ObtenerPedidos()
        {
            PedidosResponse pedidosResponse = new();
            pedidosResponse = await _pedidoService.Obtener();

            HttpContext.Session.SetString("dataPedidos", JsonConvert.SerializeObject(pedidosResponse));

            return pedidosResponse;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
