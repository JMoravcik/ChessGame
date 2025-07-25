using System.Security.Cryptography;

namespace ChessGame.Common.Extensions;

public static class CryptExtensions
{
    public static string ToSHA256(this string input)
    {
        using var sha256 = SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(input);
        var hash = sha256.ComputeHash(bytes);
        var hexString = Convert.ToHexString(hash);
        var hexStringLower = hexString.ToLower();
        return hexStringLower;
    }

    public static Guid ToHashGuid(this string input)
    {
        using var sha256 = SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(input);
        var hash = sha256.ComputeHash(bytes);
        return new Guid(hash[..16]);
    }
}
