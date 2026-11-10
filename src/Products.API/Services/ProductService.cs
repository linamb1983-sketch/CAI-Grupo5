using System.Net.Http.Json;
using Products.API.DTOs;
using Products.API.Exceptions;
using Products.API.Models;
using Products.API.Repositories;

namespace Products.API.Services;

// Reglas del negocio de productos.
// El formato de los datos ya lo validó el Controller (Data Annotations).
// Para borrar un producto, le pregunta a Orders API por HTTP si tiene órdenes activas.
public class ProductService
{
    private ProductRepository repositorio;
    private IHttpClientFactory httpClientFactory;
    private IConfiguration configuracion;
    private IHttpContextAccessor httpContextAccessor;

    // Recibe el repositorio, la fábrica de clientes HTTP, la configuración (appsettings.json)
    // y el acceso al pedido actual (para leer el Correlation ID)
    public ProductService(ProductRepository repositorio, IHttpClientFactory httpClientFactory, IConfiguration configuracion, IHttpContextAccessor httpContextAccessor)
    {
        this.repositorio = repositorio;
        this.httpClientFactory = httpClientFactory;
        this.configuracion = configuracion;
        this.httpContextAccessor = httpContextAccessor;
    }

    // Devuelve los productos. Si mandan categoria o nombre, filtra.
    public List<ProductResponse> ObtenerTodos(string? categoria, string? nombre)
    {
        List<ProductResponse> respuesta = new List<ProductResponse>();

        foreach (Product p in repositorio.ObtenerTodos())
        {
            bool coincide = true;

            if (categoria != null && p.Categoria != categoria)
            {
                coincide = false;
            }

            if (nombre != null && !p.Nombre.Contains(nombre))
            {
                coincide = false;
            }

            if (coincide)
            {
                respuesta.Add(ConvertirAResponse(p));
            }
        }

        return respuesta;
    }

    public ProductResponse ObtenerPorId(Guid id)
    {
        Product producto = repositorio.ObtenerPorId(id);

        if (producto == null)
        {
            throw new NotFoundException(ErrorCodes.PRD_001, "Producto no encontrado.");
        }

        return ConvertirAResponse(producto);
    }

    public ProductResponse Crear(ProductRequest request)
    {
        // No puede haber dos productos con el mismo nombre en la misma categoría
        foreach (Product p in repositorio.ObtenerTodos())
        {
            if (p.Nombre == request.Nombre && p.Categoria == request.Categoria)
            {
                throw new BusinessRuleException(ErrorCodes.PRD_003, $"Ya existe un producto con ese nombre en la categoría '{request.Categoria}'.");
            }
        }

        Product producto = new Product
        {
            Id = Guid.NewGuid(),
            Nombre = request.Nombre,
            Descripcion = request.Descripcion,
            Precio = request.Precio,
            Stock = request.Stock,
            Categoria = request.Categoria,
            FechaCreacion = DateTime.UtcNow
        };

        repositorio.Agregar(producto);

        return ConvertirAResponse(producto);
    }

    public ProductResponse Modificar(Guid id, ProductRequest request)
    {
        Product producto = repositorio.ObtenerPorId(id);

        if (producto == null)
        {
            throw new NotFoundException(ErrorCodes.PRD_001, "Producto no encontrado.");
        }

        producto.Nombre = request.Nombre;
        producto.Descripcion = request.Descripcion;
        producto.Precio = request.Precio;
        producto.Stock = request.Stock;
        producto.Categoria = request.Categoria;

        return ConvertirAResponse(producto);
    }

    // DELETE: borra el producto, salvo que esté en una orden activa (PRD-004).
    public async Task Eliminar(Guid id)
    {
        Product producto = repositorio.ObtenerPorId(id);

        if (producto == null)
        {
            throw new NotFoundException(ErrorCodes.PRD_001, "Producto no encontrado.");
        }

        // PRD-004: si alguna orden activa tiene este producto, no se puede borrar
        bool tieneOrdenes = await TieneOrdenesActivas(id);

        if (tieneOrdenes)
        {
            throw new BusinessRuleException(ErrorCodes.PRD_004, "El producto tiene órdenes activas y no puede eliminarse.");
        }

        repositorio.Eliminar(producto);
    }

    // Le pide a Orders API todas las órdenes y se fija si alguna activa
    // (Pendiente o Confirmada) tiene este producto.
    // Si Orders no responde o responde con error, sale una excepción: la ataja el
    // GlobalExceptionHandler (500, PRD-005) y el producto NO se borra.
    private async Task<bool> TieneOrdenesActivas(Guid productoId)
    {
        // Arma la dirección: http://localhost:5003/api/orders
        string direccion = configuracion["Services:Orders:BaseUrl"];
        string url = $"{direccion}/api/orders";

        HttpClient cliente = CrearClienteConCorrelationId();
        HttpResponseMessage respuesta = await cliente.GetAsync(url);

        // Primero se mira el código HTTP, antes de leer la respuesta
        if (respuesta.IsSuccessStatusCode == false)
        {
            throw new Exception($"Orders API respondió con error {(int)respuesta.StatusCode}.");
        }

        List<OrderInfo> ordenes = await respuesta.Content.ReadFromJsonAsync<List<OrderInfo>>();

        foreach (OrderInfo orden in ordenes)
        {
            // Solo cuentan las órdenes que todavía no salieron ni se cancelaron
            if (orden.Estado == "Pendiente" || orden.Estado == "Confirmada")
            {
                foreach (OrderItemInfo item in orden.Items)
                {
                    if (item.ProductoId == productoId)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    // Crea el cliente HTTP y le agrega el Correlation ID del pedido actual,
    // así Orders usa el mismo número y el pedido se puede seguir en los logs de los dos servicios.
    private HttpClient CrearClienteConCorrelationId()
    {
        HttpClient cliente = httpClientFactory.CreateClient();

        HttpContext context = httpContextAccessor.HttpContext;

        if (context != null)
        {
            string correlationId = $"{context.Items["CorrelationId"]}";

            if (correlationId != "")
            {
                cliente.DefaultRequestHeaders.Add("X-Correlation-Id", correlationId);
            }
        }

        return cliente;
    }

    // Pasa del Model (lo que guardamos) al Response (lo que mostramos)
    private ProductResponse ConvertirAResponse(Product p)
    {
        return new ProductResponse
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Descripcion = p.Descripcion,
            Precio = p.Precio,
            Stock = p.Stock,
            Categoria = p.Categoria,
            FechaCreacion = p.FechaCreacion
        };
    }
}