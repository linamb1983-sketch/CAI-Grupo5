namespace Cart.API.DTOs;

/// <summary>
/// El carrito completo, tal como se lo devolvemos al cliente.
/// Es la respuesta de GET, POST y PUT.
/// </summary>
public class CartResponse
{
    /// <summary>Id del usuario dueño del carrito.</summary>
    /// <example>a1b2c3d4-0000-0000-0000-111122223333</example>
    public Guid UsuarioId { get; set; }

    /// <summary>Productos que tiene el carrito.</summary>
    public List<CartItemResponse> Items { get; set; } = new List<CartItemResponse>();

    /// <summary>Última vez que se modificó el carrito.</summary>
    /// <example>2024-03-10T10:45:00Z</example>
    public DateTime FechaActualizacion { get; set; }
}