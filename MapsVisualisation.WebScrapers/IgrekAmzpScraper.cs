using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace MapsVisualisation.WebScrapers;

public static class IgrekAmzpScraper
{
    private const string BaseUrl = @"http://igrek.amzp.pl";

    public static List<RegionInfo> Run()
    {
        var regions = new List<RegionInfo>();

        var htmlUrl = BaseUrl + @"/mapindex.php";
        var htmlDoc = new HtmlWeb().Load(htmlUrl);

        if (htmlDoc is null)
            throw new Exception("Could not load the site !");

        var body = htmlDoc.DocumentNode.SelectSingleNode("/html[1]/body[1]");
        var content = body.ChildNodes.SingleOrDefault(c => c.Id == "content");

        if (content is null)
            throw new Exception("Content does not exists !");

        var mapContent = content.ChildNodes.SingleOrDefault(c => c.GetClasses().Contains("mapcont"));

        if (mapContent is null)
            throw new Exception("Map content does not exists !");

        var mapsInfos = mapContent.ChildNodes.Where(c => c.GetClasses().Contains("mapind")).ToList();

        if (mapsInfos is null || mapsInfos.Count == 0)
            throw new Exception("Elements with class name mapind do not exists !");

        foreach (var mapInfo in mapsInfos)
        {
            var hasLink = mapInfo.ChildNodes.Any(c => c.Name == "a");
            if (hasLink)
            {
                var link = mapInfo.ChildNodes.SingleOrDefault(c => c.Name == "a");

                if (link is null)
                    throw new Exception("How link is not a link?");

                var mapInfos = GetMapsInfos(link);

                var linkInfo = link.ChildNodes.Where(c => c.Name != "br").ToList();

                var identity = linkInfo[0].InnerText;
                var names = new List<string>();
                for (int i = 1; i < linkInfo.Count; i++)
                {
                    names.Add(linkInfo[i].InnerText);
                }

                names = GetNamesInParentheses(names);

                regions.Add(new RegionInfo(identity, names, mapInfos));

                //Console.WriteLine($"{identity} {names.Count} names: {names.First()} maps: {mapInfos.Count}");
            }
            else
            {
                var info = mapInfo.ChildNodes.Where(c => c.Name != "br").ToList();
                var identity = info[0].InnerText;

                var names = new List<string>();
                for (int i = 1; i < info.Count; i++)
                {
                    names.Add(info[i].InnerText);
                }

                names = GetNamesInParentheses(names);

                regions.Add(new RegionInfo(identity, names));

                //Console.WriteLine($"{identity} {names.Count} names: {names.FirstOrDefault()} No image !");
            }
        }

        return regions;
    }

    private static List<MapInfo> GetMapsInfos(HtmlNode linkNode)
    {
        var htmlUrl = BaseUrl + "/" + linkNode.Attributes.Single(a => a.Name == "href").Value.Replace(";", "&");

        var htmlDoc = new HtmlWeb().Load(htmlUrl);

        var body = htmlDoc.DocumentNode.SelectSingleNode("/html[1]/body[1]");
        var dmaps = body.ChildNodes.SingleOrDefault(c => c.GetClasses().Contains("dmaps"));

        if (dmaps is null)
            throw new Exception("Could not find element with class dmaps");

        var rows = dmaps.ChildNodes.Single(c => c.Name == "table").ChildNodes.Where(c => c.Name == "tr").ToList();

        var maps = new List<MapInfo>();

        foreach (var row in rows)
        {
            var cols = row.ChildNodes.Where(c => c.Name == "td").ToList();

            if (cols is null)
                throw new Exception("Row present but colums are gone");

            if (cols[0].FirstChild is null || cols[0].FirstChild.Name != "a") continue;

            var link = cols[0].FirstChild.Attributes.Single(a => a.Name == "href").Value;

            maps.Add(new MapInfo(FindYear(cols), FindDpi(cols), link));
        }

        return maps;
    }

    private static int FindYear(List<HtmlNode> columns)
    {
        var regex = new Regex(@"\d{4}");
        var cols = columns
            .SelectMany(c => regex.Matches(c.InnerText).Select(x => x.Value))
            .ToList();

        if (cols.Count == 0) return 0;

        var yearValue = cols.Single(s => s.StartsWith('1'));

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

    private static List<string> GetNamesInParentheses(List<string> names)
    {
        if (names.Count == 3) return names;

        var doubleNames = names.Where(n => n.Contains('(')).ToList();

        if (doubleNames.Count == 0) return names;

        for (int i = 0; i < doubleNames.Count; i++)
        {
            var doubleName = doubleNames[i];
            var startIdx = doubleName.IndexOf('(');
            var endIdx = doubleName.IndexOf(')');
            var newName = doubleName.Substring(startIdx + 1, endIdx - startIdx - 1);
            names.Add(newName.Replace(" ", ""));
            names[i] = doubleNames[i].Substring(0, startIdx).Replace(" ", "");
        }

        return names;
    }
}
