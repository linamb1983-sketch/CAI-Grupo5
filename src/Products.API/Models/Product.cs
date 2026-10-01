
namespace Products.API.Models;

public class Product
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public string Categoria { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
}
