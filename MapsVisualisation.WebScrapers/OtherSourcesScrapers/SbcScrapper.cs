using HtmlAgilityPack;
using MapsVisualisation.Domain.Entities;
using System.Text.RegularExpressions;

namespace MapsVisualisation.WebScrapers.OtherSourcesScrapers;

public static class SbcScrapper
{
    private const string SourceName = "SBC";

    public static List<OtherSourceInfo> Run(Region region)
    {
        var otherSources = new List<OtherSourceInfo>();

        var htmlUrl = @$"https://www.sbc.org.pl/dlibra/results?q={region.RegionIdentity}+{region.RegionName1}&action=SimpleSearchAction&type=-6&p=0";
        var htmlDoc = new HtmlWeb().Load(htmlUrl);

        Console.WriteLine($"Started scraping: {region.RegionIdentity} {region.RegionName1}");

        if (htmlDoc is null)
            throw new Exception("Could not load the site !");

        var body = htmlDoc.DocumentNode.SelectSingleNode("/html[1]/body[1]");

        var containerMain = body.ChildNodes.SingleOrDefault(c => c.GetClasses().Contains("container--main"));

        if (containerMain is null)
            return new();

        var searchResult = containerMain.ChildNodes.SingleOrDefault(c => c.Name == "section" && c.Id == "");

        if (searchResult is null)
            return new();

        var constantContainer = searchResult.ChildNodes.SingleOrDefault(c => c.GetClasses().ToList().Count == 1 && c.GetClasses().Contains("constant-container"));
        
        if (constantContainer is null)
            return new();

        var objectBoxes = constantContainer.ChildNodes.SingleOrDefault(c => c.GetClasses().Contains("objectboxes"));

        if (objectBoxes is null)
            return new();

        var objectBoxesTiles = objectBoxes.ChildNodes.SingleOrDefault(c => c.GetClasses().Contains("objectboxes__tiles"));

        if (objectBoxesTiles is null)
            return new();

        var objectBoxList = objectBoxesTiles.ChildNodes.Where(c => c.GetClasses().Contains("objectbox")).ToList();

        if (objectBoxList.Count == 0)
            return new();

        foreach (var objectBox in objectBoxList)
        {
            var link = objectBox.ChildNodes.SingleOrDefault(c => c.Name == "a");

            if (link is null) continue;

            var url = link.Attributes.Single(a => a.Name == "href").Value.Replace("#structure", "");

            var content = objectBox.ChildNodes.Single(c => c.GetClasses().Contains("objectbox__content"));

            var titleContainer = content.ChildNodes.Single(c => c.GetClasses().Contains("objectbox__title"));
            var titleElement = titleContainer.ChildNodes.Single(c => c.Name == "h2");
            var title = titleElement.GetAttributes().Single(a => a.Name == "title").Value;

            var dateContainer = content.ChildNodes.Single(c => c.GetClasses().Contains("objectbox__date"));
            var dateElement = dateContainer.ChildNodes.Single(c => c.GetClasses().Contains("objectbox__text--date"));
            var date = dateElement.FirstChild.InnerText.Trim();
            var yearString = Regex.Match(date, @"\d+").Value;
            int? year = null;
            if (yearString is not null && yearString.Length == 4)
            {
                year = Convert.ToInt32(yearString);
            }

            //assumption here is that best matching results are always first so no need to check other results
            var isDataValid = title.Contains($"[{region.RegionIdentity}]");

            if (!isDataValid)
                break;

            var newOtherSource = new OtherSourceInfo(SourceName, url, year);

            Console.WriteLine($"Found new other source! {region.RegionName1} {region.RegionIdentity}: {SourceName} year {year} {url}");

            otherSources.Add(newOtherSource);
        }

        return otherSources;
    }
}
