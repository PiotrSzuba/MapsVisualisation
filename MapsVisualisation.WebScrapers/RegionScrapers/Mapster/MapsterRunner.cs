namespace MapsVisualisation.WebScrapers.RegionScrapers.Mapster;

public static class MapsterRunner
{
    private const string Wig100 = @"http://igrek.amzp.pl/mapindex.php?cat=WIG100";
    private const string Wig25 = @"http://igrek.amzp.pl/mapindex.php?cat=WIG100";
    private const string MesstischblattOst = @"http://igrek.amzp.pl/mapindex.php?cat=TK25";
    private const string MesstischblattGer = @"http://igrek.amzp.pl/mapindex.php?cat=TK25GER";
    private const string AusHun75 = @"http://igrek.amzp.pl/mapindex.php?cat=KUK075";

    public static List<RegionInfo> GetWig100() =>
        MapsterScraper.Run(Wig100);

    public static async Task<List<RegionInfo>> GetWig100Async() =>
        await MapsterScraper.RunAsync(Wig100);

    public static List<RegionInfo> GetWig25() =>
        MapsterScraper.Run(Wig25);

    public static async Task<List<RegionInfo>> GetWig25Async() =>
        await MapsterScraper.RunAsync(Wig25);

    public static List<RegionInfo> GetAusHunMonarch75() =>
        MapsterScraper.Run(AusHun75);

    public static async Task<List<RegionInfo>> GetAusHunMonarch75Async() =>
        await MapsterScraper.RunAsync(AusHun75);

    public static List<RegionInfo> GetMesstischblatt()
    {
        var regions = new List<RegionInfo>();

        var ost = MapsterScraper.Run(MesstischblattOst);
        var ger = MapsterScraper.Run(MesstischblattGer);

        regions.AddRange(ost);
        regions.AddRange(ger);

        return regions;
    }

    public static async Task<List<RegionInfo>> GetMesstischblattAsync()
    {
        var regions = new List<RegionInfo>();

        var ostTask = MapsterScraper.RunAsync(MesstischblattOst);
        var gerTask = MapsterScraper.RunAsync(MesstischblattGer);

        var ost = await ostTask;
        var ger = await gerTask;

        regions.AddRange(ost);
        regions.AddRange(ger);

        return regions;
    }
}
