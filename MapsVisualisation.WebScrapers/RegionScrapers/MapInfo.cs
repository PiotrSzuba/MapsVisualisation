namespace MapsVisualisation.WebScrapers.RegionScrapers;

public class MapInfo
{
    public int PublishYear { get; set; }
    public int Dpi { get; set; }
    public string? CollectionName { get; set; }
    public string ImageUrl { get; set; }

    public MapInfo(int year, int dpi, string imageUrl, string? collectionName = null)
    {
        PublishYear = year;
        Dpi = dpi;
        ImageUrl = imageUrl;
        CollectionName = collectionName;
    }
}
