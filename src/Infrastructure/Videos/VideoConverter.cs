using System.Text;
using Application.Files;
using Application.Videos;
using CliWrap;
using CliWrap.Builders;
using CliWrap.Exceptions;

namespace Infrastructure.Videos;

public class VideoConverter : IVideoConverter
{
    public async Task<TempFile> SnapshotAsync(TempFile videoFile, TimeSpan timestamp, CancellationToken cancellationToken)
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
            await ExecuteAsync("ffmpeg", arguments.Build(), cancellationToken);
            return tempFile;
        }
        catch (Exception)
        {
            tempFile.Dispose();
            throw;
        }
    }

    private static async ValueTask<string> ExecuteAsync(string filePath, string arguments, CancellationToken cancellationToken)
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
                .ExecuteAsync(cancellationToken);

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