namespace Notifications.API.Clients;

// Cliente HTTP de Users.API. Los Services dependen de esta interfaz, no de HttpClient.

public interface IUsersClient
{
    // True si el usuario existe (HTTP 200) y false si no (HTTP 404).
    Task<bool> ExistsAsync(Guid userId, CancellationToken cancellationToken = default);
}