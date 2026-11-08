using System.Security.Cryptography;
using System.Text;
using Users.API.DTOs;
using Users.API.Exceptions;
using Users.API.Models;
using Users.API.Repositories;

namespace Users.API.Services;

// Reglas de negocio de los usuarios: registrar, iniciar sesión y buscar por Id.
public class UserService
{
    // Cantidad de intentos fallidos seguidos que bloquean la cuenta (lo pide la consigna)
    private const int MaximoIntentosFallidos = 3;

    private UserRepository repositorio;

    public UserService(UserRepository repositorio)
    {
        this.repositorio = repositorio;
    }

    // POST register: crea un usuario nuevo.
    public UserResponse Registrar(RegisterRequest request)
    {
        // USR-001: el email no puede estar repetido
        User? existente = repositorio.ObtenerPorEmail(request.Email);

        if (existente != null)
        {
            throw new BusinessRuleException(ErrorCodes.USR_001,
                $"El email '{request.Email}' ya está registrado.");
        }

        // Los datos que no manda el cliente los completa el servidor
        User usuario = new User();
        usuario.Id = Guid.NewGuid();
        usuario.Nombre = request.Nombre;
        usuario.Apellido = request.Apellido;
        usuario.Email = request.Email;
        usuario.PasswordHash = CalcularHash(request.Password);
        usuario.FechaRegistro = DateTime.UtcNow;
        usuario.Activo = true;
        usuario.IntentosFallidos = 0;

        repositorio.Agregar(usuario);

        return ConvertirAResponse(usuario);
    }

    // POST login: controla las credenciales y la regla de bloqueo.
    public LoginResponse IniciarSesion(LoginRequest request)
    {
        User? usuario = repositorio.ObtenerPorEmail(request.Email);

        // USR-003: el email no existe
        if (usuario == null)
        {
            throw new BusinessRuleException(ErrorCodes.USR_003, "Credenciales incorrectas.");
        }

        // Si el usuario está bloqueado no se mira la contraseña
        if (usuario.Activo == false)
        {
            // USR-004: se bloqueó solo, por llegar a 3 intentos fallidos
            if (usuario.IntentosFallidos >= MaximoIntentosFallidos)
            {
                throw new BusinessRuleException(ErrorCodes.USR_004,
                    "Su cuenta fue bloqueada por superar el máximo de intentos fallidos. Contacte a soporte.");
            }

            // USR-005: lo bloquearon a mano por fraude (Activo en false con menos de 3 intentos)
            throw new BusinessRuleException(ErrorCodes.USR_005,
                "Su cuenta fue suspendida por razones de seguridad. Contacte a soporte.");
        }

        // USR-003: la contraseña no coincide
        if (CalcularHash(request.Password) != usuario.PasswordHash)
        {
            usuario.IntentosFallidos = usuario.IntentosFallidos + 1;

            // Con el 3er intento fallido la cuenta queda bloqueada
            if (usuario.IntentosFallidos >= MaximoIntentosFallidos)
            {
                usuario.Activo = false;
                throw new BusinessRuleException(ErrorCodes.USR_004,
                    "Su cuenta fue bloqueada por superar el máximo de intentos fallidos. Contacte a soporte.");
            }

            throw new BusinessRuleException(ErrorCodes.USR_003, "Credenciales incorrectas.");
        }

        // Login correcto: se reinician los intentos fallidos
        usuario.IntentosFallidos = 0;

        LoginResponse respuesta = new LoginResponse();
        respuesta.Id = usuario.Id;
        respuesta.Nombre = usuario.Nombre;
        respuesta.Apellido = usuario.Apellido;
        respuesta.Email = usuario.Email;
        return respuesta;
    }

    // GET por Id: lo usan Orders y Notifications para saber si un usuario existe.
    public UserResponse ObtenerPorId(Guid id)
    {
        User? usuario = repositorio.ObtenerPorId(id);

        // USR-007: el usuario no existe
        if (usuario == null)
        {
            throw new NotFoundException(ErrorCodes.USR_007, "Usuario no encontrado.");
        }

        return ConvertirAResponse(usuario);
    }

    // Convierte la contraseña en un hash SHA256 (texto en hexadecimal).
    // Así la contraseña nunca se guarda tal cual.
    private string CalcularHash(string password)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(password);
        byte[] hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash);
    }

    // Pasa el usuario de adentro (Model) al formulario de salida (DTO), sin la contraseña
    private UserResponse ConvertirAResponse(User usuario)
    {
        UserResponse respuesta = new UserResponse();
        respuesta.Id = usuario.Id;
        respuesta.Nombre = usuario.Nombre;
        respuesta.Apellido = usuario.Apellido;
        respuesta.Email = usuario.Email;
        respuesta.FechaRegistro = usuario.FechaRegistro;
        respuesta.Activo = usuario.Activo;
        return respuesta;
    }
}