using Microsoft.AspNetCore.Mvc;
using Notifications.API.DTOs;
using Notifications.API.Exceptions;
using Notifications.API.Services;

namespace Notifications.API.Controllers;

/// <summary>
/// Recibe los pedidos HTTP de notificaciones y se los pasa al Service.
/// </summary>
[ApiController]
[Route("api/notifications")]
[Tags("Notifications")]
public class NotificationsController : ControllerBase
{
    private NotificationService servicio;

    public NotificationsController(NotificationService servicio)
    {
        this.servicio = servicio;
    }

    /// <summary>
    /// Registra una notificación y simula su envío.
    /// </summary>
    /// <param name="request">Usuario destinatario, mensaje y tipo (Email, Push o SMS).</param>
    /// <response code="201">Notificación registrada y enviada (simulado).</response>
    /// <response code="400">NTF-002: Los datos de la notificación son inválidos.</response>
    /// <response code="404">NTF-001: Usuario no encontrado.</response>
    /// <response code="500">NTF-004: Error interno al procesar la notificación.</response>
    [HttpPost("send")]
    [ProducesResponseType(typeof(NotificationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorDatosInvalidos), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorUsuarioNoEncontrado), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorInterno), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Enviar(SendNotificationRequest request)
    {
        ValidarDatos();

        NotificationResponse notificacion = await servicio.Enviar(request);

        // 201 Created, como pide la consigna
        return StatusCode(StatusCodes.Status201Created, notificacion);
    }

    /// <summary>
    /// Lista las notificaciones de un usuario.
    /// </summary>
    /// <param name="userId">Id del usuario.</param>
    /// <response code="200">Lista de notificaciones del usuario.</response>
    /// <response code="404">NTF-003: No se encontraron notificaciones para el usuario.</response>
    /// <response code="500">NTF-004: Error interno al procesar la notificación.</response>
    [HttpGet("{userId}")]
    [ProducesResponseType(typeof(List<NotificationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorSinNotificaciones), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorInterno), StatusCodes.Status500InternalServerError)]
    public IActionResult ObtenerPorUsuario(Guid userId)
    {
        List<NotificationResponse> notificaciones = servicio.ObtenerPorUsuario(userId);
        return Ok(notificaciones);
    }

    // Si los datos no cumplen las reglas del DTO, junta todos los errores
    // separados por punto y coma y corta con NTF-002.
    private void ValidarDatos()
    {
        if (ModelState.IsValid)
        {
            return;
        }

        string errores = "";

        foreach (var campo in ModelState.Values)
        {
            foreach (var error in campo.Errors)
            {
                errores = errores + error.ErrorMessage + "; ";
            }
        }

        if (errores == "")
        {
            errores = "Los datos de la notificación son inválidos.";
        }

        throw new ValidationException(ErrorCodes.NTF_002, errores);
    }
}