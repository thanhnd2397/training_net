namespace Training.Application.Common.Utils;

public class EncryptUtil
{
    /// <summary>
    /// Hash password với BCrypt
    /// </summary>
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    /// <summary>
    /// Kiểm tra password với hash
    /// </summary>
    public static bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}