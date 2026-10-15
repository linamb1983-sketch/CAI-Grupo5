using Products.API.DTOs;
using Products.API.Exceptions;
using Products.API.Models;
using Products.API.Repositories;

namespace Products.API.Services;

// Reglas del negocio de productos
public class ProductService
{
    private ProductRepository repositorio;

    public ProductService(ProductRepository repositorio)
    {
        this.repositorio = repositorio;
    }

    // Devuelve los productos. Si mandan categoria o nombre, filtra (lo pide la consigna).
    public List<ProductResponse> ObtenerTodos(string categoria, string nombre)
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
            throw new NotFoundException("PRD-001", "Producto no encontrado.");
        }

        return ConvertirAResponse(producto);
    }

    public ProductResponse Crear(ProductRequest request)
    {
        Validar(request);

        // No puede haber dos productos con el mismo nombre en la misma categoría
        foreach (Product p in repositorio.ObtenerTodos())
        {
            if (p.Nombre == request.Nombre && p.Categoria == request.Categoria)
            {
                throw new BusinessRuleException("PRD-003", $"Ya existe un producto con ese nombre en la categoría '{request.Categoria}'.");
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
            FechaCreacion = DateTime.Now
        };

        repositorio.Agregar(producto);

        return ConvertirAResponse(producto);
    }

    public ProductResponse Modificar(Guid id, ProductRequest request)
    {
        Product producto = repositorio.ObtenerPorId(id);

        if (producto == null)
        {
            throw new NotFoundException("PRD-001", "Producto no encontrado.");
        }

        Validar(request);

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
            throw new NotFoundException("PRD-001", "Producto no encontrado.");
        }

        repositorio.Eliminar(producto);
    }

    // Junta todos los errores y, si hay alguno, corta con PRD-002.
    // La consigna dice que se pueden listar separados por punto y coma.
    private void Validar(ProductRequest request)
    {
        string errores = "";

        if (request.Nombre == null || request.Nombre == "")
        {
            errores = errores + "El nombre es obligatorio; ";
        }
        else if (request.Nombre.Length > 100)
        {
            errores = errores + "El nombre no puede superar los 100 caracteres; ";
        }

        if (request.Descripcion != null && request.Descripcion.Length > 500)
        {
            errores = errores + "La descripción no puede superar los 500 caracteres; ";
        }

        if (request.Precio <= 0)
        {
            errores = errores + "El precio debe ser mayor a cero; ";
        }

        if (request.Stock < 0)
        {
            errores = errores + "El stock no puede ser negativo; ";
        }

        if (request.Categoria == null || request.Categoria == "")
        {
            errores = errores + "La categoría es obligatoria; ";
        }

        if (errores != "")
        {
            throw new ValidationException("PRD-002", errores);
        }
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