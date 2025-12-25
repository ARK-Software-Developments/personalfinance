using Newtonsoft.Json;
using PersonalFinance.Helper;
using PersonalFinance.Models;
using PersonalFinance.Models.Enums;
using PersonalFinance.Models.Pedidos;
using System.Net.Http;
using System.Text;

namespace PersonalFinance.Service
{
    public class ServiceCaller
    {
        private readonly HttpClient _httpClient;
        private readonly CacheAdmin _cacheAdmin;
        private readonly string _urlPedido = "https://localhost:443/api/v1/orders/getall";
        private readonly string _urlPedidoDetalles = "https://localhost:443/api/v1/ordersdetails/getorderid/";
        private readonly string _urlCreate = "https://localhost:443/api/v1/orders/create";
        private readonly string _urlCreateDetail = "https://localhost/api/v1/ordersdetails/create";

        private readonly string _urlUpdate = "https://localhost:443/api/v1/orders/update";
        private readonly string _urlUpdateDetail = "https://localhost/api/v1/ordersdetails/update";

        public ServiceCaller(HttpContext httpContext, HttpClient httpClient) 
        {
            this._httpClient = httpClient;
            this._cacheAdmin = new CacheAdmin(httpContext);
        }

        public async Task<T> ObtenerRegistros<T>(ServicioEnum servicio, Dictionary<string, object> keyValuePairs = null, MetodoEnum metodo = MetodoEnum.Todos)
        {
            object apiResponse;
            
            apiResponse = _cacheAdmin.Obtener<T>(servicio);

            if (apiResponse != null)
            {
                return (T)apiResponse;
            }

            // Hacer la solicitud GET a la API
            HttpResponseMessage response = await this._httpClient.GetAsync(Microservicios.get(servicio, metodo, keyValuePairs));

            // Ensure the request was successful
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                // Leer el contenido de la respuesta como una cadena JSON
                var jsonResponse = await response.Content.ReadAsStringAsync();

                // Deserializar la cadena JSON a un objeto o lista de objetos
                apiResponse = JsonConvert.DeserializeObject<T>(jsonResponse);

                return (T)apiResponse;
            }
            else
            {
                // Manejar el error si la respuesta no fue exitosa
                return (T)new object();
            }
        }

        public async Task<T> GenerarRegistro<T>(ServicioEnum servicio, GeneralRequest generalRequest)
        {
            object apiResponse;

            var jsonContent = JsonConvert.SerializeObject(generalRequest.Parametros);

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Hacer la solicitud GET a la API
            HttpResponseMessage response = await this._httpClient.PutAsync(Microservicios.get(servicio, MetodoEnum.Nuevo), content);

            // Ensure the request was successful
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                // Leer el contenido de la respuesta como una cadena JSON
                var jsonResponse = await response.Content.ReadAsStringAsync();

                // Deserializar la cadena JSON a un objeto o lista de objetos
                apiResponse = JsonConvert.DeserializeObject<T>(jsonResponse);

                return (T)apiResponse;
            }
            else
            {
                // Manejar el error si la respuesta no fue exitosa
                return (T)new object();
            }
        }

        public async Task<T> ActualizarRegistro<T>(ServicioEnum servicio, GeneralRequest generalRequest, MetodoEnum metodoEnum = MetodoEnum.Actualizar)
        {
            object apiResponse;

            var jsonContent = JsonConvert.SerializeObject(generalRequest.Parametros);

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Hacer la solicitud GET a la API
            HttpResponseMessage response = await this._httpClient.PostAsync(Microservicios.get(servicio, metodoEnum), content);

            // Ensure the request was successful
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                // Leer el contenido de la respuesta como una cadena JSON
                var jsonResponse = await response.Content.ReadAsStringAsync();

                // Deserializar la cadena JSON a un objeto o lista de objetos
                apiResponse = JsonConvert.DeserializeObject<T>(jsonResponse);

                return (T)apiResponse;
            }
            else
            {
                // Manejar el error si la respuesta no fue exitosa
                return (T)new object();
            }
        }
    }
}
