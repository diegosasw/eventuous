using Modular.Api;
using Modular.Clients;
using Serilog;
using Serilog.Events;

var serilogLogger =
    new LoggerConfiguration()
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .MinimumLevel.Override("Microsoft.AspNetCore.Hosting.Diagnostics", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
        .MinimumLevel.Override("Grpc", LogEventLevel.Information)
        .MinimumLevel.Override("EventStore", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.Console(
            outputTemplate:
            "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} <s:{SourceContext}>{NewLine}{Exception}"
        )
        .CreateLogger();

Log.Logger = serilogLogger;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

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
app.UseSerilogRequestLogging();
app.UseEventuousLogs();
app.UsePathBase(assemblyPath);
        
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSerilogRequestLogging();
app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI(options => options.SwaggerEndpoint(openApiRelativePath, openApiName));
//app.UseAuthentication();
//app.UseAuthorization();
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