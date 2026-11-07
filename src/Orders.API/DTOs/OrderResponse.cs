namespace Orders.API.DTOs;

// Datos de una orden que se devuelven al cliente.
public class OrderResponse
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public List<OrderItemResponse> Items { get; set; } = new List<OrderItemResponse>();
    public decimal Total { get; set; }
    public string Estado { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
}
