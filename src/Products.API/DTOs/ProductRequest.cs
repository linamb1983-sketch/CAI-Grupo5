using System.ComponentModel.DataAnnotations;

namespace Products.API.DTOs;

/// <summary>
/// Datos que llegan del cliente para crear o modificar un producto.
/// No tiene Id ni FechaCreacion porque eso lo genera el servidor.
/// Las reglas salen del Apéndice A de la consigna.
/// </summary>
public class ProductRequest
{
    /// <summary>Nombre del producto. Obligatorio, máximo 100 caracteres.</summary>
    /// <example>Notebook Dell XPS 15</example>
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres")]
    public string Nombre { get; set; }

    /// <summary>Descripción opcional, máximo 500 caracteres.</summary>
    /// <example>Laptop 15 pulgadas, 32GB RAM</example>
    [MaxLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres")]
    public string? Descripcion { get; set; }

    /// <summary>Precio unitario. Tiene que ser mayor a 0.</summary>
    /// <example>1500.00</example>
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero")]
    public decimal Precio { get; set; }

    /// <summary>Unidades en stock. Tiene que ser mayor o igual a 0.</summary>
    /// <example>10</example>
    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
    public int Stock { get; set; }

    /// <summary>Categoría del producto. Obligatoria (ej: Electrónica, Hogar y Deco).</summary>
    /// <example>Electrónica</example>
    [Required(ErrorMessage = "La categoría es obligatoria")]
    public string Categoria { get; set; }
}