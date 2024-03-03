namespace Domain.Entities;

public class Job(string externalId, string type) : BaseEntity
{
    public string ExternalId { get; set; } = externalId;

    public string Type { get; set; } = type;

    public required MovieRecognition MovieRecognition { get; set; }
    
    public Job? ParentJob { get; set; }

    public static Specification<Job> WithExternalId(string externalId) => new(job => job.ExternalId == externalId);
}