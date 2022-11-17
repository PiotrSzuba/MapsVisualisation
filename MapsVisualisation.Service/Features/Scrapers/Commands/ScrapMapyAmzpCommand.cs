using MapsVisualisation.Service.BuildingBlocks;
using MapsVisualisation.Service.Features.Scrapers.Shared;
using MapsVisualisation.WebScrapers;

namespace MapsVisualisation.Service.Features.Scrapers.Commands;

public class ScrapMapyAmzpCommand : ICommand<List<RegionInfo>>
{
    internal class ScrapMapyAmzpCommandHandler : ICommandHandler<ScrapMapyAmzpCommand, List<RegionInfo>>
    {
        private readonly IScrapedInfoHandler _scrapedInfoHandler;

        public ScrapMapyAmzpCommandHandler(IScrapedInfoHandler scrapedInfoHandler)
        {
            _scrapedInfoHandler = scrapedInfoHandler;
        }

        public async Task<List<RegionInfo>> Handle(ScrapMapyAmzpCommand request, CancellationToken cancellationToken)
        {
            var regions = MapyAmzpScraper.Run();

            await _scrapedInfoHandler.Handle(regions);

            return regions;
        }
    }
}