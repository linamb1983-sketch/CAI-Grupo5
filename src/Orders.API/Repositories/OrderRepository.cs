using Orders.API.Models;

namespace Orders.API.Repositories;

// Guarda las órdenes en memoria hasta que llegue la librería de la cátedra.
public class OrderRepository
{
    private List<Order> ordenes = new List<Order>();

    // Devuelve todas las órdenes guardadas.
    public List<Order> ObtenerTodas()
    {
        return ordenes;
    }

    // Busca una orden por su identificador.
    public Order? ObtenerPorId(Guid id)
    {
        foreach (Order orden in ordenes)
        {
            if (orden.Id == id)
            {
                return orden;
            }
        }

        return null;
    }

    // Agrega una orden a la lista.
    public void Agregar(Order orden)
    {
        ordenes.Add(orden);
    }
}
