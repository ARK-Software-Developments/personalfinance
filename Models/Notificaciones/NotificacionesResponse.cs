namespace PersonalFinance.Models.Notificaciones;

using Newtonsoft.Json;

public class NotificacionesResponse : GeneralResponse
{
    public NotificacionesResponse() { }

    [JsonProperty("data")]
    public List<Notificacion>? Notificaciones { get; set; }
}
