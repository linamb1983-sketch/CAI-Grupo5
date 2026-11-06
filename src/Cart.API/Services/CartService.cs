using System.Net;
using System.Net.Http.Json;
using Cart.API.DTOs;
using Cart.API.Exceptions;
using Cart.API.Models;
using Cart.API.Repositories;

namespace Cart.API.Services;

// Reglas de negocio del carrito.
// Para validar productos y stock, le pregunta a Products API por HTTP.
public class CartService
{
    private CartRepository repositorio;
    private IHttpClientFactory httpClientFactory;
    private IConfiguration configuracion;

    // Recibe el repositorio, la fábrica de clientes HTTP y la configuración (appsettings.json)
    public CartService(CartRepository repositorio, IHttpClientFactory httpClientFactory, IConfiguration configuracion)
    {
        this.repositorio = repositorio;
        this.httpClientFactory = httpClientFactory;
        this.configuracion = configuracion;
    }

    // GET: devuelve el carrito del usuario. Si no tiene carrito, CRT-001.
    public CartResponse ObtenerCarrito(Guid usuarioId)
    {
        ShoppingCart carrito = repositorio.ObtenerPorUsuario(usuarioId);

        if (carrito == null)
        {
            throw new NotFoundException(ErrorCodes.CRT_001, "Carrito no encontrado.");
        }

        return ConvertirAResponse(carrito);
    }

    // POST: agrega un producto al carrito.
    // Si el usuario no tiene carrito, se crea. Si el producto ya estaba, se suma la cantidad.
    public async Task<CartResponse> AgregarItem(Guid usuarioId, AddItemRequest request)
    {
        // CRT-004: la cantidad tiene que ser mayor a 0
        if (request.Cantidad <= 0)
        {
            throw new ValidationException(ErrorCodes.CRT_004, "Cantidad inválida.");
        }

        // CRT-002: el producto tiene que existir en Products
        ProductInfo producto = await ConsultarProducto(request.ProductoId);

        // Busca el carrito. Si no tiene, arma uno nuevo (todavía no se guarda)
        ShoppingCart carrito = repositorio.ObtenerPorUsuario(usuarioId);
        bool esNuevo = false;

        if (carrito == null)
        {
            carrito = new ShoppingCart();
            carrito.UsuarioId = usuarioId;
            esNuevo = true;
        }

        // Calcula cuántas unidades quedarían en total (lo que ya había + lo nuevo)
        CartItem itemExistente = BuscarItem(carrito, request.ProductoId);
        int cantidadTotal = request.Cantidad;

        if (itemExistente != null)
        {
            cantidadTotal = itemExistente.Cantidad + request.Cantidad;
        }

        // CRT-003: no puede superar el stock de Products
        if (producto.Stock < cantidadTotal)
        {
            throw new BusinessRuleException(ErrorCodes.CRT_003,
                $"Stock insuficiente. Disponible: {producto.Stock}, solicitado: {cantidadTotal}.");
        }

        // Pasó todas las reglas: actualiza o agrega el renglón
        if (itemExistente != null)
        {
            itemExistente.Cantidad = cantidadTotal;
        }
        else
        {
            CartItem nuevoItem = new CartItem();
            nuevoItem.ProductoId = request.ProductoId;
            nuevoItem.Cantidad = request.Cantidad;
            carrito.Items.Add(nuevoItem);
        }

        carrito.FechaActualizacion = DateTime.UtcNow;

        // Recién ahora, si era nuevo, se guarda en el repositorio
        if (esNuevo)
        {
            repositorio.Agregar(carrito);
        }

        return ConvertirAResponse(carrito);
    }

    // PUT: cambia la cantidad de un producto que ya está en el carrito.
    public async Task<CartResponse> ActualizarCantidad(Guid usuarioId, Guid productoId, UpdateItemRequest request)
    {
        // CRT-004: la cantidad tiene que ser mayor a 0
        if (request.Cantidad <= 0)
        {
            throw new ValidationException(ErrorCodes.CRT_004, "Cantidad inválida.");
        }

        // CRT-001: el usuario tiene que tener carrito
        ShoppingCart carrito = repositorio.ObtenerPorUsuario(usuarioId);

        if (carrito == null)
        {
            throw new NotFoundException(ErrorCodes.CRT_001, "Carrito no encontrado.");
        }

        // CRT-002: el producto tiene que estar en el carrito
        CartItem item = BuscarItem(carrito, productoId);

        if (item == null)
        {
            throw new NotFoundException(ErrorCodes.CRT_002, "Producto no encontrado en el carrito.");
        }

        // CRT-002: el producto tiene que seguir existiendo en Products
        ProductInfo producto = await ConsultarProducto(productoId);

        // CRT-003: la nueva cantidad no puede superar el stock
        if (producto.Stock < request.Cantidad)
        {
            throw new BusinessRuleException(ErrorCodes.CRT_003,
                $"Stock insuficiente. Disponible: {producto.Stock}, solicitado: {request.Cantidad}.");
        }

        item.Cantidad = request.Cantidad;
        carrito.FechaActualizacion = DateTime.UtcNow;

        return ConvertirAResponse(carrito);
    }

    // DELETE: saca un producto del carrito.
    public void QuitarItem(Guid usuarioId, Guid productoId)
    {
        // CRT-001: el usuario tiene que tener carrito
        ShoppingCart carrito = repositorio.ObtenerPorUsuario(usuarioId);

        if (carrito == null)
        {
            throw new NotFoundException(ErrorCodes.CRT_001, "Carrito no encontrado.");
        }

        // CRT-002: el producto tiene que estar en el carrito
        CartItem item = BuscarItem(carrito, productoId);

        if (item == null)
        {
            throw new NotFoundException(ErrorCodes.CRT_002, "Producto no encontrado en el carrito.");
        }

        carrito.Items.Remove(item);
        carrito.FechaActualizacion = DateTime.UtcNow;
    }

    // Le pregunta a Products API por un producto. Si no existe, CRT-002.
    private async Task<ProductInfo> ConsultarProducto(Guid productoId)
    {
        // Arma la dirección: http://localhost:5001/api/products/{id}
        string direccion = configuracion["Services:Products:BaseUrl"];
        string url = $"{direccion}/api/products/{productoId}";

        HttpClient cliente = httpClientFactory.CreateClient();
        HttpResponseMessage respuesta = await cliente.GetAsync(url);

        // Primero se mira el código HTTP, antes de leer la respuesta (lo pide la consigna)
        if (respuesta.StatusCode == HttpStatusCode.NotFound)
        {
            throw new NotFoundException(ErrorCodes.CRT_002, "Producto no encontrado.");
        }

        // Cualquier otro error de Products es inesperado: lo ataja el GlobalExceptionHandler (500, CRT-005)
        if (respuesta.IsSuccessStatusCode == false)
        {
            throw new Exception($"Products API respondió con error {(int)respuesta.StatusCode}.");
        }

        ProductInfo producto = await respuesta.Content.ReadFromJsonAsync<ProductInfo>();
        return producto;
    }

    // Busca un producto dentro del carrito. Si no está, devuelve null.
    private CartItem BuscarItem(ShoppingCart carrito, Guid productoId)
    {
        foreach (CartItem item in carrito.Items)
        {
            if (item.ProductoId == productoId)
            {
                return item;
            }
        }
        return null;
    }

    // Pasa el carrito de adentro (Model) al formulario de salida (DTO)
    private CartResponse ConvertirAResponse(ShoppingCart carrito)
    {
        CartResponse respuesta = new CartResponse();
        respuesta.UsuarioId = carrito.UsuarioId;
        respuesta.FechaActualizacion = carrito.FechaActualizacion;

        foreach (CartItem item in carrito.Items)
        {
            CartItemResponse itemResponse = new CartItemResponse();
            itemResponse.ProductoId = item.ProductoId;
            itemResponse.Cantidad = item.Cantidad;
            respuesta.Items.Add(itemResponse);
        }

        return respuesta;
    }
}
