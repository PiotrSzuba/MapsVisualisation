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
    public const string ThumbnailsFolder = "Thumbnails";
    public const string MapsImagesFolder = "MapsImages";
    public string Root { get; set; }
    public string ThumbnailsPath { get; set; }
    public string MapsImagesPath { get; set; }

    public PathProvider(string root)
    {
        Root = root;
        ThumbnailsPath = Path.Combine(root, ThumbnailsFolder);
        MapsImagesPath = Path.Combine(root, MapsImagesFolder);
    }

    public string GetRoot() => Root;
    public string GetThumbnailsPath() => ThumbnailsPath;
    public string GetMapsImagesPath() => MapsImagesPath;
    public string GetThumbnailsFolder() => ThumbnailsFolder;
    public string GetMapsImagesFolder() => MapsImagesFolder;
}
