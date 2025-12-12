using System.ComponentModel;

namespace PersonalFinance.Models.Enums
{
    /// <summary>
    /// Enum Extensions, para aplicacion de Enum.
    /// </summary>
    [Serializable]
    public static class EnumExtensions
    {
        /// <summary>
        /// This extension method is broken out so you can use a similar pattern with
        /// other MetaData elements in the future. This is your base method for each.
        /// </summary>
        /// <typeparam name="T">Type T.</typeparam>
        /// <param name="value">Enum.</param>
        /// <returns>Retorna Type T.</returns>
        public static T GetAttribute<T>(this Enum value)
            where T : Attribute
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
            return (T)attributes[0];
        }

        /// <summary>
        /// This method creates a specific call to the above method, requesting the
        /// Description MetaData attribute.
        /// </summary>
        /// <param name="value">Enum.</param>
        /// <returns>Retorna Type T.</returns>
        public static string ToName(this Enum value)
        {
            var attribute = value.GetAttribute<DescriptionAttribute>();
            return attribute == null ? value.ToString() : attribute.Description;
        }

        /// <summary>
        /// Método para obtener la ruta del servicio BFF.
        /// </summary>
        /// <param name="value">Enum.</param>
        /// <returns>Valor del atributo Route de la clase Api.</returns>
        public static string ToEndPoint(this Enum value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            _ = value.GetType();

            var attribute = value.GetAttribute<Api>();

            var endpoint = attribute == null ? value.ToString() : $"{((Api)attribute).GetAll()}";

            return endpoint;
        }

        /// <summary>
        /// Método para obtener la ruta del servicio BFF.
        /// </summary>
        /// <param name="value">Enum.</param>
        /// <returns>Valor del atributo Route de la clase Api.</returns>
        public static string ToRoute(this Enum value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            _ = value.GetType();

            var attribute = value.GetAttribute<Api>();

            var endpoint = attribute == null ? value.ToString() : $"{((Api)attribute).Route}";

            return endpoint;
        }

        /// <summary>
        /// Método para obtener la ruta del servicio BFF.
        /// </summary>
        /// <param name="value">Enum.</param>
        /// <returns>Valor del atributo Route de la clase Api.</returns>
        public static string ToMethod(this Enum value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            _ = value.GetType();

            var attribute = value.GetAttribute<Api>();

            var endpoint = attribute == null ? value.ToString() : $"{((Api)attribute).Method}";

            return endpoint;
        }

    }
}
