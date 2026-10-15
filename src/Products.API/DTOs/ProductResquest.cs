namespace Products.API.DTOs;

// Datos que llegan del cliente.
// No tiene Id ni FechaCreacion porque eso lo genera el servidor.
public class ProductRequest
{
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public string Categoria { get; set; }
}