using System.ComponentModel;

namespace PersonalFinance.Models.Enums
{
    public enum MetodoEnum
    {
        [Api(Method = "getall")]        
        Todos = 1,

        [Api(Method = "getorderid")]
        DetallePedidoByOrderId = 2,

        [Api(Method = "getid")]
        Uno = 3,

        [Api(Method = "create")]
        Nuevo = 4,

        [Api(Method = "update")]
        Actualizar = 5,

        [Api(Method = "updatetransid")]
        ActualizarTransId = 100,
    }
}
