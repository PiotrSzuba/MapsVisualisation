using MapsVisualisation.Service.Features.Scrapers.Shared;
using MapsVisualisation.Service.BuildingBlocks;
using MapsVisualisation.WebScrapers.RegionScrapers;
using MapsVisualisation.WebScrapers.RegionScrapers.Mapster;
using MapsVisualisation.Domain.Entities;

namespace MapsVisualisation.Service.Features.Scrapers.Commands;

public class ScrapWig25Command : ICommand<List<RegionInfo>>
{
    internal class ScrapWig25CommandHandler : ICommandHandler<ScrapWig25Command, List<RegionInfo>>
    {
        private readonly IScrapedInfoHandler _scrapedInfoHandler;

        public ScrapWig25CommandHandler(IScrapedInfoHandler scrapedInfoHandler)
        {
            _scrapedInfoHandler = scrapedInfoHandler;
        }

        public async Task<List<RegionInfo>> Handle(ScrapWig25Command request, CancellationToken cancellationToken)
        {
            var regions = await MapsterRunner.GetWig25Async();

            await _scrapedInfoHandler.Handle(regions, RegionType.Wig25);

            return regions;
        }
    }
}