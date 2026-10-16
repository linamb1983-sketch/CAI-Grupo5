namespace Products.API.DTOs;

// Datos que devolvemos al cliente.
// Acá sí va el Id y la fecha, porque el producto ya existe.
public class ProductResponse
{
    public Guid Id { get; set; }
    public string Nombre { get; set; }
    public string? Descripcion { get; set; }
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public string Categoria { get; set; }
    public DateTime FechaCreacion { get; set; }
}