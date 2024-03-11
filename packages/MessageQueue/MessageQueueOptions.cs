using System.ComponentModel.DataAnnotations;

namespace MessageQueue;

public class MessageQueueOptions
{
    [Required] public required Uri ReceiveVideoQueueUrl { get; set; }
}