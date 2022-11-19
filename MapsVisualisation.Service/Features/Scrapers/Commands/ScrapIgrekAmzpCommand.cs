using MapsVisualisation.Service.Features.Scrapers.Shared;
using MapsVisualisation.Service.BuildingBlocks;
using MapsVisualisation.WebScrapers.RegionScrapers;

namespace MapsVisualisation.Service.Features.Scrapers.Commands;

public class ScrapIgrekAmzpCommand : ICommand<List<RegionInfo>>
{
    internal class ScrapIgrekAmzpCommandHandler : ICommandHandler<ScrapIgrekAmzpCommand, List<RegionInfo>>
    {
        private readonly IScrapedInfoHandler _scrapedInfoHandler;

        public ScrapIgrekAmzpCommandHandler(IScrapedInfoHandler scrapedInfoHandler)
        {
            _scrapedInfoHandler = scrapedInfoHandler;
        }

        public async Task<List<RegionInfo>> Handle(ScrapIgrekAmzpCommand request, CancellationToken cancellationToken)
        {
            var regions = IgrekAmzpScraper.Run();

            await _scrapedInfoHandler.Handle(regions);

            return regions;
        }
    }
}
