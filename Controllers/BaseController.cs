

namespace PersonalFinance.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using PersonalFinance.Helper;
    using PersonalFinance.Models;
    using PersonalFinance.Models.Balance;
    using PersonalFinance.Models.Categorias;
    using PersonalFinance.Models.Entidades;
    using PersonalFinance.Models.Enums;
    using PersonalFinance.Models.Gastos;
    using PersonalFinance.Models.IngresosMensuales;
    using PersonalFinance.Models.IngresosTipo;
    using PersonalFinance.Models.Inversiones;
    using PersonalFinance.Models.InversionesElementos;
    using PersonalFinance.Models.InversionesInstrumentos;
    using PersonalFinance.Models.InversionTipo;
    using PersonalFinance.Models.Notificaciones;
    using PersonalFinance.Models.Pagos;
    using PersonalFinance.Models.Pedidos;
    using PersonalFinance.Models.Prestamos;
    using PersonalFinance.Models.TarjetaConsumos;
    using PersonalFinance.Models.Tarjetas;
    using PersonalFinance.Models.Transacciones;
    using PersonalFinance.Service;
    using System.Diagnostics;

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
        public PagosResponse pagosResponse;
        public IngresosResponse ingresosResponse;
        public IngresosTipoResponse ingresosTipoResponse;
        public TarjetaConsumoResumenResponse tarjetaConsumoResumenResponse;
        public BalanceResponse balanceResponse;
        public NotificacionesResponse notificacionResponse;
        public CalendarioVencimientoResponse calendarioVencimientoResponse;
        public PrestamoResponse prestamoResponse;
        public InversionesResponse inversionesResponse;
        public InversionTipoResponse inversionTipoResponse;
        public InversionesElementosResponse inversionesElementosResponse;
        public InversionesInstrumentosResponse inversionesInstrumentosResponse;

        public HttpContext httpContext;

        public BaseController() 
        {
        }

        public void Inicialized()
        {
            this.httpContext = ControllerContext.HttpContext;
            this.serviceCaller = new ServiceCaller(this.httpContext, new HttpClient(this.httpClientHandler));
            
            if (!this.keyValuePairs.ContainsKey("year"))
            {
                this.keyValuePairs.Add("year", Utils.GetYear(HttpContext));
            }
        }

        public async Task<GeneralDataResponse> EjecutarProceso(ServicioEnum servicioEnum, MetodoEnum metodoEnum)
        {
            try
            {
                if (metodoEnum == MetodoEnum.CopiarPresupuestoMensual || metodoEnum == MetodoEnum.CopiarIngresoMensual)
                {
                    var anoDesde = int.Parse(this.Request.Form["AnoDesde"]);
                    var anoHasta = int.Parse(this.Request.Form["AnoHasta"]);
                    var mesDesde = int.Parse(this.Request.Form["MesDesde"]);
                    var mesHasta = int.Parse(this.Request.Form["MesHasta"]);

                    this.generalRequest = new()
                    {
                        Parametros =
                                    [
                                     new Parametro()
                         {
                             Nombre = "pYearFrom",
                             Valor = anoDesde,
                         },
                         new Parametro()
                         {
                             Nombre = "pYearTo",
                             Valor = anoHasta,
                         },
                         new Parametro()
                         {
                             Nombre = "pMonthFrom",
                             Valor = mesDesde,
                         },
                         new Parametro()
                         {
                             Nombre = "pMonthTo",
                             Valor = mesHasta,
                         },
                     ],
                    };
                }
                else if (metodoEnum == MetodoEnum.ProcesoVRP)
                {
                    
                }

                generalDataResponse = await this.serviceCaller.EjecutarProceso<GeneralDataResponse>(servicioEnum, this.generalRequest, metodoEnum);                
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("PersonalFinance", ex.ToString(), EventLogEntryType.Error);
            }

            return this.generalDataResponse;
        }
    }
}
