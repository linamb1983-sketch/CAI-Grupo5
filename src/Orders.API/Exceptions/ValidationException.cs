namespace Orders.API.Exceptions;

// Indica que los datos recibidos no cumplen la validación.
public class ValidationException : Exception
{
    public string ErrorCode { get; }

    // Crea la excepción con el código y el mensaje del catálogo.
    public ValidationException(string errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }
}
