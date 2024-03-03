namespace WebApi.Models;

public class JobDto
{
    public required Guid Id { get; set; }
    
    public required string ExternalId { get; set; }

    public required string Type { get; set; }

    public required Guid? ParentJobId { get; set; }
}