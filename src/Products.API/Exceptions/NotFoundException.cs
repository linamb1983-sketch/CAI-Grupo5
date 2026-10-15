namespace Products.API.Exceptions;

// Cuando algo no existe (ej: un Id que no está)
public class NotFoundException : Exception
{
    public string ErrorCode { get; set; }

    public NotFoundException(string errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }
}