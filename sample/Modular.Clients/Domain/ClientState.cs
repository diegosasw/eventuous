using Eventuous;
using Modular.Clients.DomainEvents;

namespace Modular.Clients.Domain;

public record ClientState
    : State<ClientState>
{
    public string Name { get; init; } = string.Empty;
    public string AdminEmail { get; init; } = string.Empty;

    public ClientState()
    {
        On<ClientEvents.V1.ClientCreated>(HandleClientCreated);
        On<ClientEvents.V1.ClientAdminCredentialsUpdated>(HandleClientAdminCredentialsUpdated);
    }

    static ClientState HandleClientCreated(ClientState clientState, ClientEvents.V1.ClientCreated clientCreated)
        => clientState with
        {
            Name = clientCreated.DisplayName,
            AdminEmail = clientCreated.AdminEmail
        };

    static ClientState HandleClientAdminCredentialsUpdated(
        ClientState clientState,
        ClientEvents.V1.ClientAdminCredentialsUpdated clientAdminCredentialsUpdated)
        => clientState with  { };
}