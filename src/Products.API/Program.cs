using Products.API.ExceptionHandlers;
using Products.API.Repositories;
using Products.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Controllers. Apagamos la validación automática de .NET para que use la nuestra (PRD-002)
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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