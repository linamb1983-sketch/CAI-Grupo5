using Products.API.DTOs;
using Products.API.Exceptions;
using Products.API.Models;
using Products.API.Repositories;

namespace Products.API.Services;

// Reglas del negocio de productos.
// El formato de los datos ya lo validó el Controller (Data Annotations).
public class ProductService
{
    private ProductRepository repositorio;

    public ProductService(ProductRepository repositorio)
    {
        this.repositorio = repositorio;
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

    public void Eliminar(Guid id)
    {
        Product producto = repositorio.ObtenerPorId(id);

        if (producto == null)
        {
            throw new NotFoundException(ErrorCodes.PRD_001, "Producto no encontrado.");
        }

        // PRD-004 (órdenes activas) queda pendiente hasta que exista Orders.API

        repositorio.Eliminar(producto);
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