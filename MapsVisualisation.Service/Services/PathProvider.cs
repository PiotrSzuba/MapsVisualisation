namespace MapsVisualisation.Service.Services;

public interface IPathProvider
{
    string GetRoot();
    string GetThumbnailsPath();
    string GetMapsImagesPath();
}

public class PathProvider : IPathProvider
{
    public string Root { get; set; }
    public string ThumbnailsPath { get; set; }
    public string MapsImagesPath { get; set; }

    public PathProvider(string root)
    {
        Root = root;
        ThumbnailsPath = Path.Combine(root, "Thumbnails");
        MapsImagesPath = Path.Combine(root, "MapsImages");
    }

    public string GetRoot() => Root;
    public string GetThumbnailsPath() => ThumbnailsPath;
    public string GetMapsImagesPath() => MapsImagesPath;
}
