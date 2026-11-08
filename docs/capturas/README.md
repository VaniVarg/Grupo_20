# Capturas de Swagger UI

Capturas de Swagger UI mostrando las respuestas de **error con `errorCode` y `errorMessage`**, como pide la entrega.

## Swagger de cada servicio

| Servicio | Captura | Debe verse |
|---|---|---|
| Products.API | ![Products](swagger-products.png) | Los cinco endpoints con sus respuestas |
| Users.API | ![Users](swagger-users.png) | Los tres endpoints con sus respuestas |
| Orders.API | ![Orders](swagger-orders.png) | Los cuatro endpoints con sus respuestas |
| Cart.API | ![Cart](swagger-cart.png) | Los cinco endpoints con sus respuestas |
| Notifications.API | ![Notifications](swagger-notifications.png) | Los dos endpoints con sus respuestas |

## Errores ejecutados con "Try it out"

| Error | Captura | Request |
|---|---|---|
| PRD-001 (404) | ![PRD-001](error-prd-001.png) | `GET /api/products/99` |
| PRD-003 (409) | ![PRD-003](error-prd-003.png) | `POST /api/products` con un producto repetido |
| PRD-004 (409) | ![PRD-004](error-prd-004.png) | `DELETE` de un producto con una orden activa |
| USR-003 (401) | ![USR-003](error-usr-003.png) | `POST /api/users/login` con contraseña incorrecta |
| USR-004 (403) | ![USR-004](error-usr-004.png) | Tercer login fallido consecutivo |
| ORD-005 (422) | ![ORD-005](error-ord-005.png) | `POST /api/orders` con más cantidad que el stock |
| ORD-006 (409) | ![ORD-006](error-ord-006.png) | `PUT /api/orders/{id}/status` con una transición no permitida |
| CRT-003 (422) | ![CRT-003](error-crt-003.png) | `POST /api/cart/{userId}/items` con más cantidad que el stock |
| NTF-001 (404) | ![NTF-001](error-ntf-001.png) | `POST /api/notifications/send` con un usuario inexistente |

En cada captura de error tiene que verse el **código de respuesta**, el **cuerpo JSON** con `errorCode`,
`errorMessage` y `correlationId`, y los **headers** de la respuesta con `x-correlation-id`.
