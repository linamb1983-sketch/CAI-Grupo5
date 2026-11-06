using System.ComponentModel.DataAnnotations;

namespace Cart.API.DTOs;

/// <summary>
/// Datos que manda el cliente para cambiar la cantidad de un producto (PUT).
/// El userId y el productId no van acá porque vienen en la ruta:
/// /api/cart/{userId}/items/{productId}
/// </summary>
public class UpdateItemRequest
{
    /// <summary>Nueva cantidad de unidades. Tiene que ser mayor a 0.</summary>
    /// <example>4</example>
    [Range(1, int.MaxValue, ErrorMessage = "Cantidad inválida.")]
    public int Cantidad { get; set; }
}