using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace MapsVisualisation.WebScrapers;

public static class ThumbnailGenerator
{
    public static async Task<Image?> GetThumbnailImage(string imageUrl)
    {
        return await DownloadImageFromUrl(imageUrl);
    }

    private static async Task<Image?> DownloadImageFromUrl(string imageUrl)
    {
        using var client = new HttpClient();
        using var response = await client.GetAsync(imageUrl);
        using var stream = await response.Content.ReadAsStreamAsync();
        byte[] bytes;
        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        bytes = memoryStream.ToArray();
        try
        {
            var image = Image.Load(bytes);

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
}
