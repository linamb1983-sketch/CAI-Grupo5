namespace Orders.API.Exceptions;

// Indica que no se encontró la orden o una referencia necesaria.
public class NotFoundException : Exception
{
    public string ErrorCode { get; }

    // Crea la excepción con el código y el mensaje del catálogo.
    public NotFoundException(string errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }
}
