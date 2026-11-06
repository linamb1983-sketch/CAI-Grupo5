using Microsoft.AspNetCore.Mvc;
using Cart.API.DTOs;
using Cart.API.Exceptions;
using Cart.API.Services;

namespace Cart.API.Controllers;

/// <summary>
/// Recibe los pedidos HTTP del carrito y se los pasa al Service.
/// </summary>
[ApiController]
[Route("api/cart")]
[Tags("Cart")]
public class CartController : ControllerBase
{
    private CartService servicio;

    public CartController(CartService servicio)
    {
        this.servicio = servicio;
    }

    /// <summary>
    /// Obtiene el carrito de un usuario.
    /// </summary>
    /// <param name="userId">Id del usuario.</param>
    /// <response code="200">El carrito del usuario.</response>
    /// <response code="404">CRT-001: Carrito no encontrado.</response>
    /// <response code="500">CRT-005: Error interno al procesar el carrito.</response>
    [HttpGet("{userId}")]
    [ProducesResponseType(typeof(CartResponse), StatusCodes.Status200OK)]
    public IActionResult ObtenerCarrito(Guid userId)
    {
        CartResponse carrito = servicio.ObtenerCarrito(userId);
        return Ok(carrito);
    }

    /// <summary>
    /// Agrega un producto al carrito. Si el usuario no tiene carrito, se crea.
    /// </summary>
    /// <param name="userId">Id del usuario.</param>
    /// <param name="request">Producto y cantidad a agregar.</param>
    /// <response code="200">El carrito actualizado.</response>
    /// <response code="400">CRT-004: Cantidad inválida.</response>
    /// <response code="404">CRT-002: Producto no encontrado.</response>
    /// <response code="422">CRT-003: Stock insuficiente para agregar al carrito.</response>
    /// <response code="500">CRT-005: Error interno al procesar el carrito.</response>
    [HttpPost("{userId}/items")]
    [ProducesResponseType(typeof(CartResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> AgregarItem(Guid userId, AddItemRequest request)
    {
        ValidarDatos();

        CartResponse carrito = await servicio.AgregarItem(userId, request);
        return Ok(carrito);
    }

    /// <summary>
    /// Cambia la cantidad de un producto que ya está en el carrito.
    /// </summary>
    /// <param name="userId">Id del usuario.</param>
    /// <param name="productId">Id del producto.</param>
    /// <param name="request">Nueva cantidad.</param>
    /// <response code="200">El carrito actualizado.</response>
    /// <response code="400">CRT-004: Cantidad inválida.</response>
    /// <response code="404">CRT-001: Carrito no encontrado. CRT-002: Producto no encontrado.</response>
    /// <response code="422">CRT-003: Stock insuficiente para agregar al carrito.</response>
    /// <response code="500">CRT-005: Error interno al procesar el carrito.</response>
    [HttpPut("{userId}/items/{productId}")]
    [ProducesResponseType(typeof(CartResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> ActualizarCantidad(Guid userId, Guid productId, UpdateItemRequest request)
    {
        ValidarDatos();

        CartResponse carrito = await servicio.ActualizarCantidad(userId, productId, request);
        return Ok(carrito);
    }

    /// <summary>
    /// Saca un producto del carrito.
    /// </summary>
    /// <param name="userId">Id del usuario.</param>
    /// <param name="productId">Id del producto a sacar.</param>
    /// <response code="204">Producto quitado (sin contenido).</response>
    /// <response code="404">CRT-001: Carrito no encontrado. CRT-002: Producto no encontrado.</response>
    /// <response code="500">CRT-005: Error interno al procesar el carrito.</response>
    [HttpDelete("{userId}/items/{productId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult QuitarItem(Guid userId, Guid productId)
    {
        servicio.QuitarItem(userId, productId);
        return NoContent();
    }

    // Si los datos no cumplen las reglas del DTO, junta todos los errores
    // separados por punto y coma y corta con CRT-004.
    private void ValidarDatos()
    {
        if (ModelState.IsValid)
        {
            return;
        }

        string errores = "";

        foreach (var campo in ModelState.Values)
        {
            foreach (var error in campo.Errors)
            {
                errores = errores + error.ErrorMessage + "; ";
            }
        }

        if (errores == "")
        {
            errores = "Cantidad inválida.";
        }

        throw new ValidationException(ErrorCodes.CRT_004, errores);
    }
}