using System.Security.Cryptography;

namespace Users.API.Services;

/// <summary>
/// Genera y verifica el hash de las contraseñas con PBKDF2 y una sal aleatoria por usuario.
/// Nunca se guarda ni se devuelve la contraseña en texto plano.
/// El resultado se guarda como "iteraciones.sal.hash" (sal y hash en Base64).
/// </summary>
public static class PasswordHasher
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 100_000;

    public static string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var key = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, KeySize);

        return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(key)}";
    }

    /// <summary>Compara una contraseña ingresada contra el hash guardado.</summary>
    public static bool Verify(string password, string storedHash)
    {
        var parts = storedHash.Split('.');
        if (parts.Length != 3 || !int.TryParse(parts[0], out var iterations))
            return false;

        var salt = Convert.FromBase64String(parts[1]);
        var expected = Convert.FromBase64String(parts[2]);

        // Se recalcula el hash con la misma sal y se compara en tiempo constante.
        var actual = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, expected.Length);
        return CryptographicOperations.FixedTimeEquals(actual, expected);
    }
}
