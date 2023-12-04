using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Movies.IntegrationTests;

public sealed class CommonTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task ShouldReturnNotFound_WhenEndpointNotFound()
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var httpResponse = await client.GetAsync("/not_found");

        // Assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var apiResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse>();
        apiResponse.Should().NotBeNull();
        apiResponse?.Ok.Should().BeFalse();
        apiResponse?.Code.Should().Be("not_found");
    }
    
    [Fact]
    public async Task ShouldReturnInternalError_WhenErrorOccurred()
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var httpResponse = await client.GetAsync("/boom");

        // Assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

        var apiResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse>();
        apiResponse.Should().NotBeNull();
        apiResponse?.Ok.Should().BeFalse();
        apiResponse?.Code.Should().Be("internal_error");
    }
}
