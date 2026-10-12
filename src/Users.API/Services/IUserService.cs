using Users.API.DTOs;

namespace Users.API.Services;

/// <summary>
/// Lógica de negocio de usuarios.
/// </summary>
public interface IUserService
{
    Task<UserResponse> RegisterAsync(RegisterRequest request);
    Task<UserResponse> GetByIdAsync(Guid id);
}
