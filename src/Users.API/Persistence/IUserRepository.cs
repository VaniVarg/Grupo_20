using Users.API.Models;

namespace Users.API.Persistence;

/// <summary>
/// Contrato de acceso a datos de usuarios. El Service solo conoce esta interfaz.
/// </summary>
public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
}
