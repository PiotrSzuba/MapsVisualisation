using HtmlAgilityPack;
using System;

namespace MapsVisualisation.WebScrapers.RegionScrapers.Mapster;

internal static class MapsterScraper
{
    private const string BaseUrl = @"http://igrek.amzp.pl";

    public static List<RegionInfo> Run(string url)
    {
        var body = GetBody(url);
        var mapGrid = GetMapGrid(body);
        var gridCells = GetGridCells(mapGrid);

        var regions = gridCells
            .Select(cell => GetRegionInfo(cell))
            .ToList();

        return regions;
    }

    public static async Task<List<RegionInfo>> RunAsync(string url)
    {
        return await Task.Run(() => Run(url));
    }

    private static HtmlNode GetBody(string url)
    {
        var htmlDoc = new HtmlWeb().Load(url);

        if (htmlDoc is null)
            throw new Exception($"Could not load {url} !");

        var body = htmlDoc.DocumentNode.SelectSingleNode("/html[1]/body[1]");

        if (body is null)
            throw new Exception($"Could not load <body> of {url} !");

        return body;
    }

    private static HtmlNode GetMapGrid(HtmlNode body)
    {
        var content = body.ChildNodes.Single(c => c.Id == "content");
        var mapContent = content.ChildNodes.Single(c => c.GetClasses().Contains("mapcont"));

        if (mapContent is null)
            throw new Exception("Could not load map grid");

        return mapContent;
    }

    private static List<HtmlNode> GetGridCells(HtmlNode mapGrid)
    {
        var gridCells = mapGrid.ChildNodes.Where(c => c.GetClasses().Contains("mapind")).ToList();

        if (gridCells is null || gridCells.Count == 0)
            throw new Exception("Elements with class name mapind do not exists !");

        return gridCells;
    }

    private static RegionInfo GetRegionInfo(HtmlNode gridCell)
    {
        var hasLink = gridCell.ChildNodes.Any(c => c.Name == "a");

        return hasLink ? GetAllInfo(gridCell) : GetBasicInfo(gridCell);
    }

    private static RegionInfo GetAllInfo(HtmlNode gridCell)
    {
        var link = gridCell.ChildNodes.Single(c => c.Name == "a");

        var htmlUrl = BaseUrl + "/" + link.Attributes.Single(a => a.Name == "href").Value.Replace(";", "&");

        if (htmlUrl is null)
            return GetBasicInfo(gridCell);

        var mapInfos = MapsterTableScraper.GetMapsInfos(htmlUrl);

        if (mapInfos is null)
            return GetBasicInfo(gridCell);

        var info = link.ChildNodes.Where(c => c.Name != "br").ToList();
        var identity = GetIdentity(info[0].InnerText);
        var names = GetNames(info);

        Console.WriteLine($"{identity}, {names.FirstOrDefault()}");
        return new RegionInfo(identity, names, mapInfos);
    }

    private static RegionInfo GetBasicInfo(HtmlNode gridCell)
    {
        var info = gridCell.ChildNodes.Where(c => c.Name != "br").ToList();
        var identity = GetIdentity(info[0].InnerText);
        var names = GetNames(info);

        Console.WriteLine($"{identity}, {names.FirstOrDefault()}");
        return new RegionInfo(identity, names);
    }

    private static string GetIdentity(string rawIdentity)
    {
        var flawedData = rawIdentity.IndexOf('(');

        return flawedData == -1 ? rawIdentity : rawIdentity.Substring(0, flawedData);
    }

    private static List<string> GetNames(List<HtmlNode> info)
    {
        var names = new List<string>();
        for (int i = 1; i < info.Count; i++)
        {
            var name = info[i].InnerText;
            if (name.Length < 3 || name.Any(char.IsDigit)) continue;
            names.Add(info[i].InnerText);
        }

        return GetNamesAfterComa(GetNamesInParentheses(names));
    }

    private static List<string> GetNamesInParentheses(List<string> names)
    {
        if (names.Count == 3) return names;

        var filteredNames = names.Where(name => name.Length >= 3 && !name.Any(char.IsDigit)).ToList();

        var doubleNames = filteredNames.Where(n => n.Contains('(')).ToList();

        if (doubleNames.Count == 0) return filteredNames;

        for (int i = 0; i < doubleNames.Count; i++)
        {
            var doubleName = doubleNames[i];
            var startIdx = doubleName.IndexOf('(');
            var endIdx = doubleName.IndexOf(')');
            var newName = doubleName.Substring(startIdx + 1, endIdx - startIdx - 1);
            filteredNames.Add(newName.Replace(" ", ""));
            filteredNames[i] = doubleNames[i].Substring(0, startIdx).Replace(" ", "");
        }

        return filteredNames;
    }

    private static List<string> GetNamesAfterComa(List<string> names)
    {
        if (names.Count == 3) return names;

        var filteredNames = names.Where(name => name.Length >= 3 && !name.Any(char.IsDigit)).ToList();

        var doubleNames = filteredNames.Where(n => n.Contains(',')).ToList();

        if (doubleNames.Count == 0) return filteredNames;

        var newNames = new List<string>();

        foreach( var doubleName in doubleNames)
        {
            var orgNames = doubleName.Split(',').ToList();
            orgNames = orgNames.Select(n => n.Replace(" ", "")).ToList();
            newNames.AddRange(orgNames);
        }

        return newNames;
    }
}
