using MapsVisualisation.Database;
using MapsVisualisation.Domain.Entities;
using MapsVisualisation.Domain.Exceptions;
using MapsVisualisation.Service.Services;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using MapsVisualisation.WebScrapers;

namespace MapsVisualisation.Service.Features.Scrapers.Shared;

public interface IScrapedInfoHandler
{
    Task Handle(List<RegionInfo> regionInfos);
}

public class ScrapedInfoHandler : IScrapedInfoHandler
{
    private readonly MapsVisualisationContext _context;
    private readonly IPathProvider _pathProvider;

    public ScrapedInfoHandler(MapsVisualisationContext contex, IPathProvider pathProvider)
    {
        _context = contex;
        _pathProvider = pathProvider;
    }

    public async Task Handle(List<RegionInfo> regionInfos)
    {
        foreach (var regionInfo in regionInfos)
        {
            var region = await _context.Regions
                .Include(r => r.Maps)
                .SingleOrDefaultAsync(r => r.RegionIdentity == regionInfo.RegionIdentity);

            var regionPresent = region is not null;

            if (region is null)
            {
                region = Region.Create(
                    regionInfo.RegionName1 ?? "",
                    regionInfo.RegionName2 ?? "",
                    regionInfo.RegionName3 ?? "",
                    regionInfo.RegionIdentity ?? "");

            }
            var changesWereMade = false;
            if (regionInfo.Maps is not null)
            {
                foreach (var mapInfo in regionInfo.Maps)
                {
                    if (!regionPresent)
                    {
                        var urlPresent = region.Maps.SingleOrDefault(m => m.ImageUrl == mapInfo.ImageUrl);

                        if (urlPresent is not null) continue;

                        string? thumbnail = null;

                        if (!File.Exists(GetFilePath(regionInfo, mapInfo)))
                        {
                            var thumbnailImage = await ThumbnailGenerator.GetThumbnailImage(mapInfo.ImageUrl);

                            thumbnail = await SaveImage(thumbnailImage, regionInfo, mapInfo);
                        }
                        else
                        {
                            thumbnail = GetFileName(regionInfo, mapInfo);
                        }

                        region.AddMap(mapInfo.PublishYear, mapInfo.Dpi, mapInfo.ImageUrl, mapInfo.CollectionName, thumbnail);
                        continue;
                    }

                    var existingMap = region.Maps.SingleOrDefault(m => m.ImageUrl == mapInfo.ImageUrl);

                    if (existingMap is null)
                    {
                        changesWereMade = true;

                        string? thumbnail = null;
                        var path = GetFilePath(regionInfo, mapInfo);

                        if (!File.Exists(path))
                        {
                            var thumbnailImage = await ThumbnailGenerator.GetThumbnailImage(mapInfo.ImageUrl);

                            thumbnail = await SaveImage(thumbnailImage, regionInfo, mapInfo);
                        }
                        else
                        {
                            thumbnail = GetFileName(regionInfo, mapInfo);
                        }

                        region.AddMap(mapInfo.PublishYear, mapInfo.Dpi, mapInfo.ImageUrl, mapInfo.CollectionName, thumbnail);
                        continue;
                    }
                }
            }

            if (!regionPresent)
            {
                Console.WriteLine($"Added new region {region.RegionIdentity}");
                _context.Regions.Add(region);
            }
            else if (changesWereMade)
            {
                Console.WriteLine($"Updated region {region.RegionIdentity}");
                _context.Regions.Update(region);
            }
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

    private string GetFilePath(RegionInfo regionInfo, MapInfo mapInfo)
    {
        var path = GetFileName(regionInfo, mapInfo);

        return Path.Combine(_pathProvider.GetThumbnailsPath(), path);
    }

    private async Task<string?> SaveImage(Image? image, RegionInfo regionInfo, MapInfo mapInfo)
    {
        if (image == null) return null;

        var path = GetFilePath(regionInfo, mapInfo);

        await image.SaveAsJpegAsync(path);

        return GetFileName(regionInfo, mapInfo);
    }
}
