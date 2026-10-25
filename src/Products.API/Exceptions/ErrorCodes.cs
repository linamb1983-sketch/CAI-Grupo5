namespace Products.API.Exceptions;

// Catálogo de códigos de error de Products (sección 4.1 de la consigna).
// Son constantes para no escribir los textos a mano en cada lugar.
public static class ErrorCodes
{
    public const string PRD_001 = "PRD-001"; // 404 - Producto no encontrado
    public const string PRD_002 = "PRD-002"; // 400 - Datos del producto inválidos
    public const string PRD_003 = "PRD-003"; // 409 - Producto duplicado en la categoría
    public const string PRD_004 = "PRD-004"; // 409 - Producto con órdenes activas
    public const string PRD_005 = "PRD-005"; // 500 - Error interno
}
