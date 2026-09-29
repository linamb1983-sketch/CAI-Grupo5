\# CAI-Grupo5 - E-Commerce con Microservicios (.NET 10)



\## Integrantes

\- Federico Glorioso (líder)

\- Carolina ...

\- ...



\## Microservicios

| Servicio | Descripción | Puerto |

|---|---|---|

| Products.API | CRUD de productos | 5001 |

| Users.API | Registro, login y bloqueo | 5002 |

| Orders.API | Creación y estado de órdenes | 5003 |

| Cart.API | Carrito por usuario | 5004 |

| Notifications.API | Envío simulado de notificaciones | 5005 |



\## Cómo ejecutar

dotnet build

dotnet run --project src/Products.API



\## Flujo de trabajo con Git

1\. `git checkout main` y `git pull`

2\. `git checkout -b feature/<servicio>-<tarea>`

3\. Commits chicos y descriptivos

4\. `git push -u origin <rama>` y abrir Pull Request

5\. Otro integrante revisa y aprueba antes del merge

6\. Nadie sube directo a `main`

