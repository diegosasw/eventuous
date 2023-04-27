using Eventuous;
using static Modular.Clients.DomainEvents.ClientEvents;

namespace Modular.Clients.Domain;

public class Client
    : Aggregate<ClientState>
{
    public void CreateClient(string id, string displayName, string adminEmail)
    {
        EnsureDoesntExist();
        var clientCreated = new V1.ClientCreated(id, displayName, adminEmail);
        Apply(clientCreated);
    }

    public void UpdateAdminCredentials(string password, Services.HashPbkdf2 hashPbkdf2Service)
    {
        EnsureExists();
        var hashResult = hashPbkdf2Service(password);
        var clientAdminCredentialsUpdated = 
            new V1.ClientAdminCredentialsUpdated(
                hashResult.Hash,
                hashResult.Salt,
                hashResult.HashAlgorithmName);
        Apply(clientAdminCredentialsUpdated);
    }
}