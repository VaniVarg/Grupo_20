namespace Orders.API.Clients;

/// <summary>
/// Cliente HTTP de Users.API. Los Services dependen de esta interfaz, no de HttpClient.
/// </summary>
public interface IUsersClient
{
    /// <summary>True si el usuario existe (HTTP 200) y false si no (HTTP 404).</summary>
    Task<bool> ExistsAsync(Guid userId, CancellationToken cancellationToken = default);
}
