namespace MapsVisualisation.WebScrapers.RegionScrapers;

public class RegionInfo
{
    public string? RegionIdentity { get; set; }
    public string? RegionName1 { get; set; }
    public string? RegionName2 { get; set; }
    public string? RegionName3 { get; set; }
    public List<MapInfo>? Maps { get; set; }

    public RegionInfo(string identity, List<string> names, List<MapInfo>? maps = null)
    {
        RegionIdentity = identity;

        switch (names.Count)
        {
            case 1:
                RegionName1 = names[0];
                break;
            case 2:
                RegionName1 = names[0];
                RegionName2 = names[1];
                break;
            case 3:
                RegionName1 = names[0];
                RegionName2 = names[1];
                RegionName3 = names[2];
                break;
        }

        Maps = maps;
    }
}

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
