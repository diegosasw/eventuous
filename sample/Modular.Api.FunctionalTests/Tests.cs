using FluentAssertions;
using Modular.Api.FunctionalTests.TestSupport;
using System.Net;

namespace Modular.Api.FunctionalTests;

public class Tests
    : FunctionalTest
{
    [Fact]
    public async Task It_Returns_200_Ok()
    {
        var result = await HttpClient.GetAsync("health");
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
        
    [Fact]
    public async Task It_Shows_Environment_Is_Development()
    {
        var result = await HttpClient.GetAsync("health");
        var body = await result.Content.ReadAsStringAsync();
        body.Should().Contain("Environment=Development");
    }
}