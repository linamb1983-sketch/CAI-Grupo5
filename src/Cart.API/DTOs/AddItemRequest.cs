using System.ComponentModel.DataAnnotations;

namespace Cart.API.DTOs;

/// <summary>
/// Datos que manda el cliente para agregar un producto al carrito (POST).
/// El userId no va acá porque viene en la ruta: /api/cart/{userId}/items
/// </summary>
public class AddItemRequest
{
    /// <summary>Id del producto a agregar (tiene que existir en Products API).</summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    [Required(ErrorMessage = "El productoId es obligatorio")]
    public Guid ProductoId { get; set; }

    /// <summary>Cantidad de unidades. Tiene que ser mayor a 0.</summary>
    /// <example>2</example>
    [Range(1, int.MaxValue, ErrorMessage = "Cantidad inválida.")]
    public int Cantidad { get; set; }
}