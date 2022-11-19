namespace MapsVisualisation.Domain.Entities;

public class OtherSource
{
    public Guid Id { get; private set; }
    public string? Name { get; private set; }
    public string? Url { get; private set; }
    public int? Year { get; private set; }
    public Region? Region { get; private set; }

    public OtherSource() { }

    private OtherSource(string name, string url, int? year)
    {
        Name = name;
        Url = url;
        Year = year;
    }

    public static OtherSource Create(Region region, string name, string url, int? year)
    {
        return new OtherSource(name, url, year)
        {
            Region = region,
        };
    }
}
