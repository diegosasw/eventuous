namespace Modular.Clients.Commands; 

public static class ClientCommandsHttp {
	public record CreateClientHttp(
		string Id,
		string Name,
		string AdminEmail
	);

	public record UpdateClientAdminCredentialsHttp(
		string Id,
		string Password
	);
}