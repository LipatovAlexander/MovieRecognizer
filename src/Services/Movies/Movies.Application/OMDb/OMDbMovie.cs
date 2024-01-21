namespace Movies.Application.OMDb;

public sealed class OMDbMovie
{
    public string Title { get; set; } = default!;
    public string Year { get; set; } = default!;
    public string Genre { get; set; } = default!;
    public string Actors { get; set; } = default!;
    public string Plot { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string Poster { get; set; } = default!;
    public string Type { get; set; } = default!;
    public required string Response { get; set; }
}
