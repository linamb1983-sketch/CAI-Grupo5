namespace Products.API.Exceptions;

// Cuando los datos que mandan están mal (ej: precio negativo)
public class ValidationException : Exception
{
    public string ErrorCode { get; set; }

    public ValidationException(string errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }
}