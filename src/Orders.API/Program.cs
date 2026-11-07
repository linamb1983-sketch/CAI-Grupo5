using Orders.API.ExceptionHandlers;
using Orders.API.HealthChecks;
using Orders.API.Middlewares;
using Orders.API.Repositories;
using Orders.API.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Orders.API.Swagger;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

var builder = WebApplication.CreateBuilder(args);

// Configura logs estructurados para consola y archivos JSON diarios.
builder.Host.UseSerilog((contexto, configuracion) =>
{
    configuracion
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware", LogEventLevel.Fatal)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Servicio", "Orders.API")
        .Enrich.WithProperty("CorrelationId", "-")
        .Enrich.WithProperty("Endpoint", "-")
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{CorrelationId}] {Message:lj}{NewLine}{Exception}")
        .WriteTo.File(new JsonFormatter(renderMessage: true), "logs/orders-.log", rollingInterval: RollingInterval.Day);
});

// Desactiva la respuesta automática para usar ORD-002 en las validaciones.
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opciones =>
{
    string archivoXml = Path.Combine(AppContext.BaseDirectory, "Orders.API.xml");
    opciones.IncludeXmlComments(archivoXml);
    opciones.OperationFilter<OrdersExamplesOperationFilter>();
});

// Registra primero los errores específicos y al final el handler global.
builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<BusinessRuleExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddSingleton<OrderRepository>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHealthChecks()
    .AddCheck<ProcesoHealthCheck>("proceso", tags: new[] { "live" })
    .AddCheck<OrdersHealthCheck>("repositorio", tags: new[] { "ready" });

var app = builder.Build();

app.UseExceptionHandler();
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// /health ejecuta todos los chequeos; ready y live ejecutan solo su etiqueta.
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
