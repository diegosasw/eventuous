using FluentAssertions;
using Modular.Api.FunctionalTests.TestSupport;
using System.Net;
using Xunit.Abstractions;

namespace Modular.Api.FunctionalTests;

public class AnotherTests
    : FunctionalTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public AnotherTests(ITestOutputHelper testOutputHelper) {
        _testOutputHelper = testOutputHelper;
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    [InlineData(10)]
    public async Task TestInParallel(int testCaseNumber)
    {
        _testOutputHelper.WriteLine(testCaseNumber.ToString());
        var result = await HttpClient.GetAsync("health");
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}