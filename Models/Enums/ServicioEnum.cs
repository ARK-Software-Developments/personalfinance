using System.ComponentModel;

namespace PersonalFinance.Models.Enums
{
    public enum ServicioEnum
    {
        [Api(Route = "cards")]
        [Description("Tarjetas")]
        Tarjetas = 1,

        [Api(Route = "entities")]
        [Description("Entidades")]
        Entidades = 2,
    }
}
