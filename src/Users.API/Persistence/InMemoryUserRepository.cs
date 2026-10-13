using System.Collections.Concurrent;
using Users.API.Models;

namespace Users.API.Persistence;

/// <summary>
/// Implementación temporal en memoria (los datos se pierden al reiniciar).
/// </summary>
public class InMemoryUserRepository : IUserRepository
{
    private readonly ConcurrentDictionary<Guid, User> _users = new();

    public Task<User?> GetByIdAsync(Guid id)
    {
        _users.TryGetValue(id, out var user);
        return Task.FromResult<User?>(user);
    }

    public Task<User?> GetByEmailAsync(string email)
    {
        var user = _users.Values.FirstOrDefault(u =>
            u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult<User?>(user);
    }

    public Task AddAsync(User user)
    {
        _users[user.Id] = user;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(User user)
    {
        _users[user.Id] = user;
        return Task.CompletedTask;
    }
}
