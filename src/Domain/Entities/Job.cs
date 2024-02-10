namespace Domain.Entities;

public class Job(string externalId, string type) : BaseEntity
{
    public string ExternalId { get; set; } = externalId;

    public string Type { get; set; } = type;

    public required MovieRecognition MovieRecognition { get; set; }
}