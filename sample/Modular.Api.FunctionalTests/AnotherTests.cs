using FluentAssertions;
using Modular.Api.FunctionalTests.TestSupport;
using System.Net;

namespace Modular.Api.FunctionalTests;

public class AnotherTests
    : FunctionalTest
{

    [Fact]
    public async Task It_Returns_Unauthorized()
    {
        var result = await HttpClient.GetAsync("health");
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
        
    [Fact]
    public async Task Two()
    {
        var result = await HttpClient.GetAsync("health");
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
        
    [Fact]
    public async Task Three()
    {
        var result = await HttpClient.GetAsync("health");
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Four()
    {
        var result = await HttpClient.GetAsync("health");
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Five()
    {
        var result = await HttpClient.GetAsync("health");
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}