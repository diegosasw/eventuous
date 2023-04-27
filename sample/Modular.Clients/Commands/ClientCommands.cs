using Modular.Clients.Domain;

namespace Modular.Clients.Commands;

public static class ClientCommands
{
    public record CreateClient(
        ClientId ClientId,
        string DisplayName,
        string AdminEmail);
    
    public record UpdateClientAdminCredentials(
        ClientId ClientId,
        string Password);
}