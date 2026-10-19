using System.Net;

namespace Orders.API.Clients;

public class UsersClient(HttpClient httpClient, ILogger<UsersClient> logger) : IUsersClient
{
    public async Task<bool> ExistsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.GetAsync($"api/users/{userId}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return false;

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Users.API respondió {StatusCode} al consultar el usuario {UserId}",
                (int)response.StatusCode, userId);
            throw new HttpRequestException($"Users.API respondió {(int)response.StatusCode}.");
        }

        return true;
    }
}
