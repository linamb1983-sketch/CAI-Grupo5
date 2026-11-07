namespace Orders.API.DTOs;

/// <summary>Datos que se devuelven al actualizar el estado de una orden.</summary>
public class UpdateOrderStatusResponse
{
    /// <summary>Identificador de la orden.</summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    public Guid Id { get; set; }
    /// <summary>Nuevo estado.</summary>
    /// <example>Confirmada</example>
    public string Estado { get; set; } = string.Empty;
    /// <summary>Fecha de actualización en UTC.</summary>
    /// <example>2024-01-15T10:30:00Z</example>
    public DateTime FechaActualizacion { get; set; }
}
