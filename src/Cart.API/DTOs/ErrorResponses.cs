namespace Cart.API.DTOs;

// Estas clases NO las usa el código para devolver errores (eso lo hacen los ExceptionHandlers).
// Existen solo para que Swagger muestre un ejemplo de cada error de la consigna (sección 3.1).

/// <summary>CRT-001 (404): el usuario no tiene carrito.</summary>
public class ErrorCarritoNoEncontrado
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
    /// <example>/api/cart/a1b2c3d4-0000-0000-0000-111122223333</example>
    public string Instance { get; set; } = string.Empty;

    /// <summary>Código de error del catálogo de la consigna.</summary>
    /// <example>CRT-001</example>
    public string ErrorCode { get; set; } = string.Empty;

    /// <summary>Mensaje del catálogo de errores.</summary>
    /// <example>Carrito no encontrado.</example>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>Id para seguir el pedido en los logs (mismo valor que el header X-Correlation-Id).</summary>
    /// <example>8b1f2c3d-4e5a-4b6c-9d7e-0f1a2b3c4d5e</example>
    public string CorrelationId { get; set; } = string.Empty;
}

/// <summary>CRT-002 (404): el producto no existe en Products o no está en el carrito.</summary>
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
    /// <example>/api/cart/a1b2c3d4-0000-0000-0000-111122223333/items</example>
    public string Instance { get; set; } = string.Empty;

    /// <summary>Código de error del catálogo de la consigna.</summary>
    /// <example>CRT-002</example>
    public string ErrorCode { get; set; } = string.Empty;

    /// <summary>Mensaje del catálogo de errores.</summary>
    /// <example>Producto no encontrado.</example>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>Id para seguir el pedido en los logs (mismo valor que el header X-Correlation-Id).</summary>
    /// <example>8b1f2c3d-4e5a-4b6c-9d7e-0f1a2b3c4d5e</example>
    public string CorrelationId { get; set; } = string.Empty;
}

/// <summary>CRT-003 (422): no hay stock suficiente en Products.</summary>
public class ErrorStockInsuficiente
{
    /// <summary>Enlace al tipo de error (RFC).</summary>
    /// <example>https://tools.ietf.org/html/rfc4918#section-11.2</example>
    public string Type { get; set; } = string.Empty;

    /// <summary>Nombre del error HTTP.</summary>
    /// <example>Unprocessable Entity</example>
    public string Title { get; set; } = string.Empty;

    /// <summary>Código de estado HTTP.</summary>
    /// <example>422</example>
    public int Status { get; set; }

    /// <summary>Explicación general del error.</summary>
    /// <example>No se puede procesar la solicitud.</example>
    public string Detail { get; set; } = string.Empty;

    /// <summary>Ruta del pedido que falló.</summary>
    /// <example>/api/cart/a1b2c3d4-0000-0000-0000-111122223333/items</example>
    public string Instance { get; set; } = string.Empty;

    /// <summary>Código de error del catálogo de la consigna.</summary>
    /// <example>CRT-003</example>
    public string ErrorCode { get; set; } = string.Empty;

}

/// <summary>CRT-004 (400): la cantidad es menor o igual a cero.</summary>
public class ErrorCantidadInvalida
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
    /// <example>/api/cart/a1b2c3d4-0000-0000-0000-111122223333/items</example>
    public string Instance { get; set; } = string.Empty;

    /// <summary>Código de error del catálogo de la consigna.</summary>
    /// <example>CRT-004</example>
    public string ErrorCode { get; set; } = string.Empty;

    /// <summary>Mensaje del catálogo de errores.</summary>
    /// <example>Cantidad inválida.</example>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>Id para seguir el pedido en los logs (mismo valor que el header X-Correlation-Id).</summary>
    /// <example>8b1f2c3d-4e5a-4b6c-9d7e-0f1a2b3c4d5e</example>
    public string CorrelationId { get; set; } = string.Empty;
}


/// <summary>CRT-005 (500): error inesperado del servicio (por ejemplo, Products no responde).</summary>
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
    /// <example>/api/cart/a1b2c3d4-0000-0000-0000-111122223333/items</example>
    public string Instance { get; set; } = string.Empty;

    /// <summary>Código de error del catálogo de la consigna.</summary>
    /// <example>CRT-005</example>
    public string ErrorCode { get; set; } = string.Empty;

    /// <summary>Mensaje del catálogo de errores.</summary>
    /// <example>Error interno al procesar el carrito.</example>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>Id para seguir el pedido en los logs (mismo valor que el header X-Correlation-Id).</summary>
    /// <example>8b1f2c3d-4e5a-4b6c-9d7e-0f1a2b3c4d5e</example>
    public string CorrelationId { get; set; } = string.Empty;
}
