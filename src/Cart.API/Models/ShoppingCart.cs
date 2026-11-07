namespace Cart.API.Models;

/// <summary>
/// Carrito de un usuario (en la consigna: Cart).
/// Se llama ShoppingCart para no confundirse con el nombre del proyecto Cart.API.
/// </summary>
public class ShoppingCart
{
    /// <summary>Id del usuario dueño del carrito.</summary>
    public Guid UsuarioId { get; set; }

    /// <summary>Productos que tiene el carrito.</summary>
    public List<CartItem> Items { get; set; } = new List<CartItem>();

    /// <summary>Última vez que se modificó el carrito. La asigna el servidor.</summary>
    public DateTime FechaActualizacion { get; set; }
}