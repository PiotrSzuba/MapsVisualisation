using HtmlAgilityPack;
using MapsVisualisation.Domain.Entities;
using System.Text.RegularExpressions;
namespace MapsVisualisation.WebScrapers.RegionScrapers.Mapster;

public static class MapsterTableScraper
{
    public static List<MapInfo?> GetMapsInfos(string url)
    {
        var htmlDoc = new HtmlWeb().Load(url);

        if (htmlDoc is null)
            throw new Exception($"Could not load {url} !");

        var body = htmlDoc.DocumentNode.SelectSingleNode("/html[1]/body[1]");

        if (body is null)
            throw new Exception($"Could not load <body> of {url} !");

        var dmaps = body.ChildNodes.SingleOrDefault(c => c.GetClasses().Contains("dmaps"));

        if (dmaps is null)
            throw new Exception("Could not find element with class dmaps");

        var rows = dmaps.ChildNodes.Single(c => c.Name == "table").ChildNodes.Where(c => c.Name == "tr").ToList();

        if (rows is null || rows.Count == 0)
            return new List<MapInfo?>();

        var maps = rows
            .Select(row => GetMapInfo(row))
            .Where(info => info != null)
            .ToList();

        return maps is null ? new List<MapInfo?>() : maps;
    }

    private static MapInfo? GetMapInfo(HtmlNode row)
    {
        var cols = row.ChildNodes.Where(c => c.Name == "td").ToList();

        if (cols is null || cols[0].FirstChild is null || cols[0].FirstChild.Name != "a")
            return null;

        var link = cols[0].FirstChild.Attributes.Single(a => a.Name == "href").Value;

        return new MapInfo(FindYear(cols), FindDpi(cols), link);
    }

    private static int FindYear(List<HtmlNode> columns)
    {
        var regex = new Regex(@"\d{4}");
        var cols = columns
            .SelectMany(c => regex.Matches(c.InnerText).Select(x => x.Value))
            .ToList();

        if (cols.Count == 0) return 0;

        var yearValue = cols.First(s => s.StartsWith('1'));

        return Convert.ToInt32(yearValue);
    }

    private static int FindDpi(List<HtmlNode> columns)
    {
        var columnWithDpi = columns.SingleOrDefault(c => c.InnerText.ToLower().Contains("dpi"));

        if (columnWithDpi is null) return 0;

        var dpiValue = Regex.Matches(columnWithDpi.InnerText, @"\d+").Single();

        if (dpiValue is null)
            throw new Exception("Could not extract dpi value!");

        return Convert.ToInt32(dpiValue.Value);
    }
}
