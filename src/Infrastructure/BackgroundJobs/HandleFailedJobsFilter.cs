using Application;
using Domain;
using Domain.Entities;
using Hangfire.Common;
using Hangfire.States;
using Hangfire.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.BackgroundJobs;

public class HandleFailedJobsFilter(IServiceScopeFactory serviceScopeFactory) : JobFilterAttribute, IApplyStateFilter
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    
    public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
    {
        if (context.NewState is not FailedState)
        {
            return;
        }
        
        var movieRecognitionId = (Guid)context.BackgroundJob.Job.Args[0];

        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        
        var movieRecognition = dbContext.MovieRecognitions
            .FirstOrDefault(Specification.ById<MovieRecognition>(movieRecognitionId));

        if (movieRecognition is null)
        {
            return;
        }
        
        movieRecognition.Status = MovieRecognitionStatus.Failed;
        dbContext.SaveChanges();
    }

    public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
    {
    }
}