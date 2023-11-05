using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Domain;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Videos.IntegrationTests;

public sealed class GetVideoEndpointTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task ShouldReturnVideoInfo_WhenVideoFound()
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var httpResponse = await client.GetAsync("/video?uri=https://youtube.com/watch?v=dQw4w9WgXcQ");

        // Assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        var apiResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<Video>>(jsonSerializerOptions);
        
        apiResponse.Should().NotBeNull();
        apiResponse?.Ok.Should().BeTrue();
        apiResponse?.Value.Should().Match<Video>(x =>
            x.Title == "Rick Astley - Never Gonna Give You Up (Official Music Video)"
            && x.Author == "Rick Astley"
            && x.FileExtension == ".mp4"
            && x.LengthSeconds == 212
            && x.Uri.IsWellFormedOriginalString()
            && x.Source == VideoSource.YouTube);
    }
    
    [Fact]
    public async Task ShouldReturnNotFound_WhenVideoNotFound()
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var httpResponse = await client.GetAsync("/video?uri=https://youtube.com/watch?v=69");

        // Assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
        var apiResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<Video>>();
        apiResponse.Should().NotBeNull();
        apiResponse?.Ok.Should().BeFalse();
        apiResponse?.Code.Should().Be("not_found");
    }
    
    [Fact]
    public async Task ShouldReturnInvalidRequest_WhenUriInvalid()
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var httpResponse = await client.GetAsync("/video?uri=69");

        // Assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var apiResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<Video>>();
        apiResponse.Should().NotBeNull();
        apiResponse?.Ok.Should().BeFalse();
        apiResponse?.Code.Should().Be("invalid_request");
    }
    
    [Fact]
    public async Task ShouldReturnInvalidRequest_WhenUriNotPassed()
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var httpResponse = await client.GetAsync("/video");

        // Assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var apiResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<Video>>();
        apiResponse.Should().NotBeNull();
        apiResponse?.Ok.Should().BeFalse();
        apiResponse?.Code.Should().Be("invalid_request");
    }
    
    [Fact]
    public async Task ShouldReturnUnsupportedSource_WhenUriIsNotValidYouTubeUri()
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var httpResponse = await client.GetAsync("/video?uri=https://google.com");

        // Assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var apiResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<Video>>();
        apiResponse.Should().NotBeNull();
        apiResponse?.Ok.Should().BeFalse();
        apiResponse?.Code.Should().Be("unsupported_source");
    }
}
