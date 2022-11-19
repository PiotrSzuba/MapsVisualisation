using MapsVisualisation.Domain.Entities;

namespace MapsVisualisation.Service.Features.Regions.Shared;

public class OtherSourcesDto
{
    public Guid Id { get; private set; }
    public string? Name { get; private set; }
    public string? Url { get; private set; }
    public int? Year { get; private set; }

    public static class Mapper
    {
        public static OtherSourcesDto Map(OtherSource otherSources)
        {
            return new OtherSourcesDto
            {
                Id = otherSources.Id,
                Name = otherSources.Name,
                Url = otherSources.Url,
                Year = otherSources.Year,
            };
        }
    }
}
