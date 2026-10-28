using Microsoft.AspNetCore.Mvc;
using Orders.API.DTOs;
using Orders.API.Exceptions;
using Orders.API.Services;

namespace Orders.API.Controllers;

// Recibe las solicitudes HTTP de órdenes y las deriva al servicio.
[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private OrderService servicio;

    // Recibe el servicio que contiene la lógica de órdenes.
    public OrdersController(OrderService servicio)
    {
        this.servicio = servicio;
    }

    // GET /api/orders?usuarioId=...
    [HttpGet]
    public IActionResult ObtenerTodas(Guid? usuarioId)
    {
        ValidarDatos();

        List<OrderResponse> ordenes = servicio.ObtenerTodas(usuarioId);
        return Ok(ordenes);
    }

    // GET /api/orders/{id}
    [HttpGet("{id}")]
    public IActionResult ObtenerPorId(Guid id)
    {
        OrderResponse orden = servicio.ObtenerPorId(id);
        return Ok(orden);
    }

    // POST /api/orders
    [HttpPost]
    public async Task<IActionResult> Crear(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        ValidarDatos();

        OrderResponse orden = await servicio.Crear(request, cancellationToken);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = orden.Id }, orden);
    }

    // PUT /api/orders/{id}/status
    [HttpPut("{id}/status")]
    public IActionResult ModificarEstado(Guid id, UpdateOrderStatusRequest request)
    {
        ValidarDatos();

        if (request == null || string.IsNullOrWhiteSpace(request.Estado))
        {
            throw new ValidationException(ErrorCodes.ORD_002, "Los datos de la orden son inválidos.");
        }

        OrderResponse orden = servicio.ModificarEstado(id, request.Estado);
        return Ok(orden);
    }

    // Junta los errores del modelo y devuelve el código de validación de Orders.
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
            errores = "Los datos de la orden son inválidos.";
        }

        throw new ValidationException(ErrorCodes.ORD_002, errores);
    }
}
