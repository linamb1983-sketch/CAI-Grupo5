using Microsoft.AspNetCore.Mvc;
using Products.API.DTOs;
using Products.API.Exceptions;
using Products.API.Services;

namespace Products.API.Controllers;

/// <summary>
/// Recibe los pedidos HTTP de productos y se los pasa al Service.
/// </summary>
[ApiController]
[Route("api/products")]
[Tags("Products")]
public class ProductsController : ControllerBase
{
    private ProductService servicio;

    public ProductsController(ProductService servicio)
    {
        this.servicio = servicio;
    }

    /// <summary>
    /// Lista los productos. Se puede filtrar por categoría y por nombre.
    /// </summary>
    /// <param name="categoria">Opcional. Muestra solo los productos de esa categoría.</param>
    /// <param name="nombre">Opcional. Muestra solo los productos cuyo nombre contiene ese texto.</param>
    /// <response code="200">Lista de productos (puede estar vacía).</response>
    /// <response code="500">PRD-005: Error interno al procesar el producto.</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<ProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorInterno), StatusCodes.Status500InternalServerError)]
    public IActionResult ObtenerTodos(string? categoria, string? nombre)
    {
        List<ProductResponse> productos = servicio.ObtenerTodos(categoria, nombre);
        return Ok(productos);
    }

    /// <summary>
    /// Obtiene un producto por su Id.
    /// </summary>
    /// <param name="id">Id del producto.</param>
    /// <response code="200">El producto encontrado.</response>
    /// <response code="404">PRD-001: Producto no encontrado.</response>
    /// <response code="500">PRD-005: Error interno al procesar el producto.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorProductoNoEncontrado), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorInterno), StatusCodes.Status500InternalServerError)]
    public IActionResult ObtenerPorId(Guid id)
    {
        ProductResponse producto = servicio.ObtenerPorId(id);
        return Ok(producto);
    }

    /// <summary>
    /// Crea un producto nuevo.
    /// </summary>
    /// <param name="request">Datos del producto a crear.</param>
    /// <response code="201">Producto creado.</response>
    /// <response code="400">PRD-002: Los datos del producto son inválidos.</response>
    /// <response code="409">PRD-003: Ya existe un producto con ese nombre en la categoría.</response>
    /// <response code="500">PRD-005: Error interno al procesar el producto.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorDatosInvalidos), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorProductoDuplicado), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorInterno), StatusCodes.Status500InternalServerError)]
    public IActionResult Crear(ProductRequest request)
    {
        ValidarDatos();

        ProductResponse producto = servicio.Crear(request);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = producto.Id }, producto);
    }

    /// <summary>
    /// Modifica un producto que ya existe.
    /// </summary>
    /// <param name="id">Id del producto a modificar.</param>
    /// <param name="request">Datos nuevos del producto.</param>
    /// <response code="200">Producto modificado.</response>
    /// <response code="400">PRD-002: Los datos del producto son inválidos.</response>
    /// <response code="404">PRD-001: Producto no encontrado.</response>
    /// <response code="500">PRD-005: Error interno al procesar el producto.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDatosInvalidos), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorProductoNoEncontrado), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorInterno), StatusCodes.Status500InternalServerError)]
    public IActionResult Modificar(Guid id, ProductRequest request)
    {
        ValidarDatos();

        ProductResponse producto = servicio.Modificar(id, request);
        return Ok(producto);
    }

    /// <summary>
    /// Elimina un producto.
    /// </summary>
    /// <param name="id">Id del producto a eliminar.</param>
    /// <response code="204">Producto eliminado (sin contenido).</response>
    /// <response code="404">PRD-001: Producto no encontrado.</response>
    /// <response code="409">PRD-004: El producto tiene órdenes activas y no puede eliminarse.</response>
    /// <response code="500">PRD-005: Error interno al procesar el producto.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorProductoNoEncontrado), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorProductoConOrdenes), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorInterno), StatusCodes.Status500InternalServerError)]
    public IActionResult Eliminar(Guid id)
    {
        servicio.Eliminar(id);
        return NoContent();
    }

    // Si los datos no cumplen las reglas del DTO, junta todos los errores
    // separados por punto y coma y corta con PRD-002.
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
            errores = "Los datos del producto son inválidos.";
        }

        throw new ValidationException(ErrorCodes.PRD_002, errores);
    }
}