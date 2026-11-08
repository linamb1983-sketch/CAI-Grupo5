namespace Notifications.API.DTOs;

// Estas clases NO las usa el código para devolver errores (eso lo hacen los ExceptionHandlers).
// Existen solo para que Swagger muestre un ejemplo de cada error de la consigna (sección 3.1).

/// <summary>NTF-001 (404): el usuario destinatario no existe en Users.</summary>
public class ErrorUsuarioNoEncontrado
{
    /// <summary>Enlace al tipo de error (RFC).</summary>
    /// <example>https://tools.ietf.org/html/rfc7231#section-6.5.4</example>
    public string Type { get; set; } = string.Empty;

    /// <summary>Nombre del error HTTP.</summary>
    /// <example>Not Found</example>
    public string Title { get; set; } = string.Empty;

    /// <summary>Código de estado HTTP.</summary>
    /// <example>404</example>
    public int Status { get; set; }

    /// <summary>Explicación general del error.</summary>
    /// <example>El recurso solicitado no fue encontrado.</example>
    public string Detail { get; set; } = string.Empty;

    /// <summary>Ruta del pedido que falló.</summary>
    /// <example>/api/notifications/send</example>
    public string Instance { get; set; } = string.Empty;

    /// <summary>Código de error del catálogo de la consigna.</summary>
    /// <example>NTF-001</example>
    public string ErrorCode { get; set; } = string.Empty;

    /// <summary>Mensaje del catálogo de errores.</summary>
    /// <example>El usuario destinatario no fue encontrado.</example>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>Id para seguir el pedido en los logs (mismo valor que el header X-Correlation-Id).</summary>
    /// <example>8b1f2c3d-4e5a-4b6c-9d7e-0f1a2b3c4d5e</example>
    public string CorrelationId { get; set; } = string.Empty;
}

/// <summary>NTF-002 (400): faltan datos o el tipo no es Email, Push ni SMS.</summary>
public class ErrorDatosInvalidos
{
    /// <summary>Enlace al tipo de error (RFC).</summary>
    /// <example>https://tools.ietf.org/html/rfc7231#section-6.5.1</example>
    public string Type { get; set; } = string.Empty;

    /// <summary>Nombre del error HTTP.</summary>
    /// <example>Bad Request</example>
    public string Title { get; set; } = string.Empty;

    /// <summary>Código de estado HTTP.</summary>
    /// <example>400</example>
    public int Status { get; set; }

    /// <summary>Explicación general del error.</summary>
    /// <example>Los datos enviados son inválidos.</example>
    public string Detail { get; set; } = string.Empty;

    /// <summary>Ruta del pedido que falló.</summary>
    /// <example>/api/notifications/send</example>
    public string Instance { get; set; } = string.Empty;

    /// <summary>Código de error del catálogo de la consigna.</summary>
    /// <example>NTF-002</example>
    public string ErrorCode { get; set; } = string.Empty;

    /// <summary>Mensaje del catálogo de errores.</summary>
    /// <example>Los datos de la notificación son inválidos.</example>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>Id para seguir el pedido en los logs (mismo valor que el header X-Correlation-Id).</summary>
    /// <example>8b1f2c3d-4e5a-4b6c-9d7e-0f1a2b3c4d5e</example>
    public string CorrelationId { get; set; } = string.Empty;
}

/// <summary>NTF-003 (404): el usuario no tiene notificaciones.</summary>
public class ErrorSinNotificaciones
{
    /// <summary>Enlace al tipo de error (RFC).</summary>
    /// <example>https://tools.ietf.org/html/rfc7231#section-6.5.4</example>
    public string Type { get; set; } = string.Empty;

    /// <summary>Nombre del error HTTP.</summary>
    /// <example>Not Found</example>
    public string Title { get; set; } = string.Empty;

    /// <summary>Código de estado HTTP.</summary>
    /// <example>404</example>
    public int Status { get; set; }

    /// <summary>Explicación general del error.</summary>
    /// <example>El recurso solicitado no fue encontrado.</example>
    public string Detail { get; set; } = string.Empty;

    /// <summary>Ruta del pedido que falló.</summary>
    /// <example>/api/notifications/a1b2c3d4-0000-0000-0000-111122223333</example>
    public string Instance { get; set; } = string.Empty;

    /// <summary>Código de error del catálogo de la consigna.</summary>
    /// <example>NTF-003</example>
    public string ErrorCode { get; set; } = string.Empty;

    /// <summary>Mensaje del catálogo de errores.</summary>
    /// <example>No se encontraron notificaciones para el usuario.</example>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>Id para seguir el pedido en los logs (mismo valor que el header X-Correlation-Id).</summary>
    /// <example>8b1f2c3d-4e5a-4b6c-9d7e-0f1a2b3c4d5e</example>
    public string CorrelationId { get; set; } = string.Empty;
}

/// <summary>NTF-004 (500): error inesperado del servicio (por ejemplo, Users no responde).</summary>
public class ErrorInterno
{
    /// <summary>Enlace al tipo de error (RFC).</summary>
    /// <example>https://tools.ietf.org/html/rfc7231#section-6.6.1</example>
    public string Type { get; set; } = string.Empty;

    /// <summary>Nombre del error HTTP.</summary>
    /// <example>Internal Server Error</example>
    public string Title { get; set; } = string.Empty;

    /// <summary>Código de estado HTTP.</summary>
    /// <example>500</example>
    public int Status { get; set; }

    /// <summary>Explicación general del error.</summary>
    /// <example>Ocurrió un error inesperado.</example>
    public string Detail { get; set; } = string.Empty;

    /// <summary>Ruta del pedido que falló.</summary>
    /// <example>/api/notifications/send</example>
    public string Instance { get; set; } = string.Empty;

    /// <summary>Código de error del catálogo de la consigna.</summary>
    /// <example>NTF-004</example>
    public string ErrorCode { get; set; } = string.Empty;

    /// <summary>Mensaje del catálogo de errores.</summary>
    /// <example>Error interno al procesar la notificación.</example>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>Id para seguir el pedido en los logs (mismo valor que el header X-Correlation-Id).</summary>
    /// <example>8b1f2c3d-4e5a-4b6c-9d7e-0f1a2b3c4d5e</example>
    public string CorrelationId { get; set; } = string.Empty;
}