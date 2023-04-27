using Modular.Clients.Domain;
using System.Security.Cryptography;
using System.Text;

namespace Modular.Api.Infrastructure;

public static class HashingService
{
    public static HashResult HashPbkdf2(string password, string? saltHex = default)
    {
        const int keySize = 64;
        const int interactions = 350000;
        var hashAlgorithm = HashAlgorithmName.SHA256;
        var salt =
            saltHex is null
                ? RandomNumberGenerator.GetBytes(keySize)
                : Convert.FromHexString(saltHex);
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var hash = Rfc2898DeriveBytes.Pbkdf2(passwordBytes, salt, interactions, hashAlgorithm, keySize);
        var hashText = Convert.ToHexString(hash);
        var saltText = saltHex ?? Convert.ToHexString(salt);

        var result = new HashResult(hashText, saltText, hashAlgorithm.Name!);
        return result;
    }
}