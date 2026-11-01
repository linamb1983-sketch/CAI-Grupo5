namespace Products.API.Models;

/// <summary>
/// Producto del catálogo. Es lo que guardamos en el repositorio.
/// </summary>
public class Product
{
    /// <summary>Identificador único del producto.</summary>
    public Guid Id { get; set; }

    /// <summary>Nombre del producto (máximo 100 caracteres).</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Descripción opcional (máximo 500 caracteres).</summary>
    public string? Descripcion { get; set; }

    /// <summary>Precio unitario. Tiene que ser mayor a 0.</summary>
    public decimal Precio { get; set; }

    /// <summary>Unidades disponibles. Tiene que ser mayor o igual a 0.</summary>
    public int Stock { get; set; }

    /// <summary>Categoría del producto (ej: Electrónica).</summary>
    public string Categoria { get; set; } = string.Empty;

    /// <summary>Fecha en que se creó el producto. La asigna el servidor.</summary>
    public DateTime FechaCreacion { get; set; }
}