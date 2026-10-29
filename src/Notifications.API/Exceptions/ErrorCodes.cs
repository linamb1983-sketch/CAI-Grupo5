namespace Notifications.API.Exceptions;

// Catálogo de códigos de error de Notifications (sección 4.5 de la consigna).
public static class ErrorCodes
{
    public const string NTF_001 = "NTF-001"; // 404 - Usuario no encontrado
    public const string NTF_002 = "NTF-002"; // 400 - Datos de la notificación inválidos
    public const string NTF_003 = "NTF-003"; // 404 - El usuario no tiene notificaciones
    public const string NTF_004 = "NTF-004"; // 500 - Error interno
}