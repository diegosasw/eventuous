using Eventuous;
using Modular.Clients.Domain;
using static Modular.Clients.Commands.ClientCommands;
// ReSharper disable ClassNeverInstantiated.Global

namespace Modular.Clients;

public class ClientCommandService
	: CommandService<Client, ClientState, ClientId>
{
	public ClientCommandService(
		IAggregateStore store,
		StreamNameMap streamNameMap,
		Services.HashPbkdf2 hashPbkdf2Service)
		: base(store, streamNameMap: streamNameMap)
	{
		OnNew<CreateClient>(
			cmd => cmd.ClientId,
			(client, cmd) => client.CreateClient(cmd.ClientId, cmd.DisplayName, cmd.AdminEmail));

		OnExisting<UpdateClientAdminCredentials>(
			cmd => cmd.ClientId,
			(client, cmd) => client.UpdateAdminCredentials(cmd.Password, hashPbkdf2Service));
	}
}
