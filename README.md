# Grupo_20
<<<<<<< Updated upstream
TP Contrucción de Aplicaciones Informaticas
Grupo 20: 
882586 Vargas Vanina Cynthia -
892718 Giannini Florencia -
892376 Noelia Ajnota
=======

# E-Commerce — Arquitectura de Microservicios

Trabajo práctico grupal de Arquitectura y Diseño de Software.
Sistema de e-commerce dividido en cinco microservicios REST, cada uno con su propia
responsabilidad, desarrollado en C# con .NET 10.

# Integrantes

882586 Vargas Vanina Cynthia
892718 Giannini Florencia 
892376 Ajnota Noelia

# Servicios y puertos

| Servicio | Responsabilidad | Puerto |
| Products.API | Catálogo de productos (CRUD) | 5101 |
| Users.API | Registro y login de usuarios, bloqueo de cuentas | 5102 |
| Orders.API | Creación y seguimiento de órdenes de compra | 5103 |
| Cart.API | Carrito de compras por usuario | 5104 |
| Notifications.API | Registro y envío simulado de notificaciones | 5105 |

## Cómo ejecutar

Requisito: .NET SDK 10 o superior.

```bash
git clone <url-del-repositorio>
cd ECommerce
dotnet build ECommerce.sln
```

Cada servicio se levanta en su propia terminal, desde la raíz del repositorio:

```bash
dotnet run --project src/Products.API
dotnet run --project src/Users.API
dotnet run --project src/Orders.API
dotnet run --project src/Cart.API
dotnet run --project src/Notifications.API
```

Swagger UI de cada servicio: `http://localhost:<puerto>/swagger`
Health checks de cada servicio: `/health`, `/health/ready`, `/health/live`

## Arquitectura

Ver el diagrama y las decisiones de diseño en [docs/arquitectura.md](docs/arquitectura.md).

```
                      +---------------------+
                      |  Cliente (Swagger)  |
                      +----------+----------+
                                 |
     +----------+----------+-----+-----+-----------+
     |          |          |           |           |
 Products     Users      Orders       Cart    Notifications
   :5101      :5102      :5103       :5104       :5105

 Cart          --> Products       (valida producto y stock)
 Orders        --> Users          (valida usuario)
 Orders        --> Products       (valida producto y stock)
 Products      --> Orders         (órdenes activas antes de eliminar)
 Notifications --> Users          (valida usuario destinatario)
```

## Estructura de cada servicio

```
<Servicio>.API/
├── Controllers/         Reciben las peticiones HTTP
├── Models/              Entidades del dominio
├── DTOs/                Request y Response
├── Services/            Lógica de negocio
├── Persistence/         Acceso a datos
├── Exceptions/          Excepciones de dominio y catálogo de códigos
├── ExceptionHandlers/   IExceptionHandler por tipo de excepción
├── logs/                Archivos de log de Serilog
└── Program.cs
```

## Contrato de errores

Toda respuesta 4xx/5xx incluye `errorCode` y `errorMessage` además de los campos
estándar de Problem Details (`type`, `title`, `status`, `detail`, `instance`).

| Código | HTTP | Significado |
| PRD-001 | 404 | Producto no encontrado |
| PRD-002 | 400 | Datos del producto inválidos |
| PRD-003 | 409 | Ya existe un producto con ese nombre en la categoría |
| PRD-004 | 409 | El producto tiene órdenes activas y no puede eliminarse |
| PRD-005 | 500 | Error interno al procesar el producto |
| USR-001 | 409 | El email ya está registrado |
| USR-002 | 400 | Datos del usuario inválidos |
| USR-003 | 401 | Credenciales incorrectas |
| USR-004 | 403 | Usuario bloqueado por demasiados intentos fallidos |
| USR-005 | 403 | Usuario bloqueado por detección de fraude |
| USR-006 | 500 | Error interno al procesar el usuario |
| ORD-001 | 404 | Orden no encontrada |
| ORD-002 | 400 | Datos de la orden inválidos |
| ORD-003 | 404 | Usuario no encontrado al crear la orden |
| ORD-004 | 404 | Producto no encontrado al crear la orden |
| ORD-005 | 422 | Stock insuficiente |
| ORD-006 | 409 | El estado de la orden no puede ser modificado |
| ORD-007 | 500 | Error interno al procesar la orden |
| CRT-001 | 404 | Carrito no encontrado |
| CRT-002 | 404 | Producto no encontrado |
| CRT-003 | 422 | Stock insuficiente para agregar al carrito |
| CRT-004 | 400 | Cantidad inválida |
| CRT-005 | 500 | Error interno al procesar el carrito |
| NTF-001 | 404 | Usuario destinatario no encontrado |
| NTF-002 | 400 | Datos de la notificación inválidos |
| NTF-003 | 404 | No se encontraron notificaciones para el usuario |
| NTF-004 | 500 | Error interno al procesar la notificación |
>>>>>>> Stashed changes
