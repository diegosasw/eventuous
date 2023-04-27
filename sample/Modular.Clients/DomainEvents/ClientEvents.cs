using Eventuous;

namespace Modular.Clients.DomainEvents;

public static class ClientEvents
{
    public static class V1
    {
        [EventType("V1.ClientCreated")]
        public record ClientCreated(
            string Id,
            string DisplayName,
            string AdminEmail);
        
        [EventType("V1.ClientAdminCredentialsUpdated")]
        public record ClientAdminCredentialsUpdated(
            string PasswordHash,
            string PasswordHashSalt,
            string HashAlgorithmName);
    }
}