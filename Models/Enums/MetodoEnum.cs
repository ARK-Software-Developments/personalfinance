using System.ComponentModel;

namespace PersonalFinance.Models.Enums
{
    public enum MetodoEnum
    {
        [Api(Method = "getall")]        
        Todos = 1,

        [Api(Method = "getid")]
        Uno = 2,

        [Api(Method = "create")]
        Nuevo = 3,

        [Api(Method = "update")]
        Actualizar = 4,

        [Api(Method = "updatetransid")]
        ActualizarTransId = 100,
    }
}
