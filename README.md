# CAI-Grupo5 - E-Commerce con Microservicios (.NET 10)

Trabajo práctico de Construcción de Aplicaciones Informáticas.
Es un e-commerce armado con 5 microservicios. Cada uno es una API REST independiente hecha en C# con .NET 10.

## Integrantes

- Federico Glorioso
- Carolina Masciotra
- Bianca Robles

## Microservicios y puertos

| Servicio | Qué hace | Puerto | Swagger |
|---|---|---|---|
| Products.API | Alta, baja, modificación y consulta de productos | 5001 | http://localhost:5001/swagger |
| Users.API | Registro, login y bloqueo de usuarios | 5002 | http://localhost:5002/swagger |
| Orders.API | Crea órdenes y cambia su estado | 5003 | http://localhost:5003/swagger |
| Cart.API | Carrito de compras de cada usuario | 5004 | http://localhost:5004/swagger |
| Notifications.API | Registra y simula el envío de notificaciones | 5005 | http://localhost:5005/swagger |

Cada puerto está fijo en el `appsettings.json` de su servicio (sección `Kestrel`).

## Diagrama de arquitectura

```
                         Cliente (Swagger / navegador)
                                      |
      +--------------+----------------+---------------+------------------+
      |              |                |               |                  |
 Products.API    Users.API       Orders.API       Cart.API      Notifications.API
    :5001          :5002           :5003           :5004             :5005

 Llamadas entre servicios (HTTP):
   Orders.API        --->  Users.API      (valida que el usuario exista)
   Orders.API        --->  Products.API   (valida el producto, toma el precio y controla el stock)
   Cart.API          --->  Products.API   (valida el producto y controla el stock)
   Notifications.API --->  Users.API      (valida que el usuario exista)
```

- Los servicios se comunican por HTTP con `IHttpClientFactory` y se pasan el header `X-Correlation-Id`.
- La persistencia la provee la cátedra como librería. Hasta que llegue, cada servicio guarda los datos en una lista en memoria (se borran al reiniciar).

## Cómo ejecutar

Requisitos: .NET 10 SDK y Visual Studio (o la consola con `dotnet`).

**Desde Visual Studio**
1. Abrir `ECommerce.sln`.
2. Para levantar los 5 juntos: clic derecho en la solución → **Configurar proyectos de inicio** → **Varios proyectos de inicio** → poner **Iniciar** en los 5.
3. Ejecutar sin depurar con **Ctrl+F5**.
4. Abrir el Swagger de cada servicio (ver la tabla de puertos).

**Desde la consola**

```
dotnet build
dotnet run --project src/Products.API
dotnet run --project src/Users.API
dotnet run --project src/Orders.API
dotnet run --project src/Cart.API
dotnet run --project src/Notifications.API
```

Cada `dotnet run` va en una consola distinta.

## Formato de los errores

Todos los errores (4xx y 5xx) responden con este formato (sección 3.1 de la consigna):

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404,
  "detail": "El recurso solicitado no fue encontrado.",
  "instance": "/api/products/99",
  "errorCode": "PRD-001",
  "errorMessage": "Producto no encontrado.",
  "correlationId": "dba88770-fd68-49a8-98d4-c1089cbc88c9"
}
```

Los errores los arman los `IExceptionHandler` de cada servicio. Nunca se muestra el detalle técnico (stack trace).

## Códigos de error

### Products.API

| errorCode | HTTP | Mensaje |
|---|---|---|
| PRD-001 | 404 | Producto no encontrado. |
| PRD-002 | 400 | Los datos del producto son inválidos. |
| PRD-003 | 409 | Ya existe un producto con ese nombre en la categoría. |
| PRD-004 | 409 | El producto tiene órdenes activas y no puede eliminarse. |
| PRD-005 | 500 | Error interno al procesar el producto. |

### Users.API

| errorCode | HTTP | Mensaje |
|---|---|---|
| USR-001 | 409 | El email ya está registrado. |
| USR-002 | 400 | Los datos del usuario son inválidos. |
| USR-003 | 401 | Credenciales incorrectas. |
| USR-004 | 403 | Usuario bloqueado por demasiados intentos fallidos. |
| USR-005 | 403 | Usuario bloqueado por detección de fraude. |
| USR-006 | 500 | Error interno al procesar el usuario. |

### Orders.API

| errorCode | HTTP | Mensaje |
|---|---|---|
| ORD-001 | 404 | Orden no encontrada. |
| ORD-002 | 400 | Los datos de la orden son inválidos. |
| ORD-003 | 404 | Usuario no encontrado al crear la orden. |
| ORD-004 | 404 | Producto no encontrado al crear la orden. |
| ORD-005 | 422 | Stock insuficiente para uno o más productos. |
| ORD-006 | 409 | El estado de la orden no puede ser modificado. |
| ORD-007 | 500 | Error interno al procesar la orden. |

### Cart.API

| errorCode | HTTP | Mensaje |
|---|---|---|
| CRT-001 | 404 | Carrito no encontrado. |
| CRT-002 | 404 | Producto no encontrado. |
| CRT-003 | 422 | Stock insuficiente para agregar al carrito. |
| CRT-004 | 400 | Cantidad inválida. |
| CRT-005 | 500 | Error interno al procesar el carrito. |

### Notifications.API

| errorCode | HTTP | Mensaje |
|---|---|---|
| NTF-001 | 404 | Usuario no encontrado. |
| NTF-002 | 400 | Los datos de la notificación son inválidos. |
| NTF-003 | 404 | No se encontraron notificaciones para el usuario. |
| NTF-004 | 500 | Error interno al procesar la notificación. |

## Logs y Correlation ID

- Los logs se hacen con **Serilog**: en la consola (fácil de leer) y en un archivo JSON dentro de la carpeta `logs` de cada servicio (uno por día).
- Cada log tiene: Timestamp, Nivel, Servicio, Endpoint, Correlation ID y errorCode cuando aplica.
- Se loguea el inicio y el fin de cada request, con su duración.
- Los errores de negocio van como **Warning** y los errores inesperados como **Error**.
- Cada pedido lleva un **X-Correlation-Id**. Si el pedido no lo trae, el servicio crea uno. Se devuelve en el header de la respuesta, en el campo `correlationId` de los errores y se pasa a los otros servicios cuando se los llama.

## Estructura del proyecto

```
ECommerce.sln
├── src/
│   ├── Products.API/
│   ├── Users.API/
│   ├── Orders.API/
│   ├── Cart.API/
│   └── Notifications.API/
├── docs/            # Capturas de Swagger
└── README.md
```

Cada servicio tiene la misma estructura (tomamos Products.API como plantilla):

```
Products.API/
├── Controllers/        # Endpoints
├── Models/             # Entidades del dominio
├── DTOs/               # Request y Response
├── Services/           # Lógica de negocio
├── Repositories/       # Guarda los datos (en memoria por ahora)
├── Exceptions/         # NotFoundException, ValidationException, BusinessRuleException y ErrorCodes
├── ExceptionHandlers/  # Un IExceptionHandler por tipo de excepción
├── Middlewares/        # CorrelationIdMiddleware
├── logs/               # Archivos de log de Serilog
└── Program.cs
```

## Cómo trabajamos con Git

Usamos GitHub Desktop y GitHub web.

1. Traer lo último de `main` (Fetch origin y Pull origin).
2. Crear una rama por tarea desde `main`: `feature/<servicio>-<tarea>` (por ejemplo `feature/products-serilog`).
3. Hacer commits chicos con título "Servicio: qué se hizo" y una descripción simple.
4. Subir la rama y abrir un Pull Request a `main`.
5. Otro integrante lo prueba y lo aprueba. Recién ahí se mergea.
6. Nadie sube directo a `main`.