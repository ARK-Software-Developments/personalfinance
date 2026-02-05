namespace PersonalFinance.Models.Notificaciones
{
    using PersonalFinanceApiNetCoreModel;

    /// <summary>
    /// Clase Notificacion.
    /// </summary>
    public class Notificacion : AbstractModelExternder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Notificacion"/> class.
        /// </summary>
        public Notificacion()
        {
        }

        /// <summary>
        /// Gets or sets propiedad notificationdate.
        /// </summary>
        public DateTime FechaNotificacion { get; set; }

        /// <summary>
        /// Gets or sets propiedad title.
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// Gets or sets propiedad type.
        /// </summary>
        public string Tipo { get; set; }

        /// <summary>
        /// Gets or sets propiedad messaje.
        /// </summary>
        public string Mensaje { get; set; }

        /// <summary>
        /// Gets or sets propiedad to.
        /// </summary>
        public string Para { get; set; }

        /// <summary>
        /// Gets or sets propiedad app.
        /// </summary>
        public string Aplicacion { get; set; }

        /// <summary>
        /// Gets or sets propiedad level.
        /// </summary>
        public string Nivel { get; set; }

        /// <summary>
        /// Gets or sets propiedad img.
        /// </summary>
        public string Imagen { get; set; }
    }
}