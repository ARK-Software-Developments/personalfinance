using Newtonsoft.Json;
using PersonalFinance.Helper;
using PersonalFinance.Models;
using PersonalFinance.Models.Entidades;
using PersonalFinance.Models.Pedidos;
using System.Net.Http;
using System.Text;

namespace PersonalFinance.Service
{
    public class EntidadesService
    {
        private readonly HttpClient _httpClient;
        private readonly string _urlEntidades = "https://localhost:443/api/v1/entities/getall";
        private readonly string _urlPedidoDetalles = "https://localhost:443/api/v1/ordersdetails/getorderid/";
        private readonly string _urlCreate = "https://localhost:443/api/v1/orders/create";
        private readonly string _urlCreateDetail = "https://localhost/api/v1/ordersdetails/create";

        private readonly string _urlUpdate = "https://localhost:443/api/v1/orders/update";
        private readonly string _urlUpdateDetail = "https://localhost/api/v1/ordersdetails/update";

        public EntidadesService(HttpClient httpClient) 
        {
            this._httpClient = httpClient;
        }


        public async Task<EntidadesResponse> Obtener()
        {
            EntidadesResponse entidadesResponse = new();

            // Hacer la solicitud GET a la API
            HttpResponseMessage response = await this._httpClient.GetAsync(_urlEntidades);

            // Ensure the request was successful
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                // Leer el contenido de la respuesta como una cadena JSON
                var jsonResponse = await response.Content.ReadAsStringAsync();

                // Deserializar la cadena JSON a un objeto o lista de objetos
                entidadesResponse = JsonConvert.DeserializeObject<EntidadesResponse>(jsonResponse);

                return entidadesResponse;
            }
            else
            {
                // Manejar el error si la respuesta no fue exitosa
                return entidadesResponse;
            }
        }

        public async Task<List<PedidoDetalle>> ObtenerDetalles(int idPedido)
        {
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
                return [];
            }
        }

        public async Task GenerarPedido(Pedido pedido)
        {
            PedidosResponse pedidosResponse = new();

            var montoTotal = pedido.MontoTotal.Replace("$ ", string.Empty);
            decimal decena = 0;
            decimal decimales = 0;

            if (montoTotal.Contains(","))
            {
                decena = decimal.Parse(montoTotal.Split(",")[0]);
                decimales = decimal.Parse(montoTotal.Split(",")[1]);
            }
            else
            {
                decena = decimal.Parse(montoTotal);
            }

            GeneralRequest generalRequest = new()
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

            var jsonContent = JsonConvert.SerializeObject(generalRequest.Parametros);

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
                //pedidosResponse = JsonConvert.DeserializeObject<PedidosResponse>(jsonResponse);

            }
        }

        public async Task ActualizarPedido(Pedido pedido)
        {
            PedidosResponse pedidosResponse = new();

            var montoTotal = pedido.MontoTotal.Replace("$ ", string.Empty);
            decimal decena = 0;
            decimal decimales = 0;

            if (montoTotal.Contains(","))
            {
                decena = decimal.Parse(montoTotal.Split(",")[0]);
                decimales = decimal.Parse(montoTotal.Split(",")[1]);
            }
            else
            {
                decena = decimal.Parse(montoTotal);
            }

            GeneralRequest generalRequest = new()
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

            var jsonContent = JsonConvert.SerializeObject(generalRequest.Parametros);

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            // Hacer la solicitud GET a la API
            HttpResponseMessage response = await _httpClient.PostAsync(_urlUpdate, content);

            // Ensure the request was successful
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                // Leer el contenido de la respuesta como una cadena JSON
                var jsonResponse = await response.Content.ReadAsStringAsync();

                // Deserializar la cadena JSON a un objeto o lista de objetos
                //pedidosResponse = JsonConvert.DeserializeObject<PedidosResponse>(jsonResponse);

            }
        }

        public async Task GenerarDetalle(PedidoDetalle pedidoDetalle)
        {
            PedidosResponse pedidosResponse = new();

            GeneralRequest generalRequest = new()
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

            var jsonContent = JsonConvert.SerializeObject(generalRequest.Parametros);

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            // Hacer la solicitud GET a la API
            HttpResponseMessage response = await _httpClient.PutAsync(_urlCreateDetail, content);

            // Ensure the request was successful
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                // Leer el contenido de la respuesta como una cadena JSON
                var jsonResponse = await response.Content.ReadAsStringAsync();

                // Deserializar la cadena JSON a un objeto o lista de objetos
                //pedidosResponse = JsonConvert.DeserializeObject<PedidosResponse>(jsonResponse);

            }
        }

        public async Task ActualizarDetalle(PedidoDetalle pedidoDetalle)
        {
            PedidosResponse pedidosResponse = new();

            GeneralRequest generalRequest = new()
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

            var jsonContent = JsonConvert.SerializeObject(generalRequest.Parametros);

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            // Hacer la solicitud GET a la API
            HttpResponseMessage response = await _httpClient.PostAsync(_urlUpdateDetail, content);

            // Ensure the request was successful
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                // Leer el contenido de la respuesta como una cadena JSON
                var jsonResponse = await response.Content.ReadAsStringAsync();

                // Deserializar la cadena JSON a un objeto o lista de objetos
                //pedidosResponse = JsonConvert.DeserializeObject<PedidosResponse>(jsonResponse);

            }
        }

    }
}
