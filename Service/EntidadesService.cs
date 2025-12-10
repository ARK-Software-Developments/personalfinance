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
        private readonly string _urlGelAll = "https://localhost:443/api/v1/entities/getall";
        private readonly string _urlCreate = "https://localhost:443/api/v1/entities/create";
        private readonly string _urlUpdate = "https://localhost:443/api/v1/entities/update";

        public EntidadesService(HttpClient httpClient) 
        {
            this._httpClient = httpClient;
        }

        public async Task<EntidadesResponse> Obtener()
        {
            EntidadesResponse entidadesResponse = new();

            // Hacer la solicitud GET a la API
            HttpResponseMessage response = await this._httpClient.GetAsync(_urlGelAll);

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

        public async Task Generar(Entidad entidad)
        {
            EntidadesResponse response = new();

            GeneralRequest generalRequest = new()
            {
                Parametros =
                [
                 new Parametro()
                 {
                     Nombre = "pEntidad",
                     Valor = entidad.Nombre,
                 },
                 new Parametro()
                 {
                     Nombre = "pTipo",
                     Valor = entidad.Tipo,
                 },
             ],
            };

            var jsonContent = JsonConvert.SerializeObject(generalRequest.Parametros);

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            // Hacer la solicitud GET a la API
            HttpResponseMessage httpResponseMessage = await _httpClient.PutAsync(_urlCreate, content);

            // Ensure the request was successful
            httpResponseMessage.EnsureSuccessStatusCode();

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                // Leer el contenido de la respuesta como una cadena JSON
                var jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();

                // Deserializar la cadena JSON a un objeto o lista de objetos
                var generalDataResponse = JsonConvert.DeserializeObject<GeneralDataResponse>(jsonResponse);

            }
        }

        public async Task Actualizar(Entidad entidad)
        {
            PedidosResponse response = new();

            GeneralRequest generalRequest = new()
            {
                Parametros =
                [
                 new Parametro()
                 {
                     Nombre = "pId",
                     Valor = entidad.Id,
                 },
                 new Parametro()
                 {
                     Nombre = "pEntidad",
                     Valor = entidad.Nombre.ToUpper(),
                 },
                 new Parametro()
                 {
                     Nombre = "pTipo",
                     Valor = entidad.Tipo.ToUpper(),
                 },
             ],
            };

            var jsonContent = JsonConvert.SerializeObject(generalRequest.Parametros);

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            // Hacer la solicitud GET a la API
            HttpResponseMessage httpResponseMessage = await _httpClient.PostAsync(_urlUpdate, content);

            // Ensure the request was successful
            httpResponseMessage.EnsureSuccessStatusCode();

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                // Leer el contenido de la respuesta como una cadena JSON
                var jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();

                // Deserializar la cadena JSON a un objeto o lista de objetos
                var generalDataResponse = JsonConvert.DeserializeObject<GeneralDataResponse>(jsonResponse);

            }
        }
    }
}
