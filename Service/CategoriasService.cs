using Newtonsoft.Json;
using PersonalFinance.Helper;
using PersonalFinance.Models;
using PersonalFinance.Models.Categorias;
using PersonalFinance.Models.Entidades;
using PersonalFinance.Models.Pedidos;
using System.Net.Http;
using System.Text;

namespace PersonalFinance.Service
{
    public class CategoriasService
    {
        private readonly HttpClient _httpClient;
        private readonly string _urlGelAll = "https://localhost:443/api/v1/categories/getall";
        private readonly string _urlCreate = "https://localhost:443/api/v1/categories/create";
        private readonly string _urlUpdate = "https://localhost:443/api/v1/categories/update";

        public CategoriasService(HttpClient httpClient) 
        {
            this._httpClient = httpClient;
        }

        public async Task<CategoriasResponse> Obtener()
        {
            CategoriasResponse response = new();

            // Hacer la solicitud GET a la API
            HttpResponseMessage httpResponseMessage = await this._httpClient.GetAsync(_urlGelAll);

            // Ensure the request was successful
            httpResponseMessage.EnsureSuccessStatusCode();

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                // Leer el contenido de la respuesta como una cadena JSON
                var jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();

                // Deserializar la cadena JSON a un objeto o lista de objetos
                response = JsonConvert.DeserializeObject<CategoriasResponse>(jsonResponse);

                return response;
            }
            else
            {
                // Manejar el error si la respuesta no fue exitosa
                return response;
            }
        }

        public async Task Generar(Categoria entidad)
        {
            EntidadesResponse response = new();

            GeneralRequest generalRequest = new()
            {
                Parametros =
                [
                 new Parametro()
                 {
                     Nombre = "pCategoria",
                     Valor = entidad.Nombre,
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

        public async Task Actualizar(Categoria entidad)
        {
            CategoriasResponse response = new();

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
                     Nombre = "pCategoria",
                     Valor = entidad.Nombre,
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
