using Eventuous;
using Eventuous.AspNetCore.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Modular.Clients.Domain;
using static Modular.Clients.Commands.ClientCommands;
using static Modular.Clients.Commands.ClientCommandsHttp;

namespace Modular.Clients;

public static class ClientCommandsApi
{
	public static WebApplication AddClientCommands(this WebApplication app) {
		app
			.MapAggregateCommands<Client>()
			.MapCommand<CreateClientHttp, CreateClient>(
				"clients/create",
				(cmd, ctx) => {
					var domainCommand =
						new CreateClient(
							new ClientId(cmd.Id),
							cmd.Name,
							cmd.AdminEmail);

					return domainCommand;
				},
				rhb => rhb.AllowAnonymous())
			.MapCommand<UpdateClientAdminCredentialsHttp, UpdateClientAdminCredentials>(
				"clients/updateAdminCredentials",
				(cmd, ctx) => {
					var domainCommand =
						new UpdateClientAdminCredentials(
							new ClientId(cmd.Id),
							cmd.Password);

					return domainCommand;
				},
				rhb => rhb.AllowAnonymous());
		
		return app;
	}
}