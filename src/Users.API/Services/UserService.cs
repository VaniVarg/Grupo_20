using Users.API.DTOs;
using Users.API.Exceptions;
using Users.API.Models;
using Users.API.Persistence;

namespace Users.API.Services;

public class UserService(IUserRepository repository) : IUserService
{
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

    // Nótese que UserResponse no tiene PasswordHash: nunca sale en una respuesta.
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
