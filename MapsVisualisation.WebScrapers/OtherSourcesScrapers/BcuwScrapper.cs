using HtmlAgilityPack;
using MapsVisualisation.Domain.Entities;
using System.Text.RegularExpressions;

namespace MapsVisualisation.WebScrapers.OtherSourcesScrapers;

public static class BcuwScrapper
{
    private const string SourceName = "BCUWr";

    public static List<OtherSourceInfo> Run(Region region)
    {
        var otherSources = new List<OtherSourceInfo>();
        var htmlUrl = @$"https://www.bibliotekacyfrowa.pl/dlibra/results?q={region.RegionIdentity}+{region.RegionName1}&action=SimpleSearchAction&type=-6&p=0";
        var htmlDoc = new HtmlWeb().Load(htmlUrl);

        Console.WriteLine($"Started scraping {SourceName} for {region.RegionIdentity} {region.RegionName1}");

        if (htmlDoc is null)
            throw new Exception("Could not load the site !");

        var objectBoxList = htmlDoc.GetDlibraResultBoxes();

        if (objectBoxList.Count == 0)
            return new();

        foreach (var objectBox in objectBoxList)
        {
            var newSource = objectBox.GetInfoFromResultBox($"[Neue Nr {region.RegionIdentity}]", SourceName);

            if (newSource is null)
                break;

            otherSources.Add(newSource);
            Console.WriteLine($"Found new other source! {region.RegionName1} {region.RegionIdentity}: {newSource.Name} year {newSource.Year} {newSource.Url}");
        }

        return otherSources;
    }
}
