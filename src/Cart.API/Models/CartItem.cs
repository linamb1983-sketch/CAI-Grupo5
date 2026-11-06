namespace Cart.API.Models;

/// <summary>
/// Un renglón del carrito: qué producto y cuántas unidades.
/// </summary>
public class CartItem
{
    /// <summary>Id del producto (viene de Products API).</summary>
    public Guid ProductoId { get; set; }

    /// <summary>Cantidad de unidades. Tiene que ser mayor a 0.</summary>
    public int Cantidad { get; set; }
}