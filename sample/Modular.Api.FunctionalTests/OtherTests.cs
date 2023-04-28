using FluentAssertions;
using Modular.Api.FunctionalTests.TestSupport;
using System.Net;
using Xunit.Abstractions;

namespace Modular.Api.FunctionalTests;

public class AnotherTests
    : FunctionalTest
{
    public AnotherTests(ITestOutputHelper testOutputHelper) 
        : base(testOutputHelper){
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
    [InlineData(11)]
    [InlineData(12)]
    [InlineData(13)]
    [InlineData(14)]
    [InlineData(15)]
    [InlineData(16)]
    [InlineData(17)]
    [InlineData(18)]
    [InlineData(19)]
    [InlineData(20)]
    [InlineData(21)]
    [InlineData(22)]
    [InlineData(23)]
    [InlineData(24)]
    [InlineData(25)]
    [InlineData(26)]
    [InlineData(27)]
    [InlineData(28)]
    [InlineData(29)]
    [InlineData(30)]
    [InlineData(31)]
    [InlineData(32)]
    [InlineData(33)]
    [InlineData(34)]
    [InlineData(35)]
    [InlineData(36)]
    [InlineData(37)]
    [InlineData(38)]
    [InlineData(39)]
    [InlineData(40)]
    public async Task TestInParallel(int testCaseNumber)
    {
        OutputHelper.WriteLine(testCaseNumber.ToString());
        var result = await HttpClient.GetAsync("health");
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData(38)]
    [InlineData(39)]
    [InlineData(40)]
    public async Task TestInParalle2l(int testCaseNumber) {
        OutputHelper.WriteLine(testCaseNumber.ToString());
        var result = await HttpClient.GetAsync("health");
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData(38)]
    [InlineData(39)]
    [InlineData(40)]
    public async Task TestInParalle2l3(int testCaseNumber) {
        OutputHelper.WriteLine(testCaseNumber.ToString());
        var result = await HttpClient.GetAsync("health");
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}