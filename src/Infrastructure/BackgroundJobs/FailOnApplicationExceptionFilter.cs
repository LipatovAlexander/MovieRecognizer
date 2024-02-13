using Hangfire.Common;
using Hangfire.States;
using ApplicationException = Application.Exceptions.ApplicationException;

namespace Infrastructure.BackgroundJobs;

public class FailOnApplicationExceptionFilter : JobFilterAttribute, IElectStateFilter
{
    public void OnStateElection(ElectStateContext context)
    {
        if (context.CandidateState is not FailedState failedState)
        {
            return;
        }
        
        var exceptionType = failedState.Exception.GetType();

        if (exceptionType.IsAssignableTo(typeof(ApplicationException)))
        {
            return;
        }

        context.CandidateState = new FailedState(new BackgroundJobExecutionException(failedState.Exception));
    }
}