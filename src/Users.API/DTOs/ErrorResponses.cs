namespace Users.API.DTOs;

// Estas clases NO las usa el código para devolver errores (eso lo hacen los ExceptionHandlers).
// Existen solo para que Swagger muestre un ejemplo de cada error de la consigna (sección 3.1).

/// <summary>USR-007 (404): el usuario no existe.</summary>
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
    /// <example>/api/users/a1b2c3d4-0000-0000-0000-111122223333</example>
    public string Instance { get; set; } = string.Empty;

    /// <summary>Código de error del catálogo de la consigna.</summary>
    /// <example>USR-007</example>
    public string ErrorCode { get; set; } = string.Empty;

    /// <summary>Mensaje del catálogo de errores.</summary>
    /// <example>Usuario no encontrado.</example>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>Id para seguir el pedido en los logs (mismo valor que el header X-Correlation-Id).</summary>
    /// <example>8b1f2c3d-4e5a-4b6c-9d7e-0f1a2b3c4d5e</example>
    public string CorrelationId { get; set; } = string.Empty;
}

/// <summary>USR-002 (400): los datos del usuario son inválidos.</summary>
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
    /// <example>/api/users/register</example>
    public string Instance { get; set; } = string.Empty;

    /// <summary>Código de error del catálogo de la consigna.</summary>
    /// <example>USR-002</example>
    public string ErrorCode { get; set; } = string.Empty;

    /// <summary>Mensaje del catálogo de errores.</summary>
    /// <example>El nombre es obligatorio; El email no tiene un formato válido</example>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>Id para seguir el pedido en los logs (mismo valor que el header X-Correlation-Id).</summary>
    /// <example>8b1f2c3d-4e5a-4b6c-9d7e-0f1a2b3c4d5e</example>
    public string CorrelationId { get; set; } = string.Empty;
}

/// <summary>USR-001 (409): el email ya está registrado.</summary>
public class ErrorEmailDuplicado
{
    /// <summary>Enlace al tipo de error (RFC).</summary>
    /// <example>https://tools.ietf.org/html/rfc7231#section-6.5.9</example>
    public string Type { get; set; } = string.Empty;

    /// <summary>Nombre del error HTTP.</summary>
    /// <example>Conflict</example>
    public string Title { get; set; } = string.Empty;

    /// <summary>Código de estado HTTP.</summary>
    /// <example>409</example>
    public int Status { get; set; }

    /// <summary>Explicación general del error.</summary>
    /// <example>Ya existe un recurso con esos datos.</example>
    public string Detail { get; set; } = string.Empty;

    /// <summary>Ruta del pedido que falló.</summary>
    /// <example>/api/users/register</example>
    public string Instance { get; set; } = string.Empty;

    /// <summary>Código de error del catálogo de la consigna.</summary>
    /// <example>USR-001</example>
    public string ErrorCode { get; set; } = string.Empty;

    /// <summary>Mensaje del catálogo de errores.</summary>
    /// <example>El email 'maria@email.com' ya está registrado.</example>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>Id para seguir el pedido en los logs (mismo valor que el header X-Correlation-Id).</summary>
    /// <example>8b1f2c3d-4e5a-4b6c-9d7e-0f1a2b3c4d5e</example>
    public string CorrelationId { get; set; } = string.Empty;
}

/// <summary>USR-003 (401): el email o la contraseña no coinciden.</summary>
public class ErrorCredencialesIncorrectas
{
    /// <summary>Enlace al tipo de error (RFC).</summary>
    /// <example>https://tools.ietf.org/html/rfc7235#section-3.1</example>
    public string Type { get; set; } = string.Empty;

    /// <summary>Nombre del error HTTP.</summary>
    /// <example>Unauthorized</example>
    public string Title { get; set; } = string.Empty;

    /// <summary>Código de estado HTTP.</summary>
    /// <example>401</example>
    public int Status { get; set; }

    /// <summary>Explicación general del error.</summary>
    /// <example>Las credenciales no son válidas.</example>
    public string Detail { get; set; } = string.Empty;

    /// <summary>Ruta del pedido que falló.</summary>
    /// <example>/api/users/login</example>
    public string Instance { get; set; } = string.Empty;

    /// <summary>Código de error del catálogo de la consigna.</summary>
    /// <example>USR-003</example>
    public string ErrorCode { get; set; } = string.Empty;

    /// <summary>Mensaje del catálogo de errores.</summary>
    /// <example>Credenciales incorrectas.</example>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>Id para seguir el pedido en los logs (mismo valor que el header X-Correlation-Id).</summary>
    /// <example>8b1f2c3d-4e5a-4b6c-9d7e-0f1a2b3c4d5e</example>
    public string CorrelationId { get; set; } = string.Empty;
}

/// <summary>USR-004 (403): usuario bloqueado por demasiados intentos fallidos.</summary>
public class ErrorBloqueadoPorIntentos
{
    /// <summary>Enlace al tipo de error (RFC).</summary>
    /// <example>https://tools.ietf.org/html/rfc7231#section-6.5.3</example>
    public string Type { get; set; } = string.Empty;

    /// <summary>Nombre del error HTTP.</summary>
    /// <example>Forbidden</example>
    public string Title { get; set; } = string.Empty;

    /// <summary>Código de estado HTTP.</summary>
    /// <example>403</example>
    public int Status { get; set; }

    /// <summary>Explicación general del error.</summary>
    /// <example>El acceso está prohibido.</example>
    public string Detail { get; set; } = string.Empty;

    /// <summary>Ruta del pedido que falló.</summary>
    /// <example>/api/users/login</example>
    public string Instance { get; set; } = string.Empty;

    /// <summary>Código de error del catálogo de la consigna.</summary>
    /// <example>USR-004</example>
    public string ErrorCode { get; set; } = string.Empty;

    /// <summary>Mensaje del catálogo de errores.</summary>
    /// <example>Su cuenta fue bloqueada por superar el máximo de intentos fallidos. Contacte a soporte.</example>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>Id para seguir el pedido en los logs (mismo valor que el header X-Correlation-Id).</summary>
    /// <example>8b1f2c3d-4e5a-4b6c-9d7e-0f1a2b3c4d5e</example>
    public string CorrelationId { get; set; } = string.Empty;
}

/// <summary>USR-005 (403): usuario bloqueado por detección de fraude.</summary>
public class ErrorBloqueadoPorFraude
{
    /// <summary>Enlace al tipo de error (RFC).</summary>
    /// <example>https://tools.ietf.org/html/rfc7231#section-6.5.3</example>
    public string Type { get; set; } = string.Empty;

    /// <summary>Nombre del error HTTP.</summary>
    /// <example>Forbidden</example>
    public string Title { get; set; } = string.Empty;

    /// <summary>Código de estado HTTP.</summary>
    /// <example>403</example>
    public int Status { get; set; }

    /// <summary>Explicación general del error.</summary>
    /// <example>El acceso está prohibido.</example>
    public string Detail { get; set; } = string.Empty;

    /// <summary>Ruta del pedido que falló.</summary>
    /// <example>/api/users/login</example>
    public string Instance { get; set; } = string.Empty;

    /// <summary>Código de error del catálogo de la consigna.</summary>
    /// <example>USR-005</example>
    public string ErrorCode { get; set; } = string.Empty;

    /// <summary>Mensaje del catálogo de errores.</summary>
    /// <example>Su cuenta fue suspendida por razones de seguridad. Contacte a soporte.</example>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>Id para seguir el pedido en los logs (mismo valor que el header X-Correlation-Id).</summary>
    /// <example>8b1f2c3d-4e5a-4b6c-9d7e-0f1a2b3c4d5e</example>
    public string CorrelationId { get; set; } = string.Empty;
}

/// <summary>USR-006 (500): error inesperado.</summary>
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
    /// <example>/api/users/register</example>
    public string Instance { get; set; } = string.Empty;

    /// <summary>Código de error del catálogo de la consigna.</summary>
    /// <example>USR-006</example>
    public string ErrorCode { get; set; } = string.Empty;

    /// <summary>Mensaje del catálogo de errores.</summary>
    /// <example>Error interno al procesar el usuario.</example>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>Id para seguir el pedido en los logs (mismo valor que el header X-Correlation-Id).</summary>
    /// <example>8b1f2c3d-4e5a-4b6c-9d7e-0f1a2b3c4d5e</example>
    public string CorrelationId { get; set; } = string.Empty;
}