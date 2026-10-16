using System.ComponentModel.DataAnnotations;

namespace Products.API.DTOs;

// Datos que llegan del cliente.
// No tiene Id ni FechaCreacion porque eso lo genera el servidor.
// Las reglas salen del Apéndice A de la consigna.
public class ProductRequest
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres")]
    public string Nombre { get; set; }

    [MaxLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres")]
    public string? Descripcion { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero")]
    public decimal Precio { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
    public int Stock { get; set; }

    [Required(ErrorMessage = "La categoría es obligatoria")]
    public string Categoria { get; set; }
}