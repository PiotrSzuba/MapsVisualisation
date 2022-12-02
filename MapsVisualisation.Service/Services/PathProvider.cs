using Microsoft.Extensions.Configuration;

namespace MapsVisualisation.Service.Services;

public interface IPathProvider
{
    string GetRoot();
    string GetThumbnailsPath();
    string GetMapsImagesPath();
    string GetThumbnailsFolder();
    string GetMapsImagesFolder();
}

public class PathProvider : IPathProvider
{
    private static string ThumbnailsFolder { get; set; } = string.Empty;
    private static string MapsImagesFolder { get; set; } = string.Empty;
    private string Root { get; set; }
    private string ThumbnailsPath { get; set; }
    private string MapsImagesPath { get; set; }

    public PathProvider(string root, IConfiguration configuration)
    {
        Root = root;
        ThumbnailsFolder = configuration.GetSection("StaticFile").GetSection("ThumbnailsFolder").Value;
        MapsImagesFolder = configuration.GetSection("StaticFile").GetSection("MapImagesFolder").Value;
        ThumbnailsPath = Path.Combine(root, ThumbnailsFolder);
        MapsImagesPath = Path.Combine(root, MapsImagesFolder);
    }

    public string GetRoot() => Root;
    public string GetThumbnailsPath() => ThumbnailsPath;
    public string GetMapsImagesPath() => MapsImagesPath;
    public string GetThumbnailsFolder() => ThumbnailsFolder;
    public string GetMapsImagesFolder() => MapsImagesFolder;
}
