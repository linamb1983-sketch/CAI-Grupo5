using System.Text.Json.Nodes;
using Microsoft.OpenApi;
using Orders.API.DTOs;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Orders.API.Swagger;

/// <summary>Agrega ejemplos completos de request y response para Orders en Swagger.</summary>
public class OrdersExamplesOperationFilter : IOperationFilter
{
    /// <summary>Configura ejemplos de acuerdo con la ruta y el método de cada operación.</summary>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        string? metodo = context.ApiDescription.HttpMethod;
        string? ruta = context.ApiDescription.RelativePath;

        if (metodo == "GET" && ruta == "api/orders")
        {
            PonerEjemploRespuesta(operation, "200", @"[
  {
    ""id"": ""f1e2d3c4-0000-0000-0000-aabbccddeeff"",
    ""usuarioId"": ""a1b2c3d4-0000-0000-0000-111122223333"",
    ""items"": [
      {
        ""productoId"": ""3fa85f64-5717-4562-b3fc-2c963f66afa6"",
        ""cantidad"": 2,
        ""precioUnitario"": 1500.00
      }
    ],
    ""total"": 3000.00,
    ""estado"": ""Pendiente"",
    ""fechaCreacion"": ""2024-03-10T11:00:00Z""
  }
]");
            PonerEjemploRespuesta(operation, "400", EjemploError("400", "Bad Request", "Los datos enviados son inválidos.", "ORD-002"));
            PonerEjemploRespuesta(operation, "500", EjemploError("500", "Internal Server Error", "Error interno al procesar la orden.", "ORD-007"));
        }
        else if (metodo == "GET" && ruta == "api/orders/{id}")
        {
            PonerEjemploRespuesta(operation, "200", EjemploOrden());
            PonerEjemploRespuesta(operation, "404", EjemploError("404", "Not Found", "Orden no encontrada.", "ORD-001"));
            PonerEjemploRespuesta(operation, "500", EjemploError("500", "Internal Server Error", "Error interno al procesar la orden.", "ORD-007"));
        }
        else if (metodo == "POST" && ruta == "api/orders")
        {
            PonerEjemploRequest(operation, @"{
  ""usuarioId"": ""a1b2c3d4-0000-0000-0000-111122223333"",
  ""items"": [
    {
      ""productoId"": ""3fa85f64-5717-4562-b3fc-2c963f66afa6"",
      ""cantidad"": 2
    }
  ]
}");
            PonerEjemploRespuesta(operation, "201", EjemploOrden());
            PonerEjemploRespuesta(operation, "400", EjemploError("400", "Bad Request", "La orden debe incluir al menos un producto.", "ORD-002"));
            PonerEjemplos404Crear(operation);
            PonerEjemploRespuesta(operation, "422", @"{
  ""type"": ""https://tools.ietf.org/html/rfc4918#section-11.2"",
  ""title"": ""Unprocessable Entity"",
  ""status"": 422,
  ""detail"": ""No se puede procesar la solicitud."",
  ""instance"": ""/api/orders"",
  ""errorCode"": ""ORD-005"",
  ""errorMessage"": ""Stock insuficiente para 'Notebook Dell XPS 15'. Disponible: 2, solicitado: 5."",
  ""correlationId"": ""8b1f2c3d-4e5a-4b6c-9d7e-0f1a2b3c4d5e""
}");
            PonerEjemploRespuesta(operation, "500", EjemploError("500", "Internal Server Error", "Error interno al procesar la orden.", "ORD-007"));
        }
        else if (metodo == "PUT" && ruta == "api/orders/{id}/status")
        {
            PonerEjemploRequest(operation, @"{
  ""estado"": ""Confirmada""
}");
            PonerEjemploRespuesta(operation, "200", @"{
  ""id"": ""f1e2d3c4-0000-0000-0000-aabbccddeeff"",
  ""estado"": ""Confirmada"",
  ""fechaActualizacion"": ""2024-03-10T12:00:00Z""
}");
            PonerEjemploRespuesta(operation, "400", EjemploError("400", "Bad Request", "Los datos de la orden son inválidos.", "ORD-002", "/api/orders/f1e2d3c4-0000-0000-0000-aabbccddeeff/status"));
            PonerEjemploRespuesta(operation, "404", EjemploError("404", "Not Found", "Orden no encontrada.", "ORD-001"));
            PonerEjemploRespuesta(operation, "409", EjemploError("409", "Conflict", "Una orden en estado 'Entregada' no puede pasar a 'Pendiente'.", "ORD-006", "/api/orders/f1e2d3c4-0000-0000-0000-aabbccddeeff/status"));
            PonerEjemploRespuesta(operation, "500", EjemploError("500", "Internal Server Error", "Error interno al procesar la orden.", "ORD-007"));
        }
    }

    /// <summary>Agrega el ejemplo completo del cuerpo de solicitud.</summary>
    private void PonerEjemploRequest(OpenApiOperation operation, string ejemplo)
    {
        if (operation.RequestBody != null && operation.RequestBody.Content != null && operation.RequestBody.Content.TryGetValue("application/json", out OpenApiMediaType? contenido))
        {
            if (contenido != null)
            {
                contenido.Example = JsonNode.Parse(ejemplo);
            }
        }
    }

    /// <summary>Agrega el ejemplo completo a una respuesta si existe su código.</summary>
    private void PonerEjemploRespuesta(OpenApiOperation operation, string codigo, string ejemplo)
    {
        if (operation.Responses != null && operation.Responses.TryGetValue(codigo, out IOpenApiResponse? respuesta))
        {
            if (respuesta != null && respuesta.Content != null && respuesta.Content.TryGetValue("application/json", out OpenApiMediaType? contenido))
            {
                if (contenido != null)
                {
                    contenido.Example = JsonNode.Parse(ejemplo);
                }
            }
        }
    }

    /// <summary>Documenta los dos motivos de 404 posibles al crear una orden.</summary>
    private void PonerEjemplos404Crear(OpenApiOperation operation)
    {
        if (operation.Responses != null && operation.Responses.TryGetValue("404", out IOpenApiResponse? respuesta))
        {
            if (respuesta == null)
            {
                return;
            }

            if (respuesta is OpenApiResponse respuestaEditable)
            {
                respuestaEditable.Description = "ORD-003: usuario no encontrado; ORD-004: producto no encontrado.";
            }

            if (respuesta.Content != null && respuesta.Content.TryGetValue("application/json", out OpenApiMediaType? contenido) && contenido != null)
            {
                contenido.Examples = new Dictionary<string, IOpenApiExample>();
                contenido.Examples.Add("usuarioNoEncontrado", new OpenApiExample
                {
                    Summary = "ORD-003: usuario no encontrado",
                    Value = JsonNode.Parse(EjemploError("404", "Not Found", "Usuario no encontrado al crear la orden.", "ORD-003"))
                });
                contenido.Examples.Add("productoNoEncontrado", new OpenApiExample
                {
                    Summary = "ORD-004: producto no encontrado",
                    Value = JsonNode.Parse(EjemploError("404", "Not Found", "Producto no encontrado al crear la orden.", "ORD-004"))
                });
            }
        }
    }

    /// <summary>Devuelve el JSON de ejemplo de una orden.</summary>
    private string EjemploOrden()
    {
        return @"{
  ""id"": ""f1e2d3c4-0000-0000-0000-aabbccddeeff"",
  ""usuarioId"": ""a1b2c3d4-0000-0000-0000-111122223333"",
  ""items"": [
    {
      ""productoId"": ""3fa85f64-5717-4562-b3fc-2c963f66afa6"",
      ""cantidad"": 2,
      ""precioUnitario"": 1500.00
    }
  ],
  ""total"": 3000.00,
  ""estado"": ""Pendiente"",
  ""fechaCreacion"": ""2024-03-10T11:00:00Z""
}";
    }

    /// <summary>Devuelve un ejemplo de error con el formato común del TP.</summary>
    private string EjemploError(string status, string title, string errorMessage, string errorCode, string instance = "/api/orders")
    {
        string type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
        string detail = "Ocurrió un error inesperado.";

        if (status == "400")
        {
            type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
            detail = "Los datos enviados son inválidos.";
        }
        else if (status == "404")
        {
            type = "https://tools.ietf.org/html/rfc7231#section-6.5.4";
            detail = "El recurso solicitado no fue encontrado.";
        }
        else if (status == "409")
        {
            type = "https://tools.ietf.org/html/rfc7231#section-6.5.9";
            detail = "No se puede modificar el estado.";
        }

        return @"{
  ""type"": """ + type + @""",
  ""title"": """ + title + @""",
  ""status"": " + status + @",
  ""detail"": """ + detail + @""",
  ""instance"": """ + instance + @""",
  ""errorCode"": """ + errorCode + @""",
  ""errorMessage"": """ + errorMessage + @""",
  ""correlationId"": ""8b1f2c3d-4e5a-4b6c-9d7e-0f1a2b3c4d5e""
}";
    }
}
