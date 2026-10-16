using Microsoft.AspNetCore.Mvc;
using Products.API.DTOs;
using Products.API.Exceptions;
using Products.API.Services;

namespace Products.API.Controllers;

// Recibe los pedidos HTTP y se los pasa al Service.
[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private ProductService servicio;

    public ProductsController(ProductService servicio)
    {
        this.servicio = servicio;
    }

    // GET /api/products  (opcional: ?categoria=...&nombre=...)
    [HttpGet]
    public IActionResult ObtenerTodos(string? categoria, string? nombre)
    {
        List<ProductResponse> productos = servicio.ObtenerTodos(categoria, nombre);
        return Ok(productos);
    }

    // GET /api/products/{id}
    [HttpGet("{id}")]
    public IActionResult ObtenerPorId(Guid id)
    {
        ProductResponse producto = servicio.ObtenerPorId(id);
        return Ok(producto);
    }

    // POST /api/products
    [HttpPost]
    public IActionResult Crear(ProductRequest request)
    {
        ValidarDatos();

        ProductResponse producto = servicio.Crear(request);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = producto.Id }, producto);
    }

    // PUT /api/products/{id}
    [HttpPut("{id}")]
    public IActionResult Modificar(Guid id, ProductRequest request)
    {
        ValidarDatos();

        ProductResponse producto = servicio.Modificar(id, request);
        return Ok(producto);
    }

    // DELETE /api/products/{id}
    [HttpDelete("{id}")]
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