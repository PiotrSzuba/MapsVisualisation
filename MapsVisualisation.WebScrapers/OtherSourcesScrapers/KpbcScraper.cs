using HtmlAgilityPack;
using MapsVisualisation.Domain.Entities;

namespace MapsVisualisation.WebScrapers.OtherSourcesScrapers;

public static class KpbcScraper
{
    private const string SourceName = "KPBC";

    public static List<OtherSourceInfo> Run(Region region)
    {
        var otherSources = new List<OtherSourceInfo>();

        var regionIdentity = GetRegionIdentity(region);

        var key = region.RegionType == RegionType.IgrekAmzp ?
            $"{regionIdentity}" : 
            $"[Neue Nr {regionIdentity}]";

        var htmlUrl = @$"https://kpbc.umk.pl/dlibra/results?q={GetSearchQuery(region, regionIdentity)}&action=SimpleSearchAction&type=-6&p=0";
        var htmlDoc = new HtmlWeb().Load(htmlUrl);

        Console.WriteLine($"Started scraping {SourceName} for {region.RegionIdentity} {region.RegionName1}");

        if (htmlDoc is null)
            throw new Exception("Could not load the site !");

        var objectBoxList = htmlDoc.GetDlibraResultBoxes();

        if (objectBoxList.Count == 0)
            return new();

        foreach (var objectBox in objectBoxList)
        {
            var newSource = objectBox.GetInfoFromResultBox(key, SourceName);

            if (newSource is null)
                break;

            otherSources.Add(newSource);
            Console.WriteLine($"Found new other source! {region.RegionName1} {region.RegionIdentity}: {newSource.Name} year {newSource.Year} {newSource.Url}");
        }

        return otherSources;
    }

    private static string GetRegionIdentity(Region region)
    {
        if (region.RegionType == RegionType.MapyAmzp)
        {
            return region.RegionIdentity;
        }

        var pLetterIndex = region.RegionIdentity.ToLower().IndexOf("p");
        var sLetterIndex = region.RegionIdentity.ToLower().IndexOf("s");

        var pas = region.RegionIdentity.Substring(pLetterIndex + 1, 2);
        var slup = region.RegionIdentity.Substring(sLetterIndex + 1, 2);

        return $"Pas {pas} Słup {slup}";
    }

    private static string GetSearchQuery(Region region, string regionIdentity)
    {
        if (region.RegionType == RegionType.MapyAmzp)
        {
            return $"{region.RegionIdentity}+{region.RegionName1}";
        }

        return regionIdentity.Replace(" ", "+");
    }
}
