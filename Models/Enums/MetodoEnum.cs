using System.ComponentModel;

namespace PersonalFinance.Models.Enums
{
    public enum MetodoEnum
    {
        [Api(Method = "getall")]        
        Todos = 1,

        [Api(Method = "getorderid")]
        DetallePedidoByOrderId = 2,

        [Api(Method = "get")]
        Uno = 3,

        [Api(Method = "create")]
        Nuevo = 4,

        [Api(Method = "update")]
        Actualizar = 5,

        [Api(Method = "updatecreditcardspending")]
        ActualizarTransConsumo = 100,

        [Api(Method = "resumenbycard")]
        TarjetaConsumoResumen = 101,

        [Api(Method = "incomecopybudget")]
        CopiarPresupuestoIngresos = 102,
    }
}
