using MapsVisualisation.Domain.Entities;
using MapsVisualisation.Service.Services;

namespace MapsVisualisation.Service.Features.Regions.Shared;

public class RegionDto
{
    public Guid Id { get; set; }
    public string RegionIdentity { get; set; } = string.Empty;
    public string RegionName1 { get; set; } = string.Empty;
    public string RegionName2 { get; set; } = string.Empty;
    public string RegionName3 { get; set; } = string.Empty;
    public double NWLat { get; set; }
    public double NWLong { get; set; }
    public double NELat { get; set; }
    public double NELong { get; set; }
    public double SELat { get; set; }
    public double SELong { get; set; }
    public double SWLat { get; set; }
    public double SWLong { get; set; }
    public RegionType Type { get; set; }
    public List<MapDto>? Maps { get; set; }
    public List<OtherSourcesDto>? OtherSources { get; set; }

    public static class Mapper
    {
        public static List<MapDto> MapMaps(List<Map> maps, IPathProvider pathProvider, IDomainProvider domainProvider)
        {
            return maps
                .DistinctBy(map => map.Thumbnail)
                .Select(map => MapMapDto(map, pathProvider, domainProvider))
                .OrderByDescending(map => map.PublishYear)
                .ThenBy(map => map.Dpi)
                .ToList();
        }

        private static MapDto MapMapDto(Map map, IPathProvider pathProvider, IDomainProvider domainProvider)
        {
            return new MapDto
            {
                Id = map.Id,
                PublishYear = map.PublishYear,
                Dpi = map.Dpi,
                CollectionName = map.CollectionName,
                ImageUrl = GetImageUrl(map, pathProvider, domainProvider),
                Thumbnail = @$"{domainProvider.GetDomainName()}/{pathProvider.GetThumbnailsFolder()}/" + map.Thumbnail,
            };
        }

        private static string? GetImageUrl(Map map, IPathProvider pathProvider, IDomainProvider domainProvider)
        {
            return map.LocalImage is null ? map.ImageUrl : @$"{domainProvider.GetDomainName()}/{pathProvider.GetMapsImagesFolder()}/" + map.LocalImage;
        }

        public static List<OtherSourcesDto> MapOtherSources(List<OtherSource> otherSources)
        {
            return otherSources
                .Select(os => OtherSourcesDto.Mapper.Map(os))
                .OrderBy(os => os.Name)
                .ThenBy(os => os.Year)
                .ToList();
        }
    }
}

