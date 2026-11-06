using System.Net;
using Notifications.API.DTOs;
using Notifications.API.Exceptions;
using Notifications.API.Models;
using Notifications.API.Repositories;

namespace Notifications.API.Services;

// Reglas de negocio de las notificaciones.
// Para saber si el usuario existe, le pregunta a Users API por HTTP.
public class NotificationService
{
    private NotificationRepository repositorio;
    private IHttpClientFactory httpClientFactory;
    private IConfiguration configuracion;

    // Recibe el repositorio, la fábrica de clientes HTTP y la configuración (appsettings.json)
    public NotificationService(NotificationRepository repositorio, IHttpClientFactory httpClientFactory, IConfiguration configuracion)
    {
        this.repositorio = repositorio;
        this.httpClientFactory = httpClientFactory;
        this.configuracion = configuracion;
    }

    // POST: registra la notificación y simula el envío.
    public async Task<NotificationResponse> Enviar(SendNotificationRequest request)
    {
        // NTF-002: tiene que venir el usuario
        if (request.UsuarioId == Guid.Empty)
        {
            throw new ValidationException(ErrorCodes.NTF_002, "Los datos de la notificación son inválidos.");
        }

        // NTF-002: tiene que venir el mensaje
        if (request.Mensaje == null || request.Mensaje.Trim() == "")
        {
            throw new ValidationException(ErrorCodes.NTF_002, "Los datos de la notificación son inválidos.");
        }

        // NTF-002: el tipo tiene que ser Email, Push o SMS
        if (EsTipoValido(request.Tipo) == false)
        {
            throw new ValidationException(ErrorCodes.NTF_002, "Tipo de notificación no reconocido. Usar Email, Push o SMS.");
        }

        // NTF-001: el usuario tiene que existir en Users
        await ValidarUsuario(request.UsuarioId);

        // Pasó todas las reglas: se registra como enviada (el envío es simulado)
        Notification notificacion = new Notification();
        notificacion.Id = Guid.NewGuid();
        notificacion.UsuarioId = request.UsuarioId;
        notificacion.Mensaje = request.Mensaje;
        notificacion.Tipo = request.Tipo;
        notificacion.Estado = "Enviada";
        notificacion.FechaEnvio = DateTime.UtcNow;

        repositorio.Agregar(notificacion);

        return ConvertirAResponse(notificacion);
    }

    // GET: devuelve las notificaciones de un usuario. Si no tiene ninguna, NTF-003.
    public List<NotificationResponse> ObtenerPorUsuario(Guid usuarioId)
    {
        List<Notification> notificaciones = repositorio.ObtenerPorUsuario(usuarioId);

        if (notificaciones.Count == 0)
        {
            throw new NotFoundException(ErrorCodes.NTF_003, "No se encontraron notificaciones para el usuario.");
        }

        List<NotificationResponse> respuesta = new List<NotificationResponse>();

        foreach (Notification n in notificaciones)
        {
            respuesta.Add(ConvertirAResponse(n));
        }

        return respuesta;
    }

    // Le pregunta a Users API si el usuario existe. Si no existe, NTF-001.
    private async Task ValidarUsuario(Guid usuarioId)
    {
        // Arma la dirección: http://localhost:5002/api/users/{id}
        string direccion = configuracion["Services:Users:BaseUrl"];
        string url = $"{direccion}/api/users/{usuarioId}";

        HttpClient cliente = httpClientFactory.CreateClient();
        HttpResponseMessage respuesta = await cliente.GetAsync(url);

        // Primero se mira el código HTTP (lo pide la consigna)
        if (respuesta.StatusCode == HttpStatusCode.NotFound)
        {
            throw new NotFoundException(ErrorCodes.NTF_001, "El usuario destinatario no fue encontrado.");
        }

        // Cualquier otro error de Users es inesperado: lo ataja el GlobalExceptionHandler (500, NTF-004)
        if (respuesta.IsSuccessStatusCode == false)
        {
            throw new Exception($"Users API respondió con error {(int)respuesta.StatusCode}.");
        }
    }

    // Revisa que el tipo sea uno de los tres permitidos
    private bool EsTipoValido(string tipo)
    {
        if (tipo == "Email" || tipo == "Push" || tipo == "SMS")
        {
            return true;
        }
        return false;
    }

    // Pasa la notificación de adentro (Model) al formulario de salida (DTO)
    private NotificationResponse ConvertirAResponse(Notification notificacion)
    {
        NotificationResponse respuesta = new NotificationResponse();
        respuesta.Id = notificacion.Id;
        respuesta.UsuarioId = notificacion.UsuarioId;
        respuesta.Mensaje = notificacion.Mensaje;
        respuesta.Tipo = notificacion.Tipo;
        respuesta.Estado = notificacion.Estado;
        respuesta.FechaEnvio = notificacion.FechaEnvio;
        return respuesta;
    }
}