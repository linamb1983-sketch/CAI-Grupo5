using Products.API.Models;

namespace Products.API.Repositories;

// Guarda los productos en una lista en memoria.
// Cuando llegue la librería de la cátedra, cambiamos solo esta clase.
public class ProductRepository
{
    // La lista es privada: solo se modifica con los métodos de esta clase
    private List<Product> productos = new List<Product>();

    // Devuelve todos los productos.
    // Devuelve una copia de la lista, así nadie de afuera puede agregar o sacar productos sin pasar por acá.
    public List<Product> ObtenerTodos()
    {
        List<Product> copia = new List<Product>();

        foreach (Product p in productos)
        {
            copia.Add(p);
        }

        return copia;
    }

    // Busca un producto por su Id.
    // Si no lo encuentra, devuelve null (nada).
    public Product ObtenerPorId(Guid id)
    {
        foreach (Product p in productos)
        {
            if (p.Id == id)
            {
                return p;
            }
        }
        return null;
    }

    // Agrega un producto a la lista
    public void Agregar(Product producto)
    {
        productos.Add(producto);
    }

    // Saca un producto de la lista
    public void Eliminar(Product producto)
    {
        productos.Remove(producto);
    }
}