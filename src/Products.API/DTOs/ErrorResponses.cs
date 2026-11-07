namespace Products.API.DTOs;

// Estas clases NO las usa el código para devolver errores (eso lo hacen los ExceptionHandlers).
// Existen solo para que Swagger muestre un ejemplo de cada error de la consigna (sección 3.1).

/// <summary>PRD-001 (404): el producto no existe.</summary>
public class ErrorProductoNoEncontrado
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
    /// <example>/api/products/99</example>
    public string Instance { get; set; } = string.Empty;

    /// <summary>Código de error del catálogo de la consigna.</summary>
    /// <example>PRD-001</example>
    public string ErrorCode { get; set; } = string.Empty;

    /// <summary>Mensaje del catálogo de errores.</summary>
    /// <example>Producto no encontrado.</example>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>Id para seguir el pedido en los logs (mismo valor que el header X-Correlation-Id).</summary>
    /// <example>8b1f2c3d-4e5a-4b6c-9d7e-0f1a2b3c4d5e</example>
    public string CorrelationId { get; set; } = string.Empty;
}

/// <summary>PRD-002 (400): los datos del producto son inválidos.</summary>
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
    /// <example>/api/products</example>
    public string Instance { get; set; } = string.Empty;

    /// <summary>Código de error del catálogo de la consigna.</summary>
    /// <example>PRD-002</example>
    public string ErrorCode { get; set; } = string.Empty;

    /// <summary>Mensaje del catálogo de errores.</summary>
    /// <example>El nombre es obligatorio; El precio debe ser mayor a cero</example>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>Id para seguir el pedido en los logs (mismo valor que el header X-Correlation-Id).</summary>
    /// <example>8b1f2c3d-4e5a-4b6c-9d7e-0f1a2b3c4d5e</example>
    public string CorrelationId { get; set; } = string.Empty;
}

/// <summary>PRD-003 (409): ya existe un producto con ese nombre en la categoría.</summary>
public class ErrorProductoDuplicado
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
    /// <example>/api/products</example>
    public string Instance { get; set; } = string.Empty;

    /// <summary>Código de error del catálogo de la consigna.</summary>
    /// <example>PRD-003</example>
    public string ErrorCode { get; set; } = string.Empty;

    /// <summary>Mensaje del catálogo de errores.</summary>
    /// <example>Ya existe un producto con ese nombre en la categoría 'Electrónica'.</example>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>Id para seguir el pedido en los logs (mismo valor que el header X-Correlation-Id).</summary>
    /// <example>8b1f2c3d-4e5a-4b6c-9d7e-0f1a2b3c4d5e</example>
    public string CorrelationId { get; set; } = string.Empty;
}

/// <summary>PRD-004 (409): el producto tiene órdenes activas y no puede eliminarse.</summary>
public class ErrorProductoConOrdenes
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
    /// <example>No se puede eliminar el recurso.</example>
    public string Detail { get; set; } = string.Empty;

    /// <summary>Ruta del pedido que falló.</summary>
    /// <example>/api/products/3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    public string Instance { get; set; } = string.Empty;

    /// <summary>Código de error del catálogo de la consigna.</summary>
    /// <example>PRD-004</example>
    public string ErrorCode { get; set; } = string.Empty;

    /// <summary>Mensaje del catálogo de errores.</summary>
    /// <example>El producto tiene órdenes activas y no puede eliminarse.</example>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>Id para seguir el pedido en los logs (mismo valor que el header X-Correlation-Id).</summary>
    /// <example>8b1f2c3d-4e5a-4b6c-9d7e-0f1a2b3c4d5e</example>
    public string CorrelationId { get; set; } = string.Empty;
}

/// <summary>PRD-005 (500): error inesperado. Nunca muestra detalles técnicos.</summary>
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
    /// <example>/api/products</example>
    public string Instance { get; set; } = string.Empty;

    /// <summary>Código de error del catálogo de la consigna.</summary>
    /// <example>PRD-005</example>
    public string ErrorCode { get; set; } = string.Empty;

    /// <summary>Mensaje del catálogo de errores.</summary>
    /// <example>Error interno al procesar el producto.</example>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>Id para seguir el pedido en los logs (mismo valor que el header X-Correlation-Id).</summary>
    /// <example>8b1f2c3d-4e5a-4b6c-9d7e-0f1a2b3c4d5e</example>
    public string CorrelationId { get; set; } = string.Empty;
}
