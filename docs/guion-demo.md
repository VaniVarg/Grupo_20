# Guion de la demostración

La consigna pide: invocar endpoints exitosos y de error desde Swagger, mostrar logs y mostrar health checks.

## Antes de empezar

- [ ] Los cinco servicios levantados, cada uno en su terminal, con la consola visible (Products 5101,
      Users 5102, Orders 5103, Cart 5104, Notifications 5105).
- [ ] Cinco pestañas del navegador abiertas en `/swagger` de cada servicio.
- [ ] Una carpeta con los archivos `logs/*.log` de Orders, Products y Users lista para abrir.
- [ ] Los servicios **recién reiniciados** (los datos son en memoria: así la demo arranca limpia).
- [ ] Este guion y `docs/pruebas-integrales.http` abiertos en una segunda pantalla.

## Cronograma

Presentación y arquitectura | Diagrama (`docs/arquitectura.png`): cinco servicios, quién llama a quién y por qué |
Decisiones técnicas | Capas, `IExceptionHandler`, Correlation ID, persistencia detrás de interfaces |
Demo de éxito: flujo completo | Registrar usuario, crear producto, agregar al carrito, crear y confirmar una orden |
Demo de errores | Un error de cada servicio, mostrando `errorCode`, `errorMessage` y `correlationId` |
Logs y Correlation ID | El mismo `correlationId` en los logs de Orders, Users y Products |
Health checks | `/health`, `/health/live` y `/health/ready`; apagar un servicio y ver `Degraded` |


## Demo de éxito 

1. **Users:** `POST /api/users/register` → 201. Mostrar que la respuesta no trae la contraseña.
2. **Products:** `POST /api/products` (Notebook Dell XPS 15, stock 10) → 201. Copiar el `id`.
3. **Cart:** `POST /api/cart/{userId}/items` con el producto, cantidad 2 → 200. Aclarar que Cart
   consultó a Products por HTTP para validar el stock.
4. **Orders:** `POST /api/orders` → 201, con `precioUnitario` y `total` calculados. Aclarar que Orders
   consultó a Users y a Products.
5. **Orders:** `PUT /api/orders/{id}/status` con `Confirmada` → 200.
6. **Notifications:** `POST /api/notifications/send` y luego `GET /api/notifications/{userId}`.

## Demo de errores

Un error por servicio. En cada uno, mostrar el JSON completo y señalar `errorCode`, `errorMessage` y `correlationId`.

| Servicio | Request | Resultado |
|---|---|---|
| Products | `GET /api/products/99` | 404 `PRD-001` |
| Products | `DELETE` del producto con la orden confirmada | 409 `PRD-004` |
| Users | login con contraseña incorrecta, tres veces | 401 `USR-003`, 401, y 403 `USR-004` al tercero |
| Orders | `POST` con más cantidad que el stock | 422 `ORD-005` |
| Orders | `PUT` de `Entregada` a `Pendiente` sobre una orden entregada | 409 `ORD-006` |
| Cart | `POST` con una cantidad que supera el stock | 422 `CRT-003` |
| Notifications | `POST` con un `usuarioId` inexistente | 404 `NTF-001` |

## Logs y Correlation ID 

1. Hacer un `POST /api/orders`.
2. Copiar el `X-Correlation-Id` de los headers de la respuesta.
3. Mostrar que ese mismo id aparece en la consola (o en el archivo `logs/`) de **Orders, Users y
   Products**: es la prueba de que viaja entre servicios.
4. Mostrar un log de error de negocio (nivel `WRN`) y explicar la diferencia con uno inesperado (`ERR`).

## Health checks 

1. `GET /health` en Orders → `Healthy` con el detalle de sus dependencias.
2. **Apagar Products.** `GET /health/ready` en Orders → `Degraded`. `GET /health/live` sigue `Healthy`.
3. Explicar por qué: *live* dice si el proceso está vivo, *ready* si puede atender con sus dependencias.
4. Volver a levantar Products y repetir.

## Plan B si algo falla

- Si un servicio no levanta, mostrar su Swagger y los ejemplos de error documentados.
- Tener las **capturas** de `docs/capturas/` abiertas para mostrarlas si la demo en vivo se cae.
- Si falla Swagger "Try it out", usar `docs/pruebas-integrales.http`.