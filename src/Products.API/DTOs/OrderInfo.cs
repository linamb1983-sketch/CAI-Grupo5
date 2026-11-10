namespace Products.API.DTOs;

/// <summary>
/// Lo que Products lee de la respuesta de Orders API (GET /api/orders).
/// Orders devuelve más campos, pero Products solo necesita estos.
/// </summary>
public class OrderInfo
{
    /// <summary>Id de la orden.</summary>
    public Guid Id { get; set; }

    /// <summary>Estado de la orden: Pendiente, Confirmada, Enviada, Entregada o Cancelada.</summary>
    public string Estado { get; set; } = string.Empty;

    /// <summary>Productos que tiene la orden.</summary>
    public List<OrderItemInfo> Items { get; set; } = new List<OrderItemInfo>();
}