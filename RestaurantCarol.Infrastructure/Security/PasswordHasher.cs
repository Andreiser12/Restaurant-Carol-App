namespace RestaurantCarol.Infrastructure.Security;

public class PasswordHasher
{
    public static string HashPassword(string password) =>
        BCrypt.Net.BCrypt.HashPassword(password);

    public static bool VerifyPassword(string password, string hash)
    {
        if (string.IsNullOrEmpty(hash)) return false;

        if (hash.StartsWith("PLAINTEXT_"))
        {
            return hash.Substring("PLAINTEXT_".Length) == password;
        }

        try
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
        catch
        {
            return hash == password;
        }
    }
}
