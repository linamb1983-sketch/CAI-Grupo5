namespace Orders.API.DTOs;

// Estos modelos existen para documentar en Swagger los errores del catálogo ORD.

/// <summary>ORD-001 (404): la orden no existe.</summary>
public class ErrorOrdenNoEncontrada
{
    /// <summary>Tipo de error RFC.</summary>
    /// <example>https://tools.ietf.org/html/rfc7231#section-6.5.4</example>
    public string Type { get; set; } = string.Empty;
    /// <summary>Título del error.</summary>
    /// <example>Not Found</example>
    public string Title { get; set; } = string.Empty;
    /// <summary>Estado HTTP.</summary>
    /// <example>404</example>
    public int Status { get; set; }
    /// <summary>Descripción del error.</summary>
    /// <example>El recurso solicitado no fue encontrado.</example>
    public string Detail { get; set; } = string.Empty;
    /// <summary>Ruta de la solicitud.</summary>
    /// <example>/api/orders/3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    public string Instance { get; set; } = string.Empty;
    /// <summary>Código del catálogo de Orders.</summary>
    /// <example>ORD-001</example>
    public string ErrorCode { get; set; } = string.Empty;
    /// <summary>Mensaje del catálogo de errores.</summary>
    /// <example>Orden no encontrada.</example>
    public string ErrorMessage { get; set; } = string.Empty;
    /// <summary>Id para seguir la solicitud en los logs.</summary>
    /// <example>8b1f2c3d-4e5a-4b6c-9d7e-0f1a2b3c4d5e</example>
    public string CorrelationId { get; set; } = string.Empty;
}

/// <summary>ORD-002 (400): los datos enviados son inválidos.</summary>
public class ErrorDatosInvalidos
{
    /// <summary>Tipo de error RFC.</summary>
    /// <example>https://tools.ietf.org/html/rfc7231#section-6.5.1</example>
    public string Type { get; set; } = string.Empty;
    /// <summary>Título del error.</summary>
    /// <example>Bad Request</example>
    public string Title { get; set; } = string.Empty;
    /// <summary>Estado HTTP.</summary>
    /// <example>400</example>
    public int Status { get; set; }
    /// <summary>Descripción del error.</summary>
    /// <example>Los datos enviados son inválidos.</example>
    public string Detail { get; set; } = string.Empty;
    /// <summary>Ruta de la solicitud.</summary>
    /// <example>/api/orders</example>
    public string Instance { get; set; } = string.Empty;
    /// <summary>Código del catálogo de Orders.</summary>
    /// <example>ORD-002</example>
    public string ErrorCode { get; set; } = string.Empty;
    /// <summary>Mensaje del catálogo de errores.</summary>
    /// <example>La orden debe incluir al menos un producto.</example>
    public string ErrorMessage { get; set; } = string.Empty;
    /// <summary>Id para seguir la solicitud en los logs.</summary>
    /// <example>8b1f2c3d-4e5a-4b6c-9d7e-0f1a2b3c4d5e</example>
    public string CorrelationId { get; set; } = string.Empty;
}

/// <summary>ORD-003 (404): el usuario de la orden no existe.</summary>
public class ErrorUsuarioNoEncontrado
{
    /// <summary>Tipo de error RFC.</summary>
    /// <example>https://tools.ietf.org/html/rfc7231#section-6.5.4</example>
    public string Type { get; set; } = string.Empty;
    /// <summary>Título del error.</summary>
    /// <example>Not Found</example>
    public string Title { get; set; } = string.Empty;
    /// <summary>Estado HTTP.</summary>
    /// <example>404</example>
    public int Status { get; set; }
    /// <summary>Descripción del error.</summary>
    /// <example>El recurso solicitado no fue encontrado.</example>
    public string Detail { get; set; } = string.Empty;
    /// <summary>Ruta de la solicitud.</summary>
    /// <example>/api/orders</example>
    public string Instance { get; set; } = string.Empty;
    /// <summary>Código del catálogo de Orders.</summary>
    /// <example>ORD-003</example>
    public string ErrorCode { get; set; } = string.Empty;
    /// <summary>Mensaje del catálogo de errores.</summary>
    /// <example>Usuario no encontrado al crear la orden.</example>
    public string ErrorMessage { get; set; } = string.Empty;
    /// <summary>Id para seguir la solicitud en los logs.</summary>
    /// <example>8b1f2c3d-4e5a-4b6c-9d7e-0f1a2b3c4d5e</example>
    public string CorrelationId { get; set; } = string.Empty;
}

/// <summary>ORD-004 (404): un producto de la orden no existe.</summary>
public class ErrorProductoNoEncontrado
{
    /// <summary>Tipo de error RFC.</summary>
    /// <example>https://tools.ietf.org/html/rfc7231#section-6.5.4</example>
    public string Type { get; set; } = string.Empty;
    /// <summary>Título del error.</summary>
    /// <example>Not Found</example>
    public string Title { get; set; } = string.Empty;
    /// <summary>Estado HTTP.</summary>
    /// <example>404</example>
    public int Status { get; set; }
    /// <summary>Descripción del error.</summary>
    /// <example>El recurso solicitado no fue encontrado.</example>
    public string Detail { get; set; } = string.Empty;
    /// <summary>Ruta de la solicitud.</summary>
    /// <example>/api/orders</example>
    public string Instance { get; set; } = string.Empty;
    /// <summary>Código del catálogo de Orders.</summary>
    /// <example>ORD-004</example>
    public string ErrorCode { get; set; } = string.Empty;
    /// <summary>Mensaje del catálogo de errores.</summary>
    /// <example>Producto no encontrado al crear la orden.</example>
    public string ErrorMessage { get; set; } = string.Empty;
    /// <summary>Id para seguir la solicitud en los logs.</summary>
    /// <example>8b1f2c3d-4e5a-4b6c-9d7e-0f1a2b3c4d5e</example>
    public string CorrelationId { get; set; } = string.Empty;
}

/// <summary>ORD-005 (422): no hay stock suficiente.</summary>
public class ErrorStockInsuficiente
{
    /// <summary>Tipo de error RFC.</summary>
    /// <example>https://tools.ietf.org/html/rfc4918#section-11.2</example>
    public string Type { get; set; } = string.Empty;
    /// <summary>Título del error.</summary>
    /// <example>UnprocessableEntity</example>
    public string Title { get; set; } = string.Empty;
    /// <summary>Estado HTTP.</summary>
    /// <example>422</example>
    public int Status { get; set; }
    /// <summary>Descripción del error.</summary>
    /// <example>No se puede procesar la solicitud.</example>
    public string Detail { get; set; } = string.Empty;
    /// <summary>Ruta de la solicitud.</summary>
    /// <example>/api/orders</example>
    public string Instance { get; set; } = string.Empty;
    /// <summary>Código del catálogo de Orders.</summary>
    /// <example>ORD-005</example>
    public string ErrorCode { get; set; } = string.Empty;
    /// <summary>Mensaje del catálogo de errores.</summary>
    /// <example>Stock insuficiente para el producto solicitado.</example>
    public string ErrorMessage { get; set; } = string.Empty;
    /// <summary>Id para seguir la solicitud en los logs.</summary>
    /// <example>8b1f2c3d-4e5a-4b6c-9d7e-0f1a2b3c4d5e</example>
    public string CorrelationId { get; set; } = string.Empty;
}

/// <summary>ORD-006 (409): no se permite el cambio de estado solicitado.</summary>
public class ErrorCambioEstadoNoPermitido
{
    /// <summary>Tipo de error RFC.</summary>
    /// <example>https://tools.ietf.org/html/rfc7231#section-6.5.9</example>
    public string Type { get; set; } = string.Empty;
    /// <summary>Título del error.</summary>
    /// <example>Conflict</example>
    public string Title { get; set; } = string.Empty;
    /// <summary>Estado HTTP.</summary>
    /// <example>409</example>
    public int Status { get; set; }
    /// <summary>Descripción del error.</summary>
    /// <example>La operación no puede completarse por una regla del negocio.</example>
    public string Detail { get; set; } = string.Empty;
    /// <summary>Ruta de la solicitud.</summary>
    /// <example>/api/orders/3fa85f64-5717-4562-b3fc-2c963f66afa6/status</example>
    public string Instance { get; set; } = string.Empty;
    /// <summary>Código del catálogo de Orders.</summary>
    /// <example>ORD-006</example>
    public string ErrorCode { get; set; } = string.Empty;
    /// <summary>Mensaje del catálogo de errores.</summary>
    /// <example>El estado de la orden no puede ser modificado.</example>
    public string ErrorMessage { get; set; } = string.Empty;
    /// <summary>Id para seguir la solicitud en los logs.</summary>
    /// <example>8b1f2c3d-4e5a-4b6c-9d7e-0f1a2b3c4d5e</example>
    public string CorrelationId { get; set; } = string.Empty;
}

/// <summary>ORD-007 (500): ocurrió un error inesperado.</summary>
public class ErrorInterno
{
    /// <summary>Tipo de error RFC.</summary>
    /// <example>https://tools.ietf.org/html/rfc7231#section-6.6.1</example>
    public string Type { get; set; } = string.Empty;
    /// <summary>Título del error.</summary>
    /// <example>Internal Server Error</example>
    public string Title { get; set; } = string.Empty;
    /// <summary>Estado HTTP.</summary>
    /// <example>500</example>
    public int Status { get; set; }
    /// <summary>Descripción del error.</summary>
    /// <example>Ocurrió un error inesperado.</example>
    public string Detail { get; set; } = string.Empty;
    /// <summary>Ruta de la solicitud.</summary>
    /// <example>/api/orders</example>
    public string Instance { get; set; } = string.Empty;
    /// <summary>Código del catálogo de Orders.</summary>
    /// <example>ORD-007</example>
    public string ErrorCode { get; set; } = string.Empty;
    /// <summary>Mensaje del catálogo de errores.</summary>
    /// <example>Error interno al procesar la orden.</example>
    public string ErrorMessage { get; set; } = string.Empty;
    /// <summary>Id para seguir la solicitud en los logs.</summary>
    /// <example>8b1f2c3d-4e5a-4b6c-9d7e-0f1a2b3c4d5e</example>
    public string CorrelationId { get; set; } = string.Empty;
}
