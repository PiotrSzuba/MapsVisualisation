namespace MapsVisualisation.Domain.Entities;

public class Map
{
    public Guid Id { get; private set; }
    public int PublishYear { get; private set; }
    public int Dpi { get; private set; }
    public string? CollectionName { get; private set; }
    public string? ImageUrl { get; private set; }
    public string? Thumbnail { get; private set; }
    public string? LocalImage { get; private set; }
    public Region? Region { get; private set; }

    public Map() { } //ef

    private Map(int year, 
        int dpi, 
        string imageUrl, 
        string? collectionName = null, 
        string? thumbnail = null,
        string? localImage = null)
    {
        PublishYear = year;
        Dpi = dpi;
        ImageUrl = imageUrl;
        CollectionName = collectionName;
        Thumbnail = thumbnail;
        LocalImage = localImage;
    }

    public static Map Create(
        Region region, 
        int year, 
        int dpi, 
        string imageUrl, 
        string? collectionName = null, 
        string? thumbnail = null,
        string? localImage = null
        )
    {
        return new Map(year, dpi, imageUrl, collectionName, thumbnail, localImage)
        {
            Region = region,
        };
    }

    public void UpdateLocalImage(string imageName)
    {
        LocalImage = imageName;
    }
}