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
        [Description("Gastos")]
        Gastos = 4,

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
    }
}
