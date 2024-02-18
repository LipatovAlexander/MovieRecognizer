using WebApi.Models;
using File = Domain.Entities.File;

namespace WebApi.Mappers;

public static class FileMapper
{
    public static FileDto ToDto(this File file)
    {
        return new FileDto
        {
            ExternalId = file.ExternalId
        };
    }
}