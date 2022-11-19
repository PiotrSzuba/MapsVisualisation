using HtmlAgilityPack;
using System.Text;
using MapsVisualisation.Database;

namespace MapsVisualisation.WebScrapers.OtherSourcesScrapers;

public static class SbcScrapper
{
    private const string BaseUrl = @"https://www.sbc.org.pl/dlibra/results?q=4868+Breslau&action=SimpleSearchAction&type=-6&p=0";
    public static List<string> Run()
    {
        var links = new List<string>();

        var htmlUrl = @"https://www.sbc.org.pl/dlibra/results?q=4868+Breslau&action=SimpleSearchAction&type=-6&p=0";
        var htmlDoc = new HtmlWeb().Load(htmlUrl);

        if (htmlDoc is null)
            throw new Exception("Could not load the site !");

        var body = htmlDoc.DocumentNode.SelectSingleNode("/html[1]/body[1]");

        var containerMain = body.ChildNodes.Single(c => c.GetClasses().Contains("container--main"));

        var searchResult = containerMain.ChildNodes.Single(c => c.Name == "section" && c.Id == "");

        var constantContainer = searchResult.ChildNodes.Single(c => c.GetClasses().ToList().Count == 1 && c.GetClasses().Contains("constant-container"));

        var objectBoxes = constantContainer.ChildNodes.Single(c => c.GetClasses().Contains("objectboxes"));

        var objectBoxesTiles = objectBoxes.ChildNodes.Single(c => c.GetClasses().Contains("objectboxes__tiles"));

        var objectBoxList = objectBoxesTiles.ChildNodes.Where(c => c.GetClasses().Contains("objectbox")).ToList();

        if (objectBoxList.Count == 0)
            return new();

        foreach (var objectBox in objectBoxList)
        {
            var link = objectBox.ChildNodes.Single(c => c.Name == "a");

            var url = link.Attributes.Single(a => a.Name == "href").Value.Replace("#structure", "");

            var content = objectBox.ChildNodes.Single(c => c.GetClasses().Contains("objectbox__content"));

            var title = content.ChildNodes.Single(c => c.GetClasses().Contains("objectbox__title"));

            var titleElement = title.ChildNodes.Single(c => c.Name == "h2");

            var titleText = titleElement.GetAttributes().Single(a => a.Name == "title").Value;

            //assumption here is that best matching results are always first so no need to check other results
            var isDataValid = titleText.Contains($"[{4868}]");

            if (!isDataValid)
                break;

            links.Add(url);
        }

        return links;
    }
}
