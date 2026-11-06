using Orders.API.ExceptionHandlers;
using Orders.API.HealthChecks;
using Orders.API.Repositories;
using Orders.API.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Desactiva la respuesta automática para usar ORD-002 en las validaciones.
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registra primero los errores específicos y al final el handler global.
builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<BusinessRuleExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddSingleton<OrderRepository>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddHttpClient();
builder.Services.AddHealthChecks()
    .AddCheck<ProcesoHealthCheck>("proceso", tags: new[] { "live" })
    .AddCheck<OrdersHealthCheck>("repositorio", tags: new[] { "ready" });

var app = builder.Build();

app.UseExceptionHandler();

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
