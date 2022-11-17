namespace MapsVisualisation.Service.Features.Regions.Shared;

public class MapDto
{
    public Guid Id { get; set; }
    public int PublishYear { get; set; }
    public int Dpi { get; set; }
    public string? CollectionName { get; set; }
    public string? ImageUrl { get; set; }
    public string? Thumbnail { get; set; }
}
