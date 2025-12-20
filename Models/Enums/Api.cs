namespace PersonalFinance.Models.Enums
{
    /// <summary>
    /// Clase Api propiedad de Enum.
    /// </summary>
    public class Api : Attribute
    {
        private readonly string url = "https://localhost:443/api/v1/{0}/{1}";
        /// <summary>
        /// Obtiene o establece atributo o propiedad de la clase Api.
        /// </summary>
        public string Route { get; set; }

        public string Cache { get; set; }

        public string Method { get; set; }

        public string GetAll()
        {
            return this.url.Replace("{0}", this.Route).Replace("{1}", this.Method);
        }
    }
}
