using HtmlAgilityPack;
using System.Text;

namespace MapsVisualisation.WebScrapers.RegionScrapers;

public static class MapyAmzpScraper
{
    private const string BaseUrl = @"http://mapy.amzp.pl/tk25.cgi?11,61,50,103";

    public static List<RegionInfo> Run()
    {
        var regions = new List<RegionInfo>();

        var htmlDoc = new HtmlWeb().Load(BaseUrl);

        if (htmlDoc is null)
            throw new Exception("Could not load the site !");

        var table = htmlDoc.DocumentNode.SelectSingleNode("/html[1]/body[1]/table[1]");
        var rows = table.ChildNodes.Where(x => x.Name == "tr").ToList();

        foreach (var row in rows)
        {
            var cols = row.ChildNodes.Where(c => c.Name == "td").ToList();
            foreach (var col in cols)
            {
                var css = col.GetClasses().First();
                if (css == "a3") continue;
                if (css == "a2" || css == "a0")
                {
                    var info = col.ChildNodes.Where(c => c.Name == "i" || c.Name == "#text").ToList();
                    var number = info[0].InnerText.Replace("\n", "").Replace(" ", "");
                    var name = info[1].InnerText.Replace("\n", "").Replace(" ", "");
                    regions.Add(new RegionInfo(number, new() { name }));
                }
                if (css == "a1")
                {
                    var link = col.ChildNodes.SingleOrDefault(c => c.Name == "a");

                    if (link is null) continue;

                    var urlAttribute = link.Attributes.First(a => a.Name == "href");

                    var mapInfos = GetMapsInfo(urlAttribute);

                    var info = link.ChildNodes.Where(c => c.Name == "span").ToList();

                    var number = info[0].InnerText.Replace("\n", "").Replace(" ", "");
                    var gerName = info[1].InnerText.Replace("\n", "").Replace(" ", "");
                    var polName = info[2].InnerText.Replace("\n", "").Replace(" ", "");

                    regions.Add(new RegionInfo(number, new() { gerName, polName }, mapInfos));
                    //Console.WriteLine($"{number} : Ger: {gerName} Pol: {polName} MapLinksCount: {mapInfos.Count}");
                }
            }
        }

        return regions;
    }

    private static List<MapInfo> GetMapsInfo(HtmlAttribute? urlAttribute)
    {
        if (urlAttribute is null)
            throw new ArgumentNullException(nameof(urlAttribute));

        var htmlUrl = BuildUrl(urlAttribute);

        if (htmlUrl is null)
            throw new Exception($"Html attribute does not contain url {urlAttribute.XPath}");

        var htmlDoc = new HtmlWeb().Load(htmlUrl);

        if (htmlDoc is null)
            throw new Exception($"Server did not respond to {htmlUrl}");

        var table = htmlDoc.DocumentNode.SelectSingleNode("/html[1]/body[1]/table[1]");

        if (table is null)
            throw new Exception("Table is null");

        var rows = table.ChildNodes.Where(x => x.Name == "tr" && !x.GetClasses().Contains("head")).ToList();

        var mapLinks = new List<string>();

        var mapsInfos = new List<MapInfo>();

        foreach (var row in rows)
        {
            var cols = row.ChildNodes.Where(c => c.Name == "td").ToList();

            var imageLink = GetMapLink(cols[0]);

            if (imageLink is null)
                throw new Exception("Image link is empty");

            var year = cols[4].InnerText.Length != 4 ? 0 : Convert.ToInt32(cols[4].InnerText);
            var dpi = Convert.ToInt32(cols[5].InnerText);
            var collectionName = cols[6].InnerText;

            mapsInfos.Add(new MapInfo(year, dpi, imageLink, collectionName));
            mapLinks.Add(imageLink);
        }

        return mapsInfos;
    }

    private static string GetMapLink(HtmlNode? htmlNode)
    {
        if (htmlNode is null)
            throw new Exception("Node with images does not exists!");

        var links = htmlNode.ChildNodes.Where(c => c.Name == "a").ToList();

        if (links.Count == 0)
            throw new Exception("Href to image does not exists!");

        var mapUrl = links[0].Attributes.Single(a => a.Name == "href").Value;

        if (mapUrl is null)
            throw new Exception("Image does not exists");

        var fileExtension = mapUrl.Substring(mapUrl.Length - 3);

        if (fileExtension.ToLower() != "jpg" && fileExtension.ToLower() != "png" && fileExtension.ToLower() != "gif")
            throw new Exception("Image is not jpg, png or gif");

        return mapUrl;
    }

    private static string? BuildUrl(HtmlAttribute? url)
    {
        if (url is null) return null;

        var urlBuilder = new StringBuilder();
        urlBuilder.Append(url.Value);

        if (!url.Value.Contains(@"http://"))
        {
            urlBuilder.Insert(0, @"http://mapy.amzp.pl/");
        }

        return urlBuilder.ToString();
    }
}