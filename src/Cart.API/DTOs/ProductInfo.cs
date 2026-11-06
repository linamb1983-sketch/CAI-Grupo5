namespace Cart.API.DTOs;

/// <summary>
/// Lo que Cart lee de la respuesta de Products API (GET /api/products/{id}).
/// Products devuelve más campos, pero Cart solo necesita estos.
/// </summary>
public class ProductInfo
{
    /// <summary>Id del producto.</summary>
    public Guid Id { get; set; }

    /// <summary>Nombre del producto.</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Unidades disponibles en Products.</summary>
    public int Stock { get; set; }
}