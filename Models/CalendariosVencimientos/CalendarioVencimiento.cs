namespace PersonalFinance.Models.Balance
{
    public class CalendarioVencimiento : AbstractModelExternder
    {
        public CalendarioVencimiento() { }

        /// <summary>
        /// Gets or sets propiedad FechaVencimiento.
        /// </summary>
        public DateTime? FechaVencimiento { get; set; }

        /// <summary>
        /// Gets or sets propiedad tipoGastoId.
        /// </summary>
        public int TipoGastoId { get; set; }

        /// <summary>
        /// Gets or sets propiedad Activo.
        /// </summary>
        public bool Activo { get; set; }
    }
}