using Eventuous;
using Eventuous.EventStore;
using Eventuous.EventStore.Subscriptions;
using Eventuous.Subscriptions.Checkpoints;
using Modular.Api.Infrastructure;
using Modular.Clients;
using Modular.Clients.Domain;
using Modular.Clients.DomainEvents;
using Modular.ClientsInfo;
using MongoDB.Driver;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Modular.Api;

public static class Registrations {
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration) {
        DefaultEventSerializer
            .SetDefaultSerializer(
                new DefaultEventSerializer(
                    new JsonSerializerOptions(JsonSerializerDefaults.Web)
                        .ConfigureForNodaTime(DateTimeZoneProviders.Tzdb)));

        services
            .AddEventStoreDb(configuration)
            .AddDocumentDb(configuration)
            .AddTelemetry();

        return services;
    }

    public static IServiceCollection AddOpenApi(this IServiceCollection services) {
        services.AddSwaggerGen();

        return services;
    }

    private static IServiceCollection AddEventStoreDb(this IServiceCollection services, IConfiguration configuration) {
        services.AddEventStoreClient(configuration["EventStore:ConnectionString"]!);

        return services;
    }

    private static IServiceCollection AddDocumentDb(this IServiceCollection services, IConfiguration configuration) {
        var documentDbSettings = new DocumentDbSettings();
        configuration.Bind("DocumentStore", documentDbSettings);
        var mongoClientSettings = MongoClientSettings.FromConnectionString(documentDbSettings.ConnectionString);
        //mongoClientSettings.ClusterConfigurator = cb => cb.Subscribe(new DiagnosticsActivityEventSubscriber());
        var mongoClient = new MongoClient(mongoClientSettings);
        services.AddSingleton(mongoClient);

        services.AddSingleton(sp => {
            var client = sp.GetRequiredService<MongoClient>();
            var databaseName = documentDbSettings.DatabaseName;
            var mongoDatabase = client.GetDatabase(databaseName);
            return mongoDatabase;
        });

        return services;
    }

    public static IServiceCollection AddModules(this IServiceCollection services, IConfiguration configuration) {
        services.AddAggregateStore<EsdbEventStore>();
        var streamNameMap = new StreamNameMap();
        streamNameMap.Register<ClientId>(clientId => new StreamName($"Client-{clientId.Value}"));
        services.AddSingleton(streamNameMap);
        services.AddCommandService<ClientCommandService, Client>();
        TypeMap.RegisterKnownEventTypes(typeof(ClientEvents.V1.ClientCreated).Assembly);

        services.AddSingleton<Services.HashPbkdf2>(HashingService.HashPbkdf2);

        services.AddSubscription<AllStreamSubscription, AllStreamSubscriptionOptions>(
            Constants.CatchupSubscriptions.ProjectionSubscriptionId,
            builder => {
                builder
                    .UseCheckpointStore<NoOpCheckpointStore>()
                    .AddEventHandler<ClientProjection>();
            });

        return services;
    }

    private static void AddTelemetry(this IServiceCollection services) {
    }
}

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
internal class DocumentDbSettings {
    public string Hostname { get; init; } = string.Empty;
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string DatabaseName { get; init; } = string.Empty;
    public bool IsCluster { get; init; } = false;
    private string Suffix =>
        IsCluster
            ? "/?replicaSet=rs0&readPreference=secondaryPreferred&retryWrites=false"
            : string.Empty;

    public string ConnectionString => $"mongodb://{Username}:{Password}@{Hostname}:27017{Suffix}";
}