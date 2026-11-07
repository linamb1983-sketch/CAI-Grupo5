using Microsoft.AspNetCore.Mvc;
using Users.API.DTOs;
using Users.API.Exceptions;
using Users.API.Services;

namespace Users.API.Controllers;

/// <summary>
/// Recibe los pedidos HTTP de usuarios y se los pasa al Service.
/// </summary>
[ApiController]
[Route("api/users")]
[Tags("Users")]
public class UsersController : ControllerBase
{
    private UserService servicio;

    public UsersController(UserService servicio)
    {
        this.servicio = servicio;
    }

    /// <summary>
    /// Registra un usuario nuevo.
    /// </summary>
    /// <param name="request">Datos del usuario a registrar.</param>
    /// <response code="201">Usuario registrado (nunca se devuelve la contraseña).</response>
    /// <response code="400">USR-002: Los datos del usuario son inválidos.</response>
    /// <response code="409">USR-001: El email ya está registrado.</response>
    /// <response code="500">USR-006: Error interno al procesar el usuario.</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorDatosInvalidos), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorEmailDuplicado), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorInterno), StatusCodes.Status500InternalServerError)]
    public IActionResult Registrar(RegisterRequest request)
    {
        ValidarDatos();

        UserResponse usuario = servicio.Registrar(request);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = usuario.Id }, usuario);
    }

    /// <summary>
    /// Inicia sesión con email y contraseña. Con 3 intentos fallidos seguidos la cuenta se bloquea.
    /// </summary>
    /// <param name="request">Email y contraseña.</param>
    /// <response code="200">Login correcto.</response>
    /// <response code="400">USR-002: Los datos del usuario son inválidos.</response>
    /// <response code="401">USR-003: Credenciales incorrectas.</response>
    /// <response code="403">USR-004: Usuario bloqueado por demasiados intentos fallidos. USR-005: Usuario bloqueado por detección de fraude.</response>
    /// <response code="500">USR-006: Error interno al procesar el usuario.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDatosInvalidos), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorCredencialesIncorrectas), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorBloqueadoPorIntentos), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorInterno), StatusCodes.Status500InternalServerError)]
    public IActionResult IniciarSesion(LoginRequest request)
    {
        ValidarDatos();

        LoginResponse respuesta = servicio.IniciarSesion(request);
        return Ok(respuesta);
    }

    /// <summary>
    /// Obtiene un usuario por su Id (lo usan Orders y Notifications para saber si existe). Agregado propio: no está en la consigna.
    /// </summary>
    /// <param name="id">Id del usuario.</param>
    /// <response code="200">El usuario encontrado (sin la contraseña).</response>
    /// <response code="404">USR-007: Usuario no encontrado.</response>
    /// <response code="500">USR-006: Error interno al procesar el usuario.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorUsuarioNoEncontrado), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorInterno), StatusCodes.Status500InternalServerError)]
    public IActionResult ObtenerPorId(Guid id)
    {
        UserResponse usuario = servicio.ObtenerPorId(id);
        return Ok(usuario);
    }

    // Si los datos no cumplen las reglas del DTO, junta todos los errores
    // separados por punto y coma y corta con USR-002.
    private void ValidarDatos()
    {
        if (ModelState.IsValid)
        {
            return;
        }

        string errores = "";

        foreach (var campo in ModelState.Values)
        {
            foreach (var error in campo.Errors)
            {
                errores = errores + error.ErrorMessage + "; ";
            }
        }

        if (errores == "")
        {
            errores = "Los datos del usuario son inválidos.";
        }

        throw new ValidationException(ErrorCodes.USR_002, errores);
    }
}