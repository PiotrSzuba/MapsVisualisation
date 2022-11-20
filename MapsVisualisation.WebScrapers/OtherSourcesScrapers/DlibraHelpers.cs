using HtmlAgilityPack;
using MapsVisualisation.Domain.Entities;
using System.Text.RegularExpressions;

namespace MapsVisualisation.WebScrapers.OtherSourcesScrapers;

public static class DlibraHelpers
{
    public static List<HtmlNode> GetDlibraResultBoxes(this HtmlDocument htmlDocument)
    {
        var body = htmlDocument.DocumentNode.SelectSingleNode("/html[1]/body[1]");

        var containerMain = body.ChildNodes.SingleOrDefault(c => c.GetClasses().Contains("container--main"));

        if (containerMain is null)
        {
            var main = body.ChildNodes.SingleOrDefault(c => c.Name == "main");
            if (main is null)
            {
                return new();
            }

            containerMain = main.ChildNodes.SingleOrDefault(c => c.GetClasses().Contains("container--main"));
            if (containerMain is null)
            {
                return new();
            }
        }

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

        return objectBoxList;
    }

    public static OtherSourceInfo? GetInfoFromResultBox(this HtmlNode resultBox, string key, string sourceName)
    {
        var boxPhoto = resultBox.ChildNodes.Single(c => c.GetClasses().Contains("objectbox__photo"));
        var links = boxPhoto.ChildNodes.Where(c => c.Name == "a" && c.GetAttributes().Where(a => a.Name == "aria-label").Any()).ToList();

        if (links is null || links.Count == 0)
            return null;

        var link = links.SingleOrDefault(l => l.GetAttributes().Single(a => a.Name == "aria-label").Value.Contains($"{key.Replace("ł", "&#322;")}"));

        if (link is null)
            return null;

        var val = link.GetAttributes().Single(a => a.Name == "aria-label").Value;

        var isDataValid = val.Contains($"{key.Replace("ł", "&#322;")}");

        if (!isDataValid)
            return null;

        var url = link.Attributes.Single(a => a.Name == "href").Value.Replace("#structure", "");

        var content = resultBox.ChildNodes.Single(c => c.GetClasses().Contains("objectbox__content"));

        var dateContainer = content.ChildNodes.Single(c => c.GetClasses().Contains("objectbox__date"));
        var dateElement = dateContainer.ChildNodes.Single(c => c.GetClasses().Contains("objectbox__text--date"));
        var date = dateElement.FirstChild.InnerText.Trim();
        var yearString = Regex.Match(date, @"\d+").Value;
        int? year = null;
        if (yearString is not null && yearString.Length == 4)
        {
            year = Convert.ToInt32(yearString);
        }

        var newOtherSource = new OtherSourceInfo(sourceName, url, year);

        return newOtherSource;
    }
}
