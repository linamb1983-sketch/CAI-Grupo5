namespace Products.API.DTOs;

/// <summary>
/// Un renglón de una orden, tal como lo devuelve Orders API.
/// Products solo necesita saber qué producto es.
/// </summary>
public class OrderItemInfo
{
    /// <summary>Id del producto.</summary>
    public Guid ProductoId { get; set; }

    /// <summary>Cantidad pedida.</summary>
    public int Cantidad { get; set; }
}