using Modular.Api;
using Modular.Clients;
var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var services = builder.Services;

services
    .AddEndpointsApiExplorer()
    .AddCore(configuration)
    .AddModules(configuration)
    .AddOpenApi()
    .AddControllers();

var assemblyPath = $"/{typeof(Modular.Api.Program).Assembly.GetName().Name!.ToLowerInvariant()}";
var openApiRelativePath = $"{assemblyPath}/swagger/v1/swagger.json";
var openApiName = "Eventuous Modular";

var app = builder.Build();
app.UsePathBase(assemblyPath);
        
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.AddClientCommands();
app.MapGet("/health", () =>
{
    var environmentName = app.Environment.EnvironmentName;
    var currentUtcDateTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
    var assemblyName = typeof(Program).Assembly.GetName();
    var result = $"Assembly={assemblyName}, Environment={environmentName}, CurrentUtcTime={currentUtcDateTime}";
    return Results.Ok(result);
});
app.Run();


namespace Modular.Api
{
    public partial class Program { }
}