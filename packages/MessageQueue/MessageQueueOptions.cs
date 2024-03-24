using System.ComponentModel.DataAnnotations;

namespace MessageQueue;

public class MessageQueueOptions
{
    [Required] public required Uri ReceiveVideoQueueUrl { get; set; }

    [Required] public required Uri ProcessVideoQueueUrl { get; set; }

    [Required] public required Uri RecognizeFrameQueueUrl { get; set; }
}