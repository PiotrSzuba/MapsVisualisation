using MapsVisualisation.Domain.Entities;
using MapsVisualisation.Service.BuildingBlocks;
using MapsVisualisation.Service.Features.Scrapers.Shared;
using MapsVisualisation.WebScrapers.RegionScrapers;
using MapsVisualisation.WebScrapers.RegionScrapers.Mapster;

namespace MapsVisualisation.Service.Features.Scrapers.Commands;

public class ScrapWig100Command : ICommand<List<RegionInfo>>
{
    internal class ScrapWig100CommandHandler : ICommandHandler<ScrapWig100Command, List<RegionInfo>>
    {
        private readonly IScrapedInfoHandler _scrapedInfoHandler;

        public ScrapWig100CommandHandler(IScrapedInfoHandler scrapedInfoHandler)
        {
            _scrapedInfoHandler = scrapedInfoHandler;
        }

        public async Task<List<RegionInfo>> Handle(ScrapWig100Command request, CancellationToken cancellationToken)
        {
            var regions = await MapsterRunner.GetWig100Async();

            await _scrapedInfoHandler.Handle(regions, RegionType.Wig100);

            return regions;
        }
    }
}
