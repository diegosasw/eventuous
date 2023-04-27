using Eventuous.Projections.MongoDB;
using Eventuous.Subscriptions.Context;
using MongoDB.Driver;
using static Modular.Clients.DomainEvents.ClientEvents;

// ReSharper disable ClassNeverInstantiated.Global

namespace Modular.ClientsInfo;

public sealed class ClientProjection
    : MongoProjector<ClientDocument>
{
    public ClientProjection(IMongoDatabase database) 
        : base(database)
    {
        On<V1.ClientCreated>(Handler);
    }

    private ValueTask<MongoProjectOperation<ClientDocument>> Handler(
        IMessageConsumeContext<V1.ClientCreated> ctx)
    {
        var clientCreated = ctx.Message;
        var filter = Builders<ClientDocument>.Filter.Eq(x => x.Id, clientCreated.Id);

        var document =
            new ClientDocument(clientCreated.Id)
            {
                Name       = clientCreated.DisplayName,
                AdminEmail = clientCreated.AdminEmail,
                CreatedOn  = ctx.Created
            };

        var operation =
            new MongoProjectOperation<ClientDocument>(async (collection, cancellationToken) =>
            {
                var replaceOptions = new ReplaceOptions { IsUpsert = true };
                await collection.ReplaceOneAsync(filter, document, replaceOptions, cancellationToken);
            });

        return ValueTask.FromResult(operation);
    }
}