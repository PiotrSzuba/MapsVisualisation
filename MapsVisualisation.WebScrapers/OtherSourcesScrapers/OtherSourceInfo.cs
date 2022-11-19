namespace MapsVisualisation.WebScrapers.OtherSourcesScrapers;

public class OtherSourceInfo
{
    public string Name { get; set; }
    public string Url { get; set; }
    public int? Year { get; set; }

    public OtherSourceInfo(string name, string url, int? year)
    {
        Name = name;
        Url = url;
        Year = year;
    }
}
