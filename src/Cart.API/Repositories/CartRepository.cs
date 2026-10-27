using Cart.API.Models;

namespace Cart.API.Repositories;

// Guarda los carritos en una lista en memoria.
// Cuando llegue la librería de la cátedra, cambiamos solo esta clase.
public class CartRepository
{
    // La lista es privada: solo se modifica con los métodos de esta clase
    private List<ShoppingCart> carritos = new List<ShoppingCart>();

    // Busca el carrito de un usuario.
    // Si el usuario no tiene carrito, devuelve null (nada).
    public ShoppingCart ObtenerPorUsuario(Guid usuarioId)
    {
        foreach (ShoppingCart c in carritos)
        {
            if (c.UsuarioId == usuarioId)
            {
                return c;
            }
        }
        return null;
    }

    // Agrega un carrito nuevo a la lista
    public void Agregar(ShoppingCart carrito)
    {
        carritos.Add(carrito);
    }

    // Saca un carrito de la lista
    public void Eliminar(ShoppingCart carrito)
    {
        carritos.Remove(carrito);
    }
}