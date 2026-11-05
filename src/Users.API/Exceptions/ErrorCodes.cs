namespace Users.API.Exceptions;

// Catálogo de códigos de error de Users (sección 4.2 de la consigna).
public static class ErrorCodes
{
    public const string USR_001 = "USR-001"; // 409 - El email ya está registrado
    public const string USR_002 = "USR-002"; // 400 - Los datos del usuario son inválidos
    public const string USR_003 = "USR-003"; // 401 - Credenciales incorrectas
    public const string USR_004 = "USR-004"; // 403 - Usuario bloqueado por demasiados intentos fallidos
    public const string USR_005 = "USR-005"; // 403 - Usuario bloqueado por detección de fraude
    public const string USR_006 = "USR-006"; // 500 - Error interno al procesar el usuario
    public const string USR_007 = "USR-007"; // 404 - Usuario no encontrado (agregado nuestro, para GET /api/users/{id})
}