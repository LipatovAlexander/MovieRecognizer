using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Domain;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Movies.IntegrationTests;

public sealed class FindMovieEndpointTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory = factory;

    [Fact]
    public async Task ShouldReturnMovieInfo_WhenMovieFound()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var httpResponse = await client.GetAsync("/movie?title=Imitation game");

        // Assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        var apiResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<Movie>>(jsonSerializerOptions);

        apiResponse.Should().NotBeNull();
        apiResponse?.Ok.Should().BeTrue();
        apiResponse?.Value.Should().Match<Movie>(x =>
            x.Title == "The Imitation Game"
            && x.Year == "2014"
            && x.Genres.SequenceEqual(new[] { "Biography", "Drama", "Thriller" })
            && x.Actors.SequenceEqual(new[] { "Benedict Cumberbatch", "Keira Knightley", "Matthew Goode" })
            && x.Plot == "During World War II, the English mathematical genius Alan Turing tries to crack the German Enigma code with help from fellow mathematicians while attempting to come to terms with his troubled private life."
            && x.Country == "United Kingdom, United States"
            && x.PosterUri.IsWellFormedOriginalString()
            && x.Type == MovieType.Movie);
    }

    [Fact]
    public async Task ShouldReturnNotFound_WhenMovieNotFound()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var httpResponse = await client.GetAsync("/movie?title=asdf");

        // Assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
        var jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        var apiResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<Movie>>(jsonSerializerOptions);

        apiResponse.Should().NotBeNull();
        apiResponse?.Ok.Should().BeFalse();
        apiResponse?.Code.Should().Be("not_found");
    }

    [Theory]
    [InlineData("/movie?title=")]
    [InlineData("/movie")]
    public async Task ShouldReturnBadRequest_WhenTitleNotPassed(string uri)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var httpResponse = await client.GetAsync(uri);

        // Assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        var apiResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<Movie>>(jsonSerializerOptions);

        apiResponse.Should().NotBeNull();
        apiResponse?.Ok.Should().BeFalse();
        apiResponse?.Code.Should().Be("invalid_request");
    }
}
