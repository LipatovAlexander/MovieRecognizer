using System.Net;
using Domain;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Videos.IntegrationTests;

public sealed class YouTubeEndpointTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task ShouldReturnVideoInfo_WhenVideoFound()
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var httpResponse = await client.GetAsync("/youtube?uri=https://youtube.com/watch?v=dQw4w9WgXcQ");

        // Assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var apiResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<Video>>();
        apiResponse.Should().NotBeNull();
        apiResponse?.Ok.Should().BeTrue();
        apiResponse?.Value.Should().Match<Video>(x =>
            x.Title == "Rick Astley - Never Gonna Give You Up (Official Music Video)"
            && x.Author == "Rick Astley"
            && x.FileExtension == ".mp4"
            && x.LengthSeconds == 212
            && x.Uri.IsWellFormedOriginalString());
    }
    
    [Fact]
    public async Task ShouldReturnNotFound_WhenVideoNotFound()
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var httpResponse = await client.GetAsync("/youtube?uri=https://youtube.com/watch?v=69");

        // Assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
        var apiResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<Video>>();
        apiResponse.Should().NotBeNull();
        apiResponse?.Ok.Should().BeFalse();
        apiResponse?.Code.Should().Be("not_found");
    }
    
    [Fact]
    public async Task ShouldReturnBadRequest_WhenUriInvalid()
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var httpResponse = await client.GetAsync("/youtube?uri=69");

        // Assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var apiResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<Video>>();
        apiResponse.Should().NotBeNull();
        apiResponse?.Ok.Should().BeFalse();
        apiResponse?.Code.Should().Be("invalid_request");
    }
    
    [Fact]
    public async Task ShouldReturnBadRequest_WhenUriNotPassed()
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var httpResponse = await client.GetAsync("/youtube");

        // Assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var apiResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<Video>>();
        apiResponse.Should().NotBeNull();
        apiResponse?.Ok.Should().BeFalse();
        apiResponse?.Code.Should().Be("invalid_request");
    }
    
    [Fact]
    public async Task ShouldReturnBadRequest_WhenUriIsNotValidYouTubeUri()
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var httpResponse = await client.GetAsync("/youtube?uri=https://google.com");

        // Assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var apiResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<Video>>();
        apiResponse.Should().NotBeNull();
        apiResponse?.Ok.Should().BeFalse();
        apiResponse?.Code.Should().Be("invalid_request");
    }
}
