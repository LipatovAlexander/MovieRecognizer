using Domain.Entities;
using WebApi.Models;

namespace WebApi.Mappers;

public static class JobMapper
{
    public static JobDto ToDto(this Job job)
    {
        return new JobDto
        {
            Id = job.Id,
            Type = job.Type,
            ExternalId = job.ExternalId,
            ParentJobId = job.ParentJob?.Id
        };
    }
}