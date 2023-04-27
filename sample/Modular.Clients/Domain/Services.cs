namespace Modular.Clients.Domain;

public static class Services
{
    public delegate HashResult HashPbkdf2(string password, string? saltHex = null);
}

public record HashResult
{
    public string Hash { get; init; } = string.Empty;

    public string Salt { get; init; } = string.Empty;

    public string HashAlgorithmName { get; init; } = string.Empty;

    public HashResult(string hash, string salt, string hashAlgorithmName)
    {
        Hash = hash;
        Salt = salt;
        HashAlgorithmName = hashAlgorithmName;
    }
}