using Users.API.Models;

namespace Users.API.Repositories;

// Guarda los usuarios en una lista en memoria.
// Cuando llegue la librería de la cátedra, cambiamos solo esta clase.
public class UserRepository
{
    // La lista es privada: solo se modifica con los métodos de esta clase
    private List<User> usuarios = new List<User>();

    // Al arrancar, deja cargado un usuario bloqueado por fraude para poder mostrar USR-005.
    // La consigna no tiene un endpoint para bloquear usuarios a mano.
    public UserRepository()
    {
        User bloqueado = new User();
        bloqueado.Id = Guid.NewGuid();
        bloqueado.Nombre = "Usuario";
        bloqueado.Apellido = "Bloqueado";
        bloqueado.Email = "fraude@email.com";
        // Es el hash SHA256 de la contraseña "Fraude123!"
        bloqueado.PasswordHash = "E29C6C296EAE026308A12E17B2C3A9FB2F6142AB27F655F20DA524A08F47F52A";
        bloqueado.FechaRegistro = DateTime.UtcNow;
        bloqueado.Activo = false;
        bloqueado.IntentosFallidos = 0;
        usuarios.Add(bloqueado);
    }

    // Busca un usuario por su Id.
    // Si no existe, devuelve null (nada).
    public User? ObtenerPorId(Guid id)
    {
        foreach (User u in usuarios)
        {
            if (u.Id == id)
            {
                return u;
            }
        }
        return null;
    }

    // Busca un usuario por su email (sin importar mayúsculas o minúsculas).
    // Si no existe, devuelve null (nada).
    public User? ObtenerPorEmail(string email)
    {
        foreach (User u in usuarios)
        {
            if (u.Email.ToLower() == email.ToLower())
            {
                return u;
            }
        }
        return null;
    }

    // Agrega un usuario nuevo a la lista
    public void Agregar(User usuario)
    {
        usuarios.Add(usuario);
    }
}