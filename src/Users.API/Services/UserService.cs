using Users.API.DTOs;
using Users.API.Exceptions;
using Users.API.Models;
using Users.API.Persistence;

namespace Users.API.Services;

public class UserService(IUserRepository repository) : IUserService
{
    /// <summary>Cantidad de logins fallidos consecutivos que bloquean la cuenta.</summary>
    private const int MaxIntentosFallidos = 3;

    public async Task<UserResponse> RegisterAsync(RegisterRequest request)
    {
        RequestValidator.Validate(request, ErrorCodes.USR_002, "Los datos del usuario son inválidos.");

        // El email se guarda en minúsculas para que "Maria@x.com" y "maria@x.com" sean el mismo.
        var email = request.Email.Trim().ToLowerInvariant();

        if (await repository.GetByEmailAsync(email) is not null)
            throw new BusinessRuleException(ErrorCodes.USR_001, $"El email '{email}' ya está registrado.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Nombre = request.Nombre.Trim(),
            Apellido = request.Apellido.Trim(),
            Email = email,
            PasswordHash = PasswordHasher.Hash(request.Password),
            FechaRegistro = DateTime.UtcNow,
            Activo = true,
            IntentosFallidos = 0
        };

        await repository.AddAsync(user);
        return ToResponse(user);
    }

    public async Task<UserResponse> GetByIdAsync(Guid id)
    {
        var user = await repository.GetByIdAsync(id)
            ?? throw new NotFoundException(ErrorCodes.USR_007, "Usuario no encontrado.");

        return ToResponse(user);
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        RequestValidator.Validate(request, ErrorCodes.USR_002, "Los datos del usuario son inválidos.");

        var email = request.Email.Trim().ToLowerInvariant();

        // Si el email no existe respondemos lo mismo que con una contraseña incorrecta (USR-003),
        // para no revelar qué emails están registrados.
        var user = await repository.GetByEmailAsync(email)
            ?? throw new UnauthorizedException(ErrorCodes.USR_003, "Credenciales incorrectas.");

        // Cuenta ya bloqueada: ni siquiera se compara la contraseña.
        if (!user.Activo)
            throw CuentaBloqueada(user);

        if (!PasswordHasher.Verify(request.Password, user.PasswordHash))
        {
            user.IntentosFallidos++;

            // Al llegar a 3 intentos fallidos consecutivos la cuenta queda bloqueada.
            if (user.IntentosFallidos >= MaxIntentosFallidos)
                user.Activo = false;

            await repository.UpdateAsync(user);

            // El tercer intento fallido ya responde 403 (USR-004); los dos primeros, 401 (USR-003).
            if (!user.Activo)
                throw CuentaBloqueada(user);

            throw new UnauthorizedException(ErrorCodes.USR_003, "Credenciales incorrectas.");
        }

        // Login correcto: el contador de intentos fallidos vuelve a cero.
        if (user.IntentosFallidos != 0)
        {
            user.IntentosFallidos = 0;
            await repository.UpdateAsync(user);
        }

        return new LoginResponse
        {
            Id = user.Id,
            Nombre = user.Nombre,
            Apellido = user.Apellido,
            Email = user.Email
        };
    }

    /// <summary>
    /// Devuelve la excepción que corresponde a una cuenta bloqueada:
    /// USR-004 si fue por intentos fallidos, USR-005 si fue un bloqueo manual por fraude.
    /// </summary>
    private static ForbiddenException CuentaBloqueada(User user)
    {
        if (user.IntentosFallidos >= MaxIntentosFallidos)
        {
            return new ForbiddenException(
                ErrorCodes.USR_004,
                "Su cuenta fue bloqueada por superar el máximo de intentos fallidos. Contacte a soporte.");
        }

        return new ForbiddenException(
            ErrorCodes.USR_005,
            "Su cuenta fue suspendida por razones de seguridad. Contacte a soporte.");
    }

    private static UserResponse ToResponse(User u) => new()
    {
        Id = u.Id,
        Nombre = u.Nombre,
        Apellido = u.Apellido,
        Email = u.Email,
        FechaRegistro = u.FechaRegistro,
        Activo = u.Activo
    };
}
