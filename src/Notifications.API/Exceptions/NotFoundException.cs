namespace Notifications.API.Exceptions;

// Cuando algo no existe (ej: el usuario, o sus notificaciones)
public class NotFoundException : Exception
{
    public string ErrorCode { get; }

    public NotFoundException(string errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }
}