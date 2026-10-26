namespace Cart.API.DTOs;

/// <summary>
/// Un renglón del carrito, tal como se lo devolvemos al cliente.
/// </summary>
public class CartItemResponse
{
    /// <summary>Id del producto.</summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    public Guid ProductoId { get; set; }

    /// <summary>Cantidad de unidades en el carrito.</summary>
    /// <example>2</example>
    public int Cantidad { get; set; }
}