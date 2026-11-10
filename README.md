# CAI-Grupo5 - E-Commerce con Microservicios (.NET 10)

Trabajo práctico de Construcción de Aplicaciones Informáticas.
Es un e-commerce armado con 5 microservicios. Cada uno es una API REST independiente hecha en C# con .NET 10.

## Integrantes

- Federico Glorioso
- Carolina Sol Masciotra Brea
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
   Products.API      --->  Orders.API     (antes de borrar un producto, revisa que no tenga órdenes activas)
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

> **Importante:** para borrar un producto (`DELETE /api/products/{id}`) Orders.API tiene que estar levantado, porque Products le pregunta si el producto tiene órdenes activas (PRD-004). Si Orders está apagado, el borrado responde 500 PRD-005 y el producto no se borra.

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
| USR-007 | 404 | Usuario no encontrado. (Agregado propio: no está en la consigna; lo usa `GET /api/users/{id}`, que consultan Orders y Notifications.) |

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

## Health Checks

Cada servicio expone tres endpoints de salud (sección 5.4 de la consigna):

| Endpoint | Qué devuelve |
|---|---|
| `/health` | Estado general: todos los chequeos |
| `/health/ready` | Solo el chequeo `ready`: el servicio está listo para recibir pedidos |
| `/health/live` | Solo el chequeo `live`: el proceso está en ejecución |

La respuesta es siempre JSON con este formato:

```json
{
  "estado": "Healthy",
  "servicio": "Users.API",
  "duracionMs": 1.4,
  "chequeos": [
    {
      "nombre": "repositorio",
      "estado": "Healthy",
      "descripcion": "El repositorio responde (2 usuarios)."
    }
  ]
}
```

- `Healthy` → HTTP 200. `Unhealthy` → HTTP 503.
- El chequeo `live` verifica que el proceso esté en ejecución.
- El chequeo `ready` verifica que el repositorio de datos responda.

Servicios con Health Checks implementados: **Products.API**, **Users.API**, **Orders.API**, **Cart.API**, **Notifications.API**.

## Capturas de Swagger

### Products.API

| Archivo | Endpoint | Resultado |
|---|---|---|
| [products-404-PRD-001.png](docs/products-404-PRD-001.png) | GET /api/products/{id} | 404 PRD-001 |
| [products-400-PRD-002.png](docs/products-400-PRD-002.png) | POST /api/products | 400 PRD-002 |
| [products-409-PRD-003.png](docs/products-409-PRD-003.png) | POST /api/products | 409 PRD-003 |
| [products-409-PRD-004.png](docs/products-409-PRD-004.png) | DELETE /api/products/{id} (producto con una orden Pendiente) | 409 PRD-004 |
| [products-204-eliminar.png](docs/products-204-eliminar.png) | DELETE /api/products/{id} (la orden del producto ya está Enviada) | 204 |
| [products-500-PRD-005.png](docs/products-500-PRD-005.png) | DELETE /api/products/{id} (Orders apagado; en Development se ve el detalle técnico) | 500 PRD-005 |
| [products-health.png](docs/products-health.png) | /health | Healthy |
| [products-health-ready.png](docs/products-health-ready.png) | /health/ready | Healthy |
| [products-health-live.png](docs/products-health-live.png) | /health/live | Healthy |

### Users.API

| Archivo | Endpoint | Resultado |
|---|---|---|
| [users-201-USR-register.png](docs/users-201-USR-register.png) | POST /api/users/register | 201 Creado |
| [users-200-USR-login.png](docs/users-200-USR-login.png) | POST /api/users/login | 200 OK |
| [users-400-USR-002.png](docs/users-400-USR-002.png) | POST /api/users/register | 400 USR-002 |
| [users-409-USR-001.png](docs/users-409-USR-001.png) | POST /api/users/register | 409 USR-001 |
| [users-401-USR-003.png](docs/users-401-USR-003.png) | POST /api/users/login | 401 USR-003 |
| [users-403-USR-004.png](docs/users-403-USR-004.png) | POST /api/users/login | 403 USR-004 |
| [users-403-USR-005.png](docs/users-403-USR-005.png) | POST /api/users/login | 403 USR-005 |
| [users-404-USR-007.png](docs/users-404-USR-007.png) | GET /api/users/{id} | 404 USR-007 |
| [users-500-USR-006.png](docs/users-500-USR-006.png) | POST /api/users/register (ejemplo documentado en Swagger: Users no depende de otro servicio, así que el 500 solo aparece ante un error inesperado) | 500 USR-006 |
| [users-health.png](docs/users-health.png) | /health | Healthy |
| [users-health-ready.png](docs/users-health-ready.png) | /health/ready | Healthy |
| [users-health-live.png](docs/users-health-live.png) | /health/live | Healthy |

### Orders.API

| Archivo | Endpoint | Resultado |
|---|---|---|
| [setup-user-register-201.png](docs/capturas/orders/setup-user-register-201.png) | POST /api/users/register (preparación) | 201 Creado |
| [setup-product-create-201.png](docs/capturas/orders/setup-product-create-201.png) | POST /api/products (preparación) | 201 Creado |
| [orders-201-crear.png](docs/capturas/orders/orders-201-crear.png) | POST /api/orders | 201 Creado |
| [orders-200-obtener.png](docs/capturas/orders/orders-200-obtener.png) | GET /api/orders/{id} | 200 OK |
| [orders-200-listar.png](docs/capturas/orders/orders-200-listar.png) | GET /api/orders?usuarioId={id} | 200 OK |
| [orders-200-cambiar-estado.png](docs/capturas/orders/orders-200-cambiar-estado.png) | PUT /api/orders/{id}/status (Pendiente → Confirmada) | 200 OK |
| [orders-400-ORD-002.png](docs/capturas/orders/orders-400-ORD-002.png) | POST /api/orders (cantidad 0) | 400 ORD-002 |
| [orders-404-ORD-001.png](docs/capturas/orders/orders-404-ORD-001.png) | GET /api/orders/{id} (orden inexistente) | 404 ORD-001 |
| [orders-404-ORD-003.png](docs/capturas/orders/orders-404-ORD-003.png) | POST /api/orders (usuario inexistente) | 404 ORD-003 |
| [orders-404-ORD-004.png](docs/capturas/orders/orders-404-ORD-004.png) | POST /api/orders (producto inexistente) | 404 ORD-004 |
| [orders-422-ORD-005.png](docs/capturas/orders/orders-422-ORD-005.png) | POST /api/orders (pide 6, hay 5) | 422 ORD-005 |
| [orders-409-ORD-006.png](docs/capturas/orders/orders-409-ORD-006.png) | PUT /api/orders/{id}/status (Confirmada → Pendiente) | 409 ORD-006 |
| [orders-500-ORD-007.png](docs/capturas/orders/orders-500-ORD-007.png) | POST /api/orders (Users apagado; en Development se ve el detalle técnico) | 500 ORD-007 |
| [orders-serilog-consola.png](docs/capturas/orders/orders-serilog-consola.png) | Consola de Serilog: POST /api/orders (llama a Users y Products con el mismo Correlation ID) y PUT con regla de negocio | Information + Warning ORD-006 |
| [orders-health.png](docs/capturas/orders/orders-health.png) | /health | Healthy |
| [orders-health-ready.png](docs/capturas/orders/orders-health-ready.png) | /health/ready | Healthy |
| [orders-health-live.png](docs/capturas/orders/orders-health-live.png) | /health/live | Healthy |

### Cart.API

| Archivo | Endpoint | Resultado |
|---|---|---|
| [cart-200-agregar.png](docs/cart-200-agregar.png) | POST /api/cart/{userId}/items | 200 OK |
| [cart-404-CRT-001.png](docs/cart-404-CRT-001.png) | GET /api/cart/{userId} | 404 CRT-001 |
| [cart-404-CRT-002.png](docs/cart-404-CRT-002.png) | POST /api/cart/{userId}/items | 404 CRT-002 |
| [cart-422-CRT-003.png](docs/cart-422-CRT-003.png) | POST /api/cart/{userId}/items | 422 CRT-003 |
| [cart-400-CRT-004.png](docs/cart-400-CRT-004.png) | POST /api/cart/{userId}/items | 400 CRT-004 |
| [cart-500-CRT-005.png](docs/cart-500-CRT-005.png) | POST /api/cart/{userId}/items (Products apagado) | 500 CRT-005 |
| [cart-health.png](docs/cart-health.png) | /health | Healthy |
| [cart-health-ready.png](docs/cart-health-ready.png) | /health/ready | Healthy |
| [cart-health-live.png](docs/cart-health-live.png) | /health/live | Healthy |

### Notifications.API

| Archivo | Endpoint | Resultado |
|---|---|---|
| [notifications-201-enviar.png](docs/notifications-201-enviar.png) | POST /api/notifications/send | 201 Creado |
| [notifications-200-listar.png](docs/notifications-200-listar.png) | GET /api/notifications/{userId} | 200 OK |
| [notifications-400-NTF-002.png](docs/notifications-400-NTF-002.png) | POST /api/notifications/send | 400 NTF-002 |
| [notifications-404-NTF-001.png](docs/notifications-404-NTF-001.png) | POST /api/notifications/send | 404 NTF-001 |
| [notifications-404-NTF-003.png](docs/notifications-404-NTF-003.png) | GET /api/notifications/{userId} | 404 NTF-003 |
| [notifications-500-NTF-004.png](docs/notifications-500-NTF-004.png) | POST /api/notifications/send (Users apagado) | 500 NTF-004 |
| [notifications-health.png](docs/notifications-health.png) | /health | Healthy |
| [notifications-health-ready.png](docs/notifications-health-ready.png) | /health/ready | Healthy |
| [notifications-health-live.png](docs/notifications-health-live.png) | /health/live | Healthy |

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
├── HealthChecks/       # ProcesoHealthCheck, chequeo específico y HealthResponseWriter
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
