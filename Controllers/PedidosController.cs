namespace PersonalFinance.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using PersonalFinance.Helper;
    using PersonalFinance.Models;
    using PersonalFinance.Models.Enums;
    using PersonalFinance.Models.Pedidos;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net.Http;

    public class PedidosController : BaseController
    {
        private readonly string Gestion = "Administrar";
        private readonly string Modulo = "Pedidos";        
        private readonly ILogger<PedidosController> _logger;

        public PedidosController(ILogger<PedidosController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            this.httpClientHandler = new()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
            };            
        }

        [HttpPost]
        public async Task<IActionResult> FormEdit([FromForm] Pedido pedido, string action)
        {
            this.Inicialized();

            // Procesa los datos del formulario que están en el objeto 'model'
            // Por ejemplo, guarda en una base de datos  

            ViewBag.Action = action;
            ViewBag.Modulo = Modulo;
            ViewBag.Title = $"Formulario de {Modulo}";

            estadosResponse = await this.serviceCaller.ObtenerRegistros<EstadosResponse>(ServicioEnum.Estados);

            var estados = estadosResponse.Estados.FindAll(x => x.Tabla.Contains("ORDERS") || x.Tabla.Contains("ORDERDETAILS"));

            await CargarPedidos();

            switch (action)
            {
                case "add":
                    return await Task.FromResult<IActionResult>(View()); // Redirige a otra página


                case "openFormEdit":


                    var dataPedido = pedidosResponse.Pedidos.Find(x => x.Id == pedido.Id);

                    ViewBag.Estados = estados;
                    ViewBag.Pedido = dataPedido;

                    return await Task.FromResult<IActionResult>(View(ViewBag)); // Redirige a otra página

                default:

                    ViewBag.Estados = estados;
                    //ViewBag.Pedido = dataPedido;

                    return await Task.FromResult<IActionResult>(View("Index", pedidosResponse.Pedidos));
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> FormAdd([FromForm] Pedido pedido, string action)
        {
            this.Inicialized();

            // Procesa los datos del formulario que están en el objeto 'model'
            // Por ejemplo, guarda en una base de datos  

            estadosResponse = await this.serviceCaller.ObtenerRegistros<EstadosResponse>(ServicioEnum.Estados);


            var estados = estadosResponse.Estados.FindAll(x => x.Tabla.Contains("ORDERS") || x.Tabla.Contains("ORDERDETAILS"));

            pedido.Estado = estadosResponse.Estados.Find(e => e.Id == 2);

            ViewBag.Modulo = Modulo;
            ViewBag.Title = $"Formulario de {Modulo}";
            ViewBag.Pedido = pedido;
            ViewBag.Estados = estados;

            return await Task.FromResult<IActionResult>(View(ViewBag)); // Redirige a otra página
        }

        public async Task<IActionResult> Index([FromForm] Pedido pedido, string action, int EstadoSel)
        {
            this.Inicialized();

            ViewBag.Message = "Gestión de Pedidos";
            ViewBag.Modulo = Modulo;
            ViewBag.Title = $"Formulario de {Modulo}";
            string montoTotal = string.Empty;
            decimal decena = 0;
            decimal decimales = 0;

            _logger.LogInformation("Inicializando PedidosController => Index()");

            this.estadosResponse = await this.serviceCaller.ObtenerRegistros<EstadosResponse>(ServicioEnum.Estados);

            try
            {
                switch (action)
                {
                    case "generar":

                        pedido.Estado = estadosResponse.Estados.Find(e => e.Id == EstadoSel);

                        montoTotal = pedido.MontoTotal.Replace("$ ", string.Empty);
                        decena = 0;
                        decimales = 0;

                        if (montoTotal.Contains(","))
                        {
                            decena = decimal.Parse(montoTotal.Split(",")[0]);
                            decimales = decimal.Parse(montoTotal.Split(",")[1]);
                        }
                        else
                        {
                            decena = decimal.Parse(montoTotal);
                        }

                        this.generalRequest = new ()
                        {
                            Parametros =
                                [
                                 new Parametro()
                                 {
                                     Nombre = "pNumero",
                                     Valor = pedido.Numero,
                                 },
                                 new Parametro()
                                 {
                                     Nombre = "pFechaPedido",
                                     Valor = pedido.FechaPedido?.ToString("yyyy-MM-dd"),
                                 },
                                 new Parametro()
                                 {
                                     Nombre = "pMontoTotal",
                                     Valor = decimal.Parse($"{decena.ToString()},{decimales.ToString()}"),
                                 },
                                 new Parametro()
                                 {
                                     Nombre = "pTipoRecurso",
                                     Valor = pedido.TipoRecurso,
                                 },
                                 new Parametro()
                                 {
                                     Nombre = "pEstado",
                                     Valor = 2,
                                 }
                             ],
                        };

                        await this.serviceCaller.GenerarRegistro<GeneralDataResponse>(ServicioEnum.Pedidos, generalRequest);
                        CacheAdmin.Remove(HttpContext, ServicioEnum.Pedidos);
                        break;

                    case "editOrder":
                        
                        pedido.Estado = this.estadosResponse.Estados.Find(e => e.Id == EstadoSel);
                        montoTotal = pedido.MontoTotal.Replace("$ ", string.Empty);
                        decena = 0;
                        decimales = 0;

                        if (montoTotal.Contains(","))
                        {
                            decena = decimal.Parse(montoTotal.Split(",")[0]);
                            decimales = decimal.Parse(montoTotal.Split(",")[1]);
                        }
                        else
                        {
                            decena = decimal.Parse(montoTotal);
                        }

                        this.generalRequest = new()
                        {
                            Parametros =
                            [
                             new Parametro()
                             {
                                 Nombre = "pId",
                                 Valor = pedido.Id,
                             },
                             new Parametro()
                             {
                                 Nombre = "pFechaRecibido",
                                 Valor = pedido.FechaRecibido?.ToString("yyyy-MM-dd"),
                             },
                             new Parametro()
                             {
                                 Nombre = "pMontoTotal",
                                 Valor = decimal.Parse($"{decena.ToString()},{decimales.ToString()}"),
                             },
                             new Parametro()
                             {
                                 Nombre = "pEstado",
                                 Valor = pedido.Estado.Id,
                             }
                            ],
                        };

                        await this.serviceCaller.ActualizarRegistro<GeneralDataResponse>(ServicioEnum.Pedidos, generalRequest);
                        CacheAdmin.Remove(HttpContext, ServicioEnum.Pedidos);
                        break;
                }

                await CargarPedidos();

                //return View(this.pedidosResponse.Pedidos);

                return await Task.FromResult<IActionResult>(View("Index",this.pedidosResponse.Pedidos)); // Redirige a otra página
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
            this.Inicialized();

            // Procesa los datos del formulario que están en el objeto 'model'
            // Por ejemplo, guarda en una base de datos  
            ViewBag.Message = "Gestión Detalle del Pedidos";
            ViewBag.Modulo = Modulo;
            ViewBag.Title = $"Formulario Detalle de {Modulo}";

            await CargarPedidos();

            this.estadosResponse = await this.serviceCaller.ObtenerRegistros<EstadosResponse>(ServicioEnum.Estados);

            var estados = estadosResponse.Estados.FindAll(x => x.Tabla.Contains("ORDERS") || x.Tabla.Contains("ORDERDETAILS"));

            Pedido dataPedido;

            if (action == "generar")
            {
                //
                dataPedido = this.pedidosResponse.Pedidos.Find(x => x.Id == pedidoDetalle.PedidoId);
                pedidoDetalle.Estado = estados.Find(e => e.Id == EstadoSel);

                this.generalRequest = new ()
                {
                    Parametros =
                        [
                         new Parametro()
                         {
                             Nombre = "pOrderId",
                             Valor = pedidoDetalle.PedidoId,
                         },
                         new Parametro()
                         {
                             Nombre = "pBrand",
                             Valor = pedidoDetalle.Marca,
                         },
                         new Parametro()
                         {
                             Nombre = "pProductDetails",
                             Valor = pedidoDetalle.ProductoDetalle,
                         },
                         new Parametro()
                         {
                             Nombre = "pDescription",
                             Valor = pedidoDetalle.Descripcion,
                         },
                         new Parametro()
                         {
                             Nombre = "pProductCode",
                             Valor = pedidoDetalle.CodigoProducto,
                         },
                         new Parametro()
                         {
                             Nombre = "pQuantity",
                             Valor = pedidoDetalle.Cantidad,
                         },
                         new Parametro()
                         {
                             Nombre = "pUnitPrice",
                             Valor = Utils.ConvertirMonto(pedidoDetalle.MontoUnitario),
                         },
                         new Parametro()
                         {
                             Nombre = "pSubTotal",
                             Valor = Utils.ConvertirMonto(pedidoDetalle.Subtotal),
                         },
                         new Parametro()
                         {
                             Nombre = "pTo",
                             Valor = pedidoDetalle.Para,
                         },
                         new Parametro()
                         {
                             Nombre = "pStatus",
                             Valor = pedidoDetalle.Estado.Id,
                         }
                     ],
                };

                await this.serviceCaller.GenerarRegistro<GeneralDataResponse>(ServicioEnum.DetallePedido, generalRequest);

                CacheAdmin.Remove(httpContext, ServicioEnum.Pedidos);
                await CargarPedidos();

                dataPedido = pedidosResponse.Pedidos.Find(x => x.Id == pedidoDetalle.PedidoId);

                ViewBag.Pedido = dataPedido;
                ViewBag.Estados = estados;

                return await Task.FromResult<IActionResult>(View("FormEdit", ViewBag)); // Redirige a otra página
            }
            else
            {
                dataPedido = pedidosResponse.Pedidos.Find(x => x.Id == pedido.Id);
            }

            ViewBag.Pedido = dataPedido;
            ViewBag.Estados = estados;
            return await Task.FromResult<IActionResult>(View(ViewBag)); // Redirige a otra página
        }

        [HttpPost]
        public async Task<IActionResult> FormEditDetail([FromForm] PedidoDetalle pedidoDetalle, int EstadoSel, int detalleId, int pedidoId, string action)
        {
            this.Inicialized();

            // Procesa los datos del formulario que están en el objeto 'model'
            // Por ejemplo, guarda en una base de datos  
            ViewBag.Message = "Gestión Detalle del Pedidos";
            ViewBag.Modulo = Modulo;
            ViewBag.Title = $"Formulario Detalle de {Modulo}";

            await CargarPedidos();

            this.estadosResponse = await this.serviceCaller.ObtenerRegistros<EstadosResponse>(ServicioEnum.Estados);
            var estados = this.estadosResponse.Estados.FindAll(x => x.Tabla.Contains("ORDERS") || x.Tabla.Contains("ORDERDETAILS"));

            Pedido dataPedido;

            switch (action)
            {
                case "actualizar":
                    
                    pedidoDetalle.Estado = estados.Find(e => e.Id == EstadoSel);

                    this.generalRequest = new()
                    {
                        Parametros =
                            [
                             new Parametro()
                             {
                                 Nombre = "pId",
                                 Valor = pedidoDetalle.Id,
                             },
                             new Parametro()
                                {
                                 Nombre = "pOrderId",
                                 Valor = pedidoDetalle.PedidoId,
                             }, 
                             new Parametro()
                             {
                                 Nombre = "pBrand",
                                 Valor = pedidoDetalle.Marca,
                             },
                             new Parametro()
                             {
                                 Nombre = "pProductDetails",
                                 Valor = pedidoDetalle.ProductoDetalle,
                             },
                             new Parametro()
                             {
                                 Nombre = "pDescription",
                                 Valor = pedidoDetalle.Descripcion,
                             },
                             new Parametro()
                             {
                                 Nombre = "pProductCode",
                                 Valor = pedidoDetalle.CodigoProducto,
                             },
                             new Parametro()
                             {
                                 Nombre = "pQuantity",
                                 Valor = pedidoDetalle.Cantidad,
                             },
                             new Parametro()
                             {
                                 Nombre = "pUnitPrice",
                                 Valor = Utils.ConvertirMonto(pedidoDetalle.MontoUnitario),
                             },
                             new Parametro()
                             {
                                 Nombre = "pSubTotal",
                                 Valor = Utils.ConvertirMonto(pedidoDetalle.Subtotal),
                             },
                             new Parametro()
                             {
                                 Nombre = "pTo",
                                 Valor = pedidoDetalle.Para,
                             },
                             new Parametro()
                             {
                                 Nombre = "pStatus",
                                 Valor = pedidoDetalle.Estado.Id,
                             }
                         ],
                    };

                    await this.serviceCaller.ActualizarRegistro<GeneralDataResponse>(ServicioEnum.DetallePedido, generalRequest);

                    CacheAdmin.Remove(httpContext, ServicioEnum.Pedidos);
                    await CargarPedidos();

                    dataPedido = this.pedidosResponse.Pedidos.Find(x => x.Id == pedidoDetalle.PedidoId);

                    ViewBag.Pedido = dataPedido;
                    ViewBag.Estados = estados;

                    return await Task.FromResult<IActionResult>(View("FormEdit", ViewBag)); // Redirige a otra página

                case "openFormEditDetail":

                    var dataPedidotmp = this.pedidosResponse.Pedidos.Find(x => x.Id == pedidoId);

                    var detallePedido = dataPedidotmp.Detalles.Find(d => d.Id == detalleId);
                    dataPedidotmp.Detalles.Clear();
                    dataPedidotmp.Detalles.Add(detallePedido);

                    dataPedido = dataPedidotmp;
                    break;

                default:

                    dataPedido = this.pedidosResponse.Pedidos.Find(x => x.Id == pedidoId);
                    break;
            }

            ViewBag.Pedido = dataPedido;
            ViewBag.Estados = estados;
            return await Task.FromResult<IActionResult>(View(ViewBag)); // Redirige a otra página
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<PedidosResponse> CargarPedidos()
        {
            this.pedidosResponse = await this.serviceCaller.ObtenerRegistros<PedidosResponse>(ServicioEnum.Pedidos);
            
            List<Pedido> lstPedidos = [];

            foreach (var order in this.pedidosResponse.Pedidos)
            {
                this.keyValuePairs = new Dictionary<string, object>()
                    {
                        {"pId",  order.Id},
                    };

                var detail = await this.serviceCaller.ObtenerRegistros<PedidoDetallesResponse>(ServicioEnum.DetallePedido, this.keyValuePairs, MetodoEnum.DetallePedidoByOrderId);

                order.Detalles = detail.Detalles;

                lstPedidos.Add(order);
            }

            this.pedidosResponse.Pedidos = [];
            this.pedidosResponse.Pedidos = lstPedidos;

            CacheAdmin.Remove(httpContext, ServicioEnum.Pedidos);
            CacheAdmin.Set(httpContext, ServicioEnum.Pedidos, JsonConvert.SerializeObject(this.pedidosResponse));

            return this.pedidosResponse;
        }
    }
}
