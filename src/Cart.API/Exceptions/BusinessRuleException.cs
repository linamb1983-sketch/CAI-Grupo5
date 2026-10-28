namespace Cart.API.Exceptions;

// Cuando se rompe una regla del negocio 
public class BusinessRuleException : Exception
{
    public string ErrorCode { get; }

    public BusinessRuleException(string errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }
}