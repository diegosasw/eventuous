using Eventuous;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Modular.Clients.Domain;
using MongoDB.Driver;
using Xunit.Abstractions;

namespace Modular.Api.FunctionalTests.TestSupport;

public abstract class FunctionalTest
    : IDisposable {
    private readonly Action _cleanTask;
    protected HttpClient HttpClient { get; }
    protected ITestOutputHelper OutputHelper { get; }

    protected FunctionalTest(ITestOutputHelper helper) {
        OutputHelper = helper;
        var testId = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 6);

        var webApplicationFactory =
            new WebApplicationFactory<Program>()
                .WithWebHostBuilder(webHostBuilder => webHostBuilder.UseEnvironment("Development")
                .ConfigureTestServices(services => TestServices(services, testId)));

        HttpClient = webApplicationFactory.CreateClient();
        var services = webApplicationFactory.Services;

        _cleanTask = () => {
            var mongoClient = services.GetRequiredService<MongoClient>();
            mongoClient.DropDatabase(testId);
        };
    }

    private void TestServices(IServiceCollection services, string testId) {
        _ = services.RemoveRegisteredService<StreamNameMap>();

        var streamNameMapTest = new StreamNameMap();
        streamNameMapTest.Register<ClientId>(clientId => new StreamName($"Client-{clientId.Value}-{testId}"));
        services.AddSingleton(streamNameMapTest);

        _ = services.RemoveRegisteredService<IMongoDatabase>();
        services.AddSingleton(
            sp => {
                var client = sp.GetRequiredService<MongoClient>();
                var mongoDatabase = client.GetDatabase(testId);
                return mongoDatabase;
            });
    }

    public void Dispose() {
        HttpClient.Dispose();
        _cleanTask.Invoke();
    }
}

internal static class ServiceCollectionExtensions {
    public static bool RemoveRegisteredService<TService>(this IServiceCollection services) {
        var serviceDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(TService));
        if (serviceDescriptor != null)
        {
            return services.Remove(serviceDescriptor);
        }

        return false;
    }
}