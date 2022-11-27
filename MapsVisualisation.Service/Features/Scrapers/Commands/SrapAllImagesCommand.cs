using MapsVisualisation.Service.BuildingBlocks;
using MapsVisualisation.Database;
using MapsVisualisation.Domain.Entities;
using MapsVisualisation.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using MapsVisualisation.Service.Services;

namespace MapsVisualisation.Service.Features.Scrapers.Commands;

public class SrapAllImagesCommand : ICommand<List<string>>
{
    internal class ScrapAllImagesCommandHandler : ICommandHandler<SrapAllImagesCommand, List<string>>
    {
        private readonly MapsVisualisationContext _context;
        private readonly IPathProvider _pathProvider;

        public ScrapAllImagesCommandHandler(MapsVisualisationContext context, IPathProvider pathProvider)
        {
            _context = context;
            _pathProvider = pathProvider;
        }

        public async Task<List<string>> Handle(SrapAllImagesCommand request, CancellationToken cancellationToken)
        {
            var maps = await _context.Maps
                .Include(map => map.Region)
                .Where(map => map.LocalImage == null && map.ImageUrl != null && map.Region != null)
                .ToListAsync(cancellationToken: cancellationToken);

            var fileNames = new List<string>();

            foreach(Map map in maps)
            {
                var fileName = GetFileName(map);
                var path = Path.Combine(_pathProvider.GetMapsImagesPath(), fileName);

                if (File.Exists(path) && map.LocalImage is null)
                {
                    map.UpdateLocalImage(fileName);
                    
                    _context.Maps.Update(map);
                    continue;
                }

                var stopWatch = new Stopwatch();
                stopWatch.Start();
                var imageData = await DownloadImageAsync(map, cancellationToken);
                stopWatch.Stop();

                if (imageData.Length == 0)
                {
                    Console.WriteLine($"Empty data for {fileName}");
                    continue;
                }

                await File.WriteAllBytesAsync(path, imageData, cancellationToken);

                Console.WriteLine($"Saved new file {fileName} took: {stopWatch.ElapsedMilliseconds} ms");

                fileNames.Add(fileName);
            }

            return fileNames;
        }

        private async Task<byte[]> DownloadImageAsync(Map map, CancellationToken cancellationToken)
        {
            if (map.ImageUrl is null || map.ImageUrl.Length == 0) return Array.Empty<byte>();

            try
            {
                using var client = new HttpClient();
                using var response = await client.GetAsync(map.ImageUrl, cancellationToken);
                using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                using var memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);

                return memoryStream.ToArray();
            }
            catch
            {
                return Array.Empty<byte>();
            }
        }

        private string GetFileName(Map map)
        {
            if (map.Region is null)
                throw new EntityInvalidStateException("Region is null");

            var path = $"{map.Region.RegionIdentity}-{map.Region.RegionName1}-{map.PublishYear}-{map.Dpi}-{map.CollectionName}";

            path = path.Replace(".", "").Replace(@"\", "").Replace("\t", "").Replace(@"/", "");
            path += ".jpg";

            if (path is null)
                throw new EntityInvalidStateException("Path is null");

            return path;
        }
    }
}
