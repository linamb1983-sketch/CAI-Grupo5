using System.Net;
using System.Net.Http.Json;
using Orders.API.DTOs;
using Orders.API.Exceptions;
using Orders.API.Models;
using Orders.API.Repositories;

namespace Orders.API.Services;

// Contiene las reglas de negocio de las órdenes.
public class OrderService
{
    private OrderRepository repositorio;
    private IHttpClientFactory httpClientFactory;
    private IConfiguration configuracion;

    // Recibe el repositorio, la fábrica HTTP y la configuración de servicios.
    public OrderService(OrderRepository repositorio, IHttpClientFactory httpClientFactory, IConfiguration configuracion)
    {
        this.repositorio = repositorio;
        this.httpClientFactory = httpClientFactory;
        this.configuracion = configuracion;
    }

    // Devuelve todas las órdenes o las de un usuario.
    public List<OrderResponse> ObtenerTodas(Guid? usuarioId)
    {
        List<OrderResponse> respuesta = new List<OrderResponse>();

        foreach (Order orden in repositorio.ObtenerTodas())
        {
            if (usuarioId == null || orden.UsuarioId == usuarioId.Value)
            {
                respuesta.Add(ConvertirAResponse(orden));
            }
        }

        return respuesta;
    }

    // Devuelve una orden o informa que no existe.
    public OrderResponse ObtenerPorId(Guid id)
    {
        Order? orden = repositorio.ObtenerPorId(id);

        if (orden == null)
        {
            throw new NotFoundException(ErrorCodes.ORD_001, "Orden no encontrada.");
        }

        return ConvertirAResponse(orden);
    }

    // Valida referencias y stock antes de guardar la orden.
    public async Task<OrderResponse> Crear(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        if (request.UsuarioId == Guid.Empty || request.Items == null || request.Items.Count == 0)
        {
            throw new ValidationException(ErrorCodes.ORD_002, "Los datos de la orden son inválidos.");
        }

        await ValidarUsuario(request.UsuarioId, cancellationToken);

        List<ProductLookupResponse> productos = new List<ProductLookupResponse>();
        List<Guid> productosConsultados = new List<Guid>();
        List<OrderItem> items = new List<OrderItem>();
        decimal total = 0;

        foreach (OrderItemRequest itemRequest in request.Items)
        {
            if (itemRequest.ProductoId == Guid.Empty || itemRequest.Cantidad <= 0)
            {
                throw new ValidationException(ErrorCodes.ORD_002, "Los datos de la orden son inválidos.");
            }

            ProductLookupResponse? producto = BuscarProductoEnLista(productos, productosConsultados, itemRequest.ProductoId);

            if (producto == null)
            {
                producto = await ObtenerProducto(itemRequest.ProductoId, cancellationToken);
                productosConsultados.Add(itemRequest.ProductoId);
                productos.Add(producto);
            }

            int cantidadSolicitada = 0;
            foreach (OrderItemRequest otroItem in request.Items)
            {
                if (otroItem.ProductoId == itemRequest.ProductoId)
                {
                    cantidadSolicitada += otroItem.Cantidad;
                }
            }

            if (producto.Stock < cantidadSolicitada)
            {
                throw new BusinessRuleException(
                    ErrorCodes.ORD_005,
                    $"Stock insuficiente para '{producto.Nombre}'. Disponible: {producto.Stock}, solicitado: {cantidadSolicitada}.");
            }

            OrderItem item = new OrderItem
            {
                ProductoId = itemRequest.ProductoId,
                Cantidad = itemRequest.Cantidad,
                PrecioUnitario = producto.Precio
            };

            items.Add(item);
            total += item.PrecioUnitario * item.Cantidad;
        }

        Order orden = new Order
        {
            Id = Guid.NewGuid(),
            UsuarioId = request.UsuarioId,
            Items = items,
            Total = total,
            Estado = "Pendiente",
            FechaCreacion = DateTime.UtcNow
        };

        repositorio.Agregar(orden);

        return ConvertirAResponse(orden);
    }

    // Cambia el estado solo si la transición está permitida.
    public OrderResponse ModificarEstado(Guid id, string estadoNuevo)
    {
        Order? orden = repositorio.ObtenerPorId(id);

        if (orden == null)
        {
            throw new NotFoundException(ErrorCodes.ORD_001, "Orden no encontrada.");
        }

        if (!PuedeCambiarEstado(orden.Estado, estadoNuevo))
        {
            throw new BusinessRuleException(ErrorCodes.ORD_006, "El estado de la orden no puede ser modificado.");
        }

        orden.Estado = estadoNuevo;

        return ConvertirAResponse(orden);
    }

    // Consulta Users API y traduce un 404 al código de Orders.
    private async Task ValidarUsuario(Guid usuarioId, CancellationToken cancellationToken)
    {
        string direccion = ObtenerDireccionBase("Services:Users:BaseUrl");
        string url = $"{direccion}/api/users/{usuarioId}";
        HttpClient cliente = httpClientFactory.CreateClient();

        using HttpResponseMessage respuesta = await cliente.GetAsync(url, cancellationToken);

        if (respuesta.StatusCode == HttpStatusCode.NotFound)
        {
            throw new NotFoundException(ErrorCodes.ORD_003, "Usuario no encontrado al crear la orden.");
        }

        if (!respuesta.IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"Users API respondió con HTTP {(int)respuesta.StatusCode}.");
        }
    }

    // Consulta Products API y obtiene precio y stock del producto.
    private async Task<ProductLookupResponse> ObtenerProducto(Guid productoId, CancellationToken cancellationToken)
    {
        string direccion = ObtenerDireccionBase("Services:Products:BaseUrl");
        string url = $"{direccion}/api/products/{productoId}";
        HttpClient cliente = httpClientFactory.CreateClient();

        using HttpResponseMessage respuesta = await cliente.GetAsync(url, cancellationToken);

        if (respuesta.StatusCode == HttpStatusCode.NotFound)
        {
            throw new NotFoundException(ErrorCodes.ORD_004, "Producto no encontrado al crear la orden.");
        }

        if (!respuesta.IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"Products API respondió con HTTP {(int)respuesta.StatusCode}.");
        }

        ProductLookupResponse? producto = await respuesta.Content.ReadFromJsonAsync<ProductLookupResponse>(cancellationToken);

        if (producto == null)
        {
            throw new InvalidOperationException("Products API devolvió una respuesta vacía.");
        }

        return producto;
    }

    // Busca un producto ya consultado durante la misma creación.
    private ProductLookupResponse? BuscarProductoEnLista(List<ProductLookupResponse> productos, List<Guid> ids, Guid productoId)
    {
        for (int i = 0; i < ids.Count; i++)
        {
            if (ids[i] == productoId)
            {
                return productos[i];
            }
        }

        return null;
    }

    // Lee una URL de servicio y falla explícitamente si está mal configurada.
    private string ObtenerDireccionBase(string clave)
    {
        string? direccion = configuracion[clave];

        if (string.IsNullOrWhiteSpace(direccion) || !Uri.TryCreate(direccion, UriKind.Absolute, out Uri? uri))
        {
            throw new InvalidOperationException($"La configuración '{clave}' no contiene una URL válida.");
        }

        return uri.ToString().TrimEnd('/');
    }

    // Define los cambios de estado permitidos para una orden.
    private bool PuedeCambiarEstado(string estadoActual, string estadoNuevo)
    {
        if (estadoActual == "Pendiente" && (estadoNuevo == "Confirmada" || estadoNuevo == "Cancelada"))
        {
            return true;
        }

        if (estadoActual == "Confirmada" && (estadoNuevo == "Enviada" || estadoNuevo == "Cancelada"))
        {
            return true;
        }

        if (estadoActual == "Enviada" && estadoNuevo == "Entregada")
        {
            return true;
        }

        return false;
    }

    // Convierte la entidad en la respuesta pública.
    private OrderResponse ConvertirAResponse(Order orden)
    {
        OrderResponse respuesta = new OrderResponse
        {
            Id = orden.Id,
            UsuarioId = orden.UsuarioId,
            Total = orden.Total,
            Estado = orden.Estado,
            FechaCreacion = orden.FechaCreacion
        };

        foreach (OrderItem item in orden.Items)
        {
            respuesta.Items.Add(new OrderItemResponse
            {
                ProductoId = item.ProductoId,
                Cantidad = item.Cantidad,
                PrecioUnitario = item.PrecioUnitario
            });
        }

        return respuesta;
    }

    // Campos que Orders necesita leer de Products API.
    // Datos mínimos que Orders necesita leer de Products API.
    private class ProductLookupResponse
    {
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
    }
}
