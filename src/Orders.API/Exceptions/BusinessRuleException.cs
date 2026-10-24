namespace Orders.API.Exceptions;

// Indica que una regla del negocio impide completar la operación.
public class BusinessRuleException : Exception
{
    public string ErrorCode { get; }

    // Crea la excepción con el código y el mensaje del catálogo.
    public BusinessRuleException(string errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }
}
