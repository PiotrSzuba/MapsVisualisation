using MapsVisualisation.Database;
using MapsVisualisation.Domain.Entities;
using MapsVisualisation.Domain.Exceptions;
using MapsVisualisation.Service.Services;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using MapsVisualisation.WebScrapers.RegionScrapers;
using MapsVisualisation.WebScrapers.Helpers;
using RegionInfo = MapsVisualisation.WebScrapers.RegionScrapers.RegionInfo;

namespace MapsVisualisation.Service.Features.Scrapers.Shared;

public interface IScrapedInfoHandler
{
    Task Handle(List<RegionInfo> regionInfos, RegionType regionType);
}

public class ScrapedInfoHandler : IScrapedInfoHandler
{
    private readonly MapsVisualisationContext _context;
    private readonly IPathProvider _pathProvider;
    private bool ChangesWereMade = false;
    private bool RegionPresent = false;

    public ScrapedInfoHandler(MapsVisualisationContext contex, IPathProvider pathProvider)
    {
        _context = contex;
        _pathProvider = pathProvider;
    }

    public async Task Handle(List<RegionInfo> regionInfos, RegionType regionType)
    {
        foreach (var regionInfo in regionInfos)
        {
            var region = await _context.Regions
                .Include(r => r.Maps)
                .SingleOrDefaultAsync(r => r.RegionIdentity == regionInfo.RegionIdentity);

            RegionPresent = region is not null;
            ChangesWereMade = false;

            region ??= Region.Create(
                    regionInfo.RegionName1 ?? "",
                    regionInfo.RegionName2 ?? "",
                    regionInfo.RegionName3 ?? "",
                    regionInfo.RegionIdentity ?? "",
                    regionType);

            if (regionInfo.Maps is not null)
            {
                foreach (var mapInfo in regionInfo.Maps)
                {
                    await HandleMapInfo(region, regionInfo, mapInfo);
                }
            }

            if (!RegionPresent)
            {
                Console.WriteLine($"Added new region {region.RegionIdentity}");
                _context.Regions.Add(region);
            }
            else if (ChangesWereMade)
            {
                Console.WriteLine($"Updated region {region.RegionIdentity}");
                _context.Regions.Update(region);
            }
        }
    }

    private async Task HandleMapInfo(Region region, RegionInfo regionInfo, MapInfo mapInfo)
    {
        var existingMap = region.Maps
            .SingleOrDefault(m => m.Dpi == mapInfo.Dpi && m.PublishYear == mapInfo.PublishYear);

        if (existingMap is not null) return;

        var fileName = GetFileName(regionInfo, mapInfo);
        var imageData = await DownloadImageAsync(mapInfo);

        if (imageData.Length == 0) return;

        var localImage = await SaveImage(imageData, fileName);
        var thumbnail = await SaveThumbnail(imageData, fileName);

        region.AddMap(mapInfo.PublishYear, mapInfo.Dpi, mapInfo.ImageUrl, mapInfo.CollectionName, thumbnail, localImage);

        if (RegionPresent)
        {
            ChangesWereMade = true;
        }
    }

    private string GetFileName(RegionInfo regionInfo, MapInfo mapInfo)
    {
        var path = $"{regionInfo.RegionIdentity}-{regionInfo.RegionName1}-{mapInfo.PublishYear}-{mapInfo.Dpi}-{mapInfo.CollectionName}";

        path = path.Replace(".", "").Replace(@"\", "").Replace("\t", "").Replace(@"/", "");
        path += ".jpg";

        if (path is null)
            throw new EntityInvalidStateException("Path is null");

        return path;
    }

    private string GetThumbnailFilePath(string thumbnailName)
    {
        return Path.Combine(_pathProvider.GetThumbnailsPath(), thumbnailName);
    }

    private string GetImageFilePath(string imageName)
    {
        return Path.Combine(_pathProvider.GetMapsImagesPath(), imageName);
    }

    private async Task<string?> SaveThumbnail(byte[] imageData, string thumbnailName)
    {
        var thumbnailPath = GetThumbnailFilePath(thumbnailName);

        if (File.Exists(thumbnailPath))
        {
            return thumbnailName;
        }

        var thumbnailImage = ThumbnailGenerator.GetThumbnailImage(imageData);

        if (thumbnailImage == null) return null;

        await thumbnailImage.SaveAsJpegAsync(thumbnailPath);

        return thumbnailName;
    }

    private async Task<string> SaveImage(byte[] imageData, string imageName)
    {
        var imagePath = GetImageFilePath(imageName);

        if (File.Exists(imagePath))
        {
            return imageName;
        }

        await File.WriteAllBytesAsync(imagePath, imageData);

        return imageName;
    }


    private async Task<byte[]> DownloadImageAsync(MapInfo mapInfo)
    {
        try
        {
            using var client = new HttpClient();
            using var response = await client.GetAsync(mapInfo.ImageUrl);
            return await response.Content.ReadAsByteArrayAsync();
        }
        catch
        {
            return Array.Empty<byte>();
        }
    }
}
