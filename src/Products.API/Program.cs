using Products.API.ExceptionHandlers;
using Products.API.Middlewares;
using Products.API.Repositories;
using Products.API.Services;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

var builder = WebApplication.CreateBuilder(args);

// Serilog va primero, antes de cualquier otra configuración.
// Escribe los logs en dos lugares: la consola (fácil de leer) y un archivo JSON en la carpeta logs.
builder.Host.UseSerilog((contexto, configuracion) =>
{
    configuracion
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
        // Los errores ya los loguean nuestros handlers: así no salen repetidos
        .MinimumLevel.Override("Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware", LogEventLevel.Fatal)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Servicio", "Products.API")
        .Enrich.WithProperty("CorrelationId", "-")
        .Enrich.WithProperty("Endpoint", "-")
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{CorrelationId}] {Message:lj}{NewLine}{Exception}")
        .WriteTo.File(new JsonFormatter(renderMessage: true), "logs/products-.log", rollingInterval: RollingInterval.Day);
});

// Controllers. Apagamos la validación automática de .NET para que use la nuestra (PRD-002)
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opciones =>
{
    // Lee los comentarios /// del código (controller, DTOs, modelo) y los muestra en Swagger
    string archivoXml = Path.Combine(AppContext.BaseDirectory, "Products.API.xml");
    opciones.IncludeXmlComments(archivoXml);
});

// Manejo de errores (Apéndice B de la consigna).
// Van en orden: primero los específicos, último el genérico.
builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<BusinessRuleExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Nuestras clases
// Singleton = una sola lista para toda la app (si no, se borraría en cada pedido)
builder.Services.AddSingleton<ProductRepository>();
builder.Services.AddScoped<ProductService>();

var app = builder.Build();

// Le pone el Correlation ID a cada pedido. Va primero para que todo lo demás ya lo tenga.
app.UseMiddleware<CorrelationIdMiddleware>();

// Loguea el fin de cada request con su duración
app.UseSerilogRequestLogging();

// Activa los handlers de errores. Va antes del resto para atajar todo lo que venga después.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();