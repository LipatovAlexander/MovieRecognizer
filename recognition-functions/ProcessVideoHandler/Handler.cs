using System.Text;
using CliWrap;
using CliWrap.Builders;
using CliWrap.Exceptions;
using CloudFunctions;
using CloudFunctions.MessageQueue;
using Data;
using Domain;
using Files;
using MessageQueue.Messages;
using Microsoft.Extensions.DependencyInjection;
using Ydb.Sdk.Services.Table;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace ProcessVideoHandler;

public class Handler : IHandler<MessageQueueEvent>
{
    private readonly IYandexDbService _yandexDbService;
    private readonly IDatabaseContext _databaseContext;
    private readonly YoutubeClient _youtubeClient;
    private readonly IFileStorage _fileStorage;

    public Handler()
    {
        var services = new ServiceProviderBuilder().BuildServices();

        _yandexDbService = services.GetRequiredService<IYandexDbService>();
        _databaseContext = services.GetRequiredService<IDatabaseContext>();
        _youtubeClient = services.GetRequiredService<YoutubeClient>();
        _fileStorage = services.GetRequiredService<IFileStorage>();
    }

    public async Task FunctionHandler(MessageQueueEvent messageQueueEvent)
    {
        await _yandexDbService.InitializeAsync();

        var messages = messageQueueEvent.GetMessages<ProcessVideoMessage>();

        foreach (var message in messages)
        {
            var video = await _databaseContext.ExecuteAsync(async session =>
            {
                var (video, _) = await session.Videos.GetAsync(
                    message.VideoId,
                    TxControl.BeginSerializableRW().Commit());

                return video;
            });

            using var videoFile = await DownloadVideoAsync(video);

            var timestamp = TimeSpan.Zero;

            while (timestamp < video.Duration)
            {
                using var snapshot = await SnapshotAsync(videoFile, timestamp);
                await _fileStorage.UploadAsync(snapshot, snapshot.FileName);

                var videoFrame = new VideoFrame(video.Id, timestamp, snapshot.FileName);

                await _databaseContext.ExecuteAsync(async session =>
                {
                    await session.VideoFrames.SaveAsync(videoFrame, TxControl.BeginSerializableRW().Commit());
                });

                timestamp += TimeSpan.FromSeconds(3);
            }
        }
    }

    private async Task<TempFile> DownloadVideoAsync(Video video)
    {
        var streamManifest = await _youtubeClient.Videos.Streams.GetManifestAsync(video.ExternalId);

        var stream = streamManifest.GetMuxedStreams().GetWithHighestVideoQuality();

        var tempFile = new TempFile(stream.Container.Name);

        await _youtubeClient.Videos.Streams.DownloadAsync(stream, tempFile.FilePath);

        return tempFile;
    }

    private static async Task<TempFile> SnapshotAsync(TempFile videoFile, TimeSpan timestamp)
    {
        var tempFile = new TempFile("png");

        var arguments = new ArgumentsBuilder();

        arguments.Add("-ss").Add(timestamp);
        arguments.Add("-i").Add(videoFile.FilePath);
        arguments.Add("-frames:v").Add("1");

        arguments
            .Add("-nostdin")
            .Add("-y");

        arguments.Add(tempFile.FilePath);

        try
        {
            await ExecuteCommandAsync("ffmpeg", arguments.Build());
            return tempFile;
        }
        catch (Exception)
        {
            tempFile.Dispose();
            throw;
        }
    }

    private static async ValueTask<string> ExecuteCommandAsync(string filePath, string arguments)
    {
        var stdErrBuffer = new StringBuilder();
        var stdOutBuffer = new StringBuilder();

        var stdErrPipe = PipeTarget.ToStringBuilder(stdErrBuffer);
        var stdOutPipe = PipeTarget.ToStringBuilder(stdOutBuffer);

        try
        {
            await Cli.Wrap(filePath)
                .WithArguments(arguments)
                .WithStandardErrorPipe(stdErrPipe)
                .WithStandardOutputPipe(stdOutPipe)
                .ExecuteAsync();

            return stdOutBuffer.ToString();
        }
        catch (CommandExecutionException ex)
        {
            throw new InvalidOperationException(
                $"""
                 Command-line tool failed with an error.

                 Command: {filePath} {arguments}

                 Standard error:
                 {stdErrBuffer}
                 """,
                ex
            );
        }
    }
}