namespace Orders.API.DTOs;

// Datos que se devuelven al actualizar el estado de una orden.
public class UpdateOrderStatusResponse
{
    public Guid Id { get; set; }
    public string Estado { get; set; } = string.Empty;
    public DateTime FechaActualizacion { get; set; }
}
