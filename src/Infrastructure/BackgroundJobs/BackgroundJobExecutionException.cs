namespace Infrastructure.BackgroundJobs;

public class BackgroundJobExecutionException(Exception innerException) : Exception(null, innerException);