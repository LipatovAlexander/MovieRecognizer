using Application.Commands;
using Hangfire;

namespace Application.Extensions;

public static class BackgroundJobClientExtensions
{
    public static string ContinueWithCommand<TCommand>(
        this IBackgroundJobClient backgroundJobClient, string parentId, TCommand command)
        where TCommand : ICommand
    {
        return backgroundJobClient.ContinueJobWith<ICommandHandler<TCommand>>(
            parentId,
            handler => handler.HandleAsync(command, CancellationToken.None));
    }

    public static string EnqueueCommand<TCommand>(this IBackgroundJobClient backgroundJobClient, TCommand command)
        where TCommand : ICommand
    {
        return backgroundJobClient.Enqueue<ICommandHandler<TCommand>>(
            handler => handler.HandleAsync(command, CancellationToken.None));
    }
}