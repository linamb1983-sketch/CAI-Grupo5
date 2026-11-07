namespace Orders.API.DTOs;

/// <summary>Datos de una orden que se devuelven al cliente.</summary>
public class OrderResponse
{
    /// <summary>Identificador de la orden.</summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    public Guid Id { get; set; }
    /// <summary>Identificador del usuario.</summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    public Guid UsuarioId { get; set; }
    /// <summary>Productos incluidos en la orden.</summary>
    public List<OrderItemResponse> Items { get; set; } = new List<OrderItemResponse>();
    /// <summary>Total de la orden.</summary>
    /// <example>199.98</example>
    public decimal Total { get; set; }
    /// <summary>Estado actual de la orden.</summary>
    /// <example>Pendiente</example>
    public string Estado { get; set; } = string.Empty;
    /// <summary>Fecha de creación en UTC.</summary>
    /// <example>2024-01-15T10:30:00Z</example>
    public DateTime FechaCreacion { get; set; }
}
