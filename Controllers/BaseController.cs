

namespace PersonalFinance.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using PersonalFinance.Models;
    using PersonalFinance.Models.Categorias;
    using PersonalFinance.Models.Entidades;
    using PersonalFinance.Models.Gastos;
    using PersonalFinance.Models.Pedidos;
    using PersonalFinance.Models.TarjetaConsumos;
    using PersonalFinance.Models.Tarjetas;
    using PersonalFinance.Models.Transacciones;
    using PersonalFinance.Service;

    public class BaseController : Controller
    {
        public ServiceCaller serviceCaller;
        public HttpClientHandler httpClientHandler;
        public Dictionary<string, object> keyValuePairs = [];
        public GeneralDataResponse generalDataResponse;
        public GeneralRequest generalRequest;
        public EstadosResponse estadosResponse;
        public EntidadesResponse entidadesResponse;
        public CategoriasResponse categoriasResponse;
        public GastosResponse gastosResponse;
        public PedidosResponse pedidosResponse;
        public TarjetaConsumoResponse tarjetaConsumoResponse;
        public TarjetasResponse tarjetasResponse;
        public TransaccionesResponse transaccionesResponse;
        public TiposGastosResponse tiposGastosResponse;
        

        public HttpContext httpContext;

        public BaseController() 
        {
            this.keyValuePairs.Add("year", 2025);
        }

        public void Inicialized()
        {
            this.httpContext = ControllerContext.HttpContext;
            this.serviceCaller = new ServiceCaller(this.httpContext, new HttpClient(this.httpClientHandler));
            
            if (!this.keyValuePairs.ContainsKey("year"))
            {
                this.keyValuePairs.Add("year", 2025);
            }
        }
    }
}
