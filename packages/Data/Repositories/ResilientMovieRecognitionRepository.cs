using Domain;
using Polly.Registry;

namespace Data.Repositories;

public class ResilientMovieRecognitionRepository(
    ResiliencePipelineProvider<string> pipelineProvider,
    IMovieRecognitionRepository movieRecognitionRepository)
    : IMovieRecognitionRepository
{
    private readonly ResiliencePipelineProvider<string> _pipelineProvider = pipelineProvider;
    private readonly IMovieRecognitionRepository _movieRecognitionRepository = movieRecognitionRepository;

    public async Task<MovieRecognition?> GetAsync(Guid id)
    {
        var pipeline = _pipelineProvider.GetPipeline("repository-get");
        return await pipeline.ExecuteAsync(async _ => await _movieRecognitionRepository.GetAsync(id));
    }

    public async Task SaveAsync(MovieRecognition movieRecognition)
    {
        var pipeline = _pipelineProvider.GetPipeline("repository-save");
        await pipeline.ExecuteAsync(async _ => await _movieRecognitionRepository.SaveAsync(movieRecognition));
    }
}