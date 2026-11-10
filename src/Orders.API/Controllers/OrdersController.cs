using Microsoft.AspNetCore.Mvc;
using Orders.API.DTOs;
using Orders.API.Exceptions;
using Orders.API.Services;

namespace Orders.API.Controllers;

/// <summary>Recibe las solicitudes HTTP de órdenes y las deriva al servicio.</summary>
[ApiController]
[Route("api/orders")]
[Tags("Orders")]
[Produces("application/json")]
public class OrdersController : ControllerBase
{
    private OrderService servicio;

    // Recibe el servicio que contiene la lógica de órdenes.
    public OrdersController(OrderService servicio)
    {
        this.servicio = servicio;
    }

    /// <summary>Lista las órdenes, opcionalmente filtradas por usuario.</summary>
    /// <param name="usuarioId">Id del usuario a filtrar.</param>
    /// <response code="200">Lista de órdenes (puede estar vacía).</response>
    /// <response code="400">ORD-002: los datos enviados son inválidos.</response>
    /// <response code="500">ORD-007: error interno al procesar la orden.</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<OrderResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDatosInvalidos), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorInterno), StatusCodes.Status500InternalServerError)]
    public IActionResult ObtenerTodas(Guid? usuarioId)
    {
        ValidarDatos();

        List<OrderResponse> ordenes = servicio.ObtenerTodas(usuarioId);
        return Ok(ordenes);
    }

    /// <summary>Obtiene una orden por su Id.</summary>
    /// <param name="id">Id de la orden.</param>
    /// <response code="200">La orden encontrada.</response>
    /// <response code="404">ORD-001: orden no encontrada.</response>
    /// <response code="500">ORD-007: error interno al procesar la orden.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorOrdenNoEncontrada), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorInterno), StatusCodes.Status500InternalServerError)]
    public IActionResult ObtenerPorId(Guid id)
    {
        OrderResponse orden = servicio.ObtenerPorId(id);
        return Ok(orden);
    }

    /// <summary>Crea una orden luego de validar el usuario, productos y stock.</summary>
    /// <param name="request">Datos de la orden a crear.</param>
    /// <response code="201">Orden creada.</response>
    /// <response code="400">ORD-002: los datos enviados son inválidos.</response>
    /// <response code="404">ORD-003: usuario no encontrado; ORD-004: producto no encontrado.</response>
    /// <response code="422">ORD-005: stock insuficiente.</response>
    /// <response code="500">ORD-007: error interno al procesar la orden.</response>
    [HttpPost]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorDatosInvalidos), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorUsuarioNoEncontrado), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorProductoNoEncontrado), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorStockInsuficiente), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ErrorInterno), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Crear(CreateOrderRequest request)
    {
        ValidarDatos();

        OrderResponse orden = await servicio.Crear(request);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = orden.Id }, orden);
    }

    /// <summary>Modifica el estado de una orden si la transición está permitida.</summary>
    /// <param name="id">Id de la orden.</param>
    /// <param name="request">Nuevo estado de la orden.</param>
    /// <response code="200">Estado actualizado.</response>
    /// <response code="400">ORD-002: los datos enviados son inválidos.</response>
    /// <response code="404">ORD-001: orden no encontrada.</response>
    /// <response code="409">ORD-006: cambio de estado no permitido.</response>
    /// <response code="500">ORD-007: error interno al procesar la orden.</response>
    [HttpPut("{id}/status")]
    [ProducesResponseType(typeof(UpdateOrderStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDatosInvalidos), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorOrdenNoEncontrada), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorCambioEstadoNoPermitido), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorInterno), StatusCodes.Status500InternalServerError)]
    public IActionResult ModificarEstado(Guid id, UpdateOrderStatusRequest request)
    {
        ValidarDatos();

        if (request == null || request.Estado == null || request.Estado.Trim() == "")
        {
            throw new ValidationException(ErrorCodes.ORD_002, "Los datos de la orden son inválidos.");
        }

        UpdateOrderStatusResponse respuesta = servicio.ModificarEstado(id, request.Estado);
        return Ok(respuesta);
    }

    // Junta los errores del modelo y devuelve el código de validación de Orders.
    private void ValidarDatos()
    {
        if (ModelState.IsValid)
        {
            return;
        }

        string errores = "";
        bool jsonInvalido = false;

        foreach (var item in ModelState)
        {
            string campo = item.Key;

            foreach (var error in item.Value.Errors)
            {
                if (campo == "" || campo.StartsWith("$"))
                {
                    jsonInvalido = true;
                }
                else
                {
                    if (errores != "")
                    {
                        errores = errores + "; ";
                    }

                    errores = errores + error.ErrorMessage;
                }
            }
        }

        if (jsonInvalido)
        {
            errores = "El cuerpo del pedido no es un JSON válido.";
        }

        if (errores == "")
        {
            errores = "Los datos de la orden son inválidos.";
        }

        throw new ValidationException(ErrorCodes.ORD_002, errores);
    }
}
