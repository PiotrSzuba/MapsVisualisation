using MapsVisualisation.Service.Features.Scrapers.Shared;
using MapsVisualisation.Service.BuildingBlocks;
using MapsVisualisation.WebScrapers.RegionScrapers;
using MapsVisualisation.WebScrapers.RegionScrapers.Mapster;
using MapsVisualisation.Domain.Entities;

namespace MapsVisualisation.Service.Features.Scrapers.Commands;

public class ScrapAusHun75Command : ICommand<List<RegionInfo>>
{
    internal class ScrapAusHun75CommandHandler : ICommandHandler<ScrapAusHun75Command, List<RegionInfo>>
    {
        private readonly IScrapedInfoHandler _scrapedInfoHandler;

        public ScrapAusHun75CommandHandler(IScrapedInfoHandler scrapedInfoHandler)
        {
            _scrapedInfoHandler = scrapedInfoHandler;
        }

        public async Task<List<RegionInfo>> Handle(ScrapAusHun75Command request, CancellationToken cancellationToken)
        {
            var regions = await MapsterRunner.GetAusHunMonarch75Async();

            await _scrapedInfoHandler.Handle(regions, RegionType.AusHun75);

            return regions;
        }
    }
}