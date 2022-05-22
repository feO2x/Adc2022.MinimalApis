using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Synnotech.AspNetCore.MinimalApis.Responses;
using Xunit.Abstractions;

namespace MinimalApis.RealWorldApp.Tests.TestHelpers;

public static class ResultExtensions
{
    public static void WriteBodyAsJson(this ITestOutputHelper output, IResult result) =>
        output.WriteLine(result.GetBody().SerializeToJson());

    public static void ShouldBe201Created(this IResult result) =>
        result.GetStatusCode().Should().Be(StatusCodes.Status201Created);

    public static void ShouldBe400BadRequest(this IResult result) =>
        result.GetStatusCode().Should().Be(StatusCodes.Status400BadRequest);
}