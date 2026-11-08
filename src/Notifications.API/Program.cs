using Notifications.API.ExceptionHandlers;
using Notifications.API.Repositories;
using Notifications.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Controllers. Apagamos la validación automática de .NET para que use la nuestra (NTF-002)
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opciones =>
{
    // Lee los comentarios /// del código (controller, DTOs, modelo) y los muestra en Swagger
    string archivoXml = Path.Combine(AppContext.BaseDirectory, "Notifications.API.xml");
    opciones.IncludeXmlComments(archivoXml);
});

// Manejo de errores (Apéndice B de la consigna).
// Van en orden: primero los específicos, último el genérico.
// Notifications no tiene errores de regla de negocio (409/422), por eso no hay BusinessRule.
builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Para que Notifications pueda llamar a Users por HTTP (IHttpClientFactory)
builder.Services.AddHttpClient();

// Nuestras clases
// Singleton = una sola lista para toda la app (si no, se borraría en cada pedido)
builder.Services.AddSingleton<NotificationRepository>();
builder.Services.AddScoped<NotificationService>();

var app = builder.Build();

// Activa los handlers de errores. Va primero para atajar todo lo que venga después.
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