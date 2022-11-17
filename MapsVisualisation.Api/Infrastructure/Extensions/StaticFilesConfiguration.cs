using Microsoft.Extensions.FileProviders;

namespace MapsVisualisation.Api.Infrastructure.Extensions;

public static class StaticFilesConfiguration
{
    public static void ConfigureStaticFiles(this IApplicationBuilder app, string rootPath)
    {
        app.ConfigureDirectory(rootPath, "thumbnails");
        app.ConfigureDirectory(rootPath, "MapsImages");
    }

    private static void ConfigureDirectory(this IApplicationBuilder app, string rootPath, string folderName)
    {
        var directory = Path.Combine(rootPath, folderName);

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var fileProvider = new PhysicalFileProvider(directory);
        var requestPath = $"/{folderName}";

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = fileProvider,
            RequestPath = requestPath
        });

        app.UseDirectoryBrowser(new DirectoryBrowserOptions
        {
            FileProvider = fileProvider,
            RequestPath = requestPath
        });
    }
}
