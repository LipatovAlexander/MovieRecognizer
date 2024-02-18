namespace Domain.Entities;

public class File(string externalId) : BaseEntity
{
    public string ExternalId { get; set; } = externalId;
}