using Amazon.SQS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MessageQueue;

public static class ServiceCollectionExtensions
{
    public static void AddMessageQueue(this IServiceCollection services)
    {
        services.AddSingleton<IAmazonSQS>(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();

            var serviceUrl = configuration["AWS_SQS_SERVICE_URL"]
                             ?? throw new InvalidOperationException(
                                 "Required configuration AWS_SQS_SERVICE_URL not found");

            var regionEndpoint = configuration["AWS_DEFAULT_REGION"]
                                 ?? throw new InvalidOperationException(
                                     "Required configuration AWS_DEFAULT_REGION not found");

            return new AmazonSQSClient(new AmazonSQSConfig
            {
                ServiceURL = serviceUrl,
                AuthenticationRegion = regionEndpoint
            });
        });

        services.AddOptions<MessageQueueOptions>()
            .Configure<IConfiguration>((options, configuration) =>
            {
                var receiveVideoQueueUrl = configuration["RECEIVE_VIDEO_QUEUE"]
                                           ?? throw new InvalidOperationException(
                                               "Required configuration RECEIVE_VIDEO_QUEUE not found");

                var processVideoQueueUrl = configuration["PROCESS_VIDEO_QUEUE"]
                                           ?? throw new InvalidOperationException(
                                               "Required configuration PROCESS_VIDEO_QUEUE not found");

                var recognizeFrameQueueUrl = configuration["RECOGNIZE_FRAME_QUEUE"]
                                             ?? throw new InvalidOperationException(
                                                 "Required configuration RECOGNIZE_FRAME_QUEUE not found");

                options.ReceiveVideoQueueUrl = new Uri(receiveVideoQueueUrl);
                options.ProcessVideoQueueUrl = new Uri(processVideoQueueUrl);
                options.RecognizeFrameQueueUrl = new Uri(recognizeFrameQueueUrl);
            })
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton<IMessageQueueClient, MessageQueueClient>();
    }
}