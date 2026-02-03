using System.ComponentModel;

namespace PersonalFinance.Models.Enums
{
    public enum ServicioEnum
    {
        [Api(Route = "cards", Cache = "cacheDataTarjetas")]
        [Description("Tarjetas")]
        Tarjetas = 1,

        [Api(Route = "entities", Cache = "cacheDataEntidades")]
        [Description("Entidades")]
        Entidades = 2,

        [Api(Route = "transactions", Cache = "cacheDataTransacciones")]
        [Description("Transacciones")]
        Transacciones = 3,

        [Api(Route = "bills", Cache = "cacheDataGastos")]
        [Description("GastosMensuales")]
        GastosMensuales = 4,

        [Api(Route = "typeofexpense", Cache = "cacheDataTipoGastos")]
        [Description("TipoGastos")]
        TipoGastos = 5,

        [Api(Route = "creditcardspending", Cache = "cacheDataConsumosTarjeta")]
        [Description("ConsumosTarjeta")]
        ConsumosTarjeta = 6,

        [Api(Route = "categories", Cache = "cacheDataCategorias")]
        [Description("Categorias")]
        Categorias = 7,

        [Api(Route = "status", Cache = "cacheDataEstados")]
        [Description("Estados")]
        Estados = 8,

        [Api(Route = "orders", Cache = "cacheDataPedidos")]
        [Description("Pedidos")]
        Pedidos = 9,

        [Api(Route = "ordersdetails", Cache = "cacheDataDetallePedido")]
        [Description("DetallePedido")]
        DetallePedido = 10,

        [Api(Route = "payments", Cache = "cacheDataPagos")]
        [Description("Pagos")]
        Pagos = 11,

        [Api(Route = "income", Cache = "cacheDataIngresos")]
        [Description("Ingresos")]
        Ingresos = 12,

        [Api(Route = "incomedetails", Cache = "cacheDataIngresoTipo")]
        [Description("IngresoTipo")]
        IngresoTipo = 13,

        [Api(Route = "creditcardspending", Cache = "cacheDataConsumosTarjetaResumen")]
        [Description("ConsumosTarjetaResumen")]
        ConsumosTarjetaResumen = 14,

        [Api(Route = "income", Cache = "cacheDataIngresosCopiaPresupuesto")]
        [Description("IngresosCopiaMensual")]
        IngresosCopiaMensual = 15,

        [Api(Route = "balance", Cache = "cacheDataBalance")]
        [Description("Balance")]
        Balance = 16,

        [Api(Route = "notifications", Cache = "cacheDataNotificacion")]
        [Description("Notificacion")]
        Notificacion = 17,

        [Api(Route = "process", Cache = "cacheDataConsumosTarjetaExecuteBudgetUpdate")]
        [Description("ConsumosTarjetaExecuteBudgetUpdate")]
        ConsumosTarjetaExecuteBudgetUpdate = 18,

        [Api(Route = "duedatesschedules", Cache = "cacheDataCalendariosVencimientos")]
        [Description("CalendariosVencimientos")]
        CalendariosVencimientos = 19,

        [Api(Route = "bills", Cache = "cacheDataPresupuestosCopiaMensual")]
        [Description("PresupuestosCopiaMensual")]
        PresupuestosCopiaMensual = 20,

        [Api(Route = "loans", Cache = "cacheDataPrestamos")]
        [Description("Prestamos")]
        Prestamos = 21,

        [Api(Route = "process", Cache = "cacheDataProcesos")]
        [Description("Procesos")]
        Procesos = 22,

        [Api(Route = "investments", Cache = "cacheDataInversiones")]
        [Description("Inversiones")]
        Inversiones = 23,

        [Api(Route = "investmenttype", Cache = "cacheDataInversionesTipos")]
        [Description("InversionesTipos")]
        InversionesTipos = 24,
    }
}
