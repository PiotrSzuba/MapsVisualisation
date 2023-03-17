using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace MapsVisualisation.WebScrapers.Helpers;

public static class ThumbnailGenerator
{
    public static async Task<Image?> GetThumbnailImage(string imageUrl)
    {
        var imageData = await DownloadImageFromUrl(imageUrl);

        return GenerateThumbnail(imageData);
    }

    public static Image? GetThumbnailImage(byte[] imageData)
    {
        return GenerateThumbnail(imageData);
    }

    private static Image? GenerateThumbnail(byte[] imageData)
    {
        try
        {
            var image = Image.Load(imageData);

            var scale = image.Height / 256;

            var width = image.Width / scale;
            var height = image.Height / scale;

            image.Mutate(x => x.Resize(width, height, KnownResamplers.Lanczos3));

            return image;
        }
        catch
        {
            return null;
        }
    }

    private static async Task<byte[]> DownloadImageFromUrl(string imageUrl)
    {
        try
        {
            using var client = new HttpClient();
            using var response = await client.GetAsync(imageUrl);
            return await response.Content.ReadAsByteArrayAsync();
        }
        catch
        {
            return Array.Empty<byte>();
        }
    }
}
