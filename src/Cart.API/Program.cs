using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Cart.API.HealthChecks;
using Cart.API.ExceptionHandlers;
using Cart.API.Middlewares;
using Cart.API.Repositories;
using Cart.API.Services;
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
        .Enrich.WithProperty("Servicio", "Cart.API")
        .Enrich.WithProperty("CorrelationId", "-")
        .Enrich.WithProperty("Endpoint", "-")
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{Servicio}] [{Endpoint}] [{CorrelationId}] {Message:lj}{NewLine}{Exception}")
        .WriteTo.File(new JsonFormatter(renderMessage: true), "logs/cart-.log", rollingInterval: RollingInterval.Day);
});

// Controllers. Apagamos la validación automática de .NET para que use la nuestra (CRT-004)
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opciones =>
{
    // Lee los comentarios /// del código (controller, DTOs, modelo) y los muestra en Swagger
    string archivoXml = Path.Combine(AppContext.BaseDirectory, "Cart.API.xml");
    opciones.IncludeXmlComments(archivoXml);
});

// Manejo de errores (Apéndice B de la consigna).
// Van en orden: primero los específicos, último el genérico.
builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<BusinessRuleExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Para que Cart pueda llamar a Products por HTTP (IHttpClientFactory)
builder.Services.AddHttpClient();

// Para que el Service pueda leer el Correlation ID del pedido actual y mandárselo a Products
builder.Services.AddHttpContextAccessor();

// Nuestras clases
// Singleton = una sola lista para toda la app (si no, se borraría en cada pedido)
builder.Services.AddSingleton<CartRepository>();
builder.Services.AddScoped<CartService>();


// Health Checks (sección 5.4).
// "live" = el proceso está vivo. "ready" = está listo para recibir pedidos.
builder.Services.AddHealthChecks()
    .AddCheck<ProcesoHealthCheck>("proceso", tags: new[] { "live" })
    .AddCheck<CartHealthCheck>("repositorio", tags: new[] { "ready" });

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

app.UseAuthorization();

app.MapControllers();


// /health = todos los chequeos, /health/ready = los de "ready", /health/live = los de "live"
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = HealthResponseWriter.Escribir
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = HealthResponseWriter.SoloReady,
    ResponseWriter = HealthResponseWriter.Escribir
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = HealthResponseWriter.SoloLive,
    ResponseWriter = HealthResponseWriter.Escribir
});

app.Run();