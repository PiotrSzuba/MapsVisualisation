using MapsVisualisation.Service.Features.Scrapers.Shared;
using MapsVisualisation.Service.BuildingBlocks;
using MapsVisualisation.WebScrapers.RegionScrapers;
using MapsVisualisation.WebScrapers.RegionScrapers.Mapster;
using MapsVisualisation.Domain.Entities;

namespace MapsVisualisation.Service.Features.Scrapers.Commands;

public class ScrapMesstischblattCommand : ICommand<List<RegionInfo>>
{
    internal class ScrapMesstischblattCommandHandler : ICommandHandler<ScrapMesstischblattCommand, List<RegionInfo>>
    {
        private readonly IScrapedInfoHandler _scrapedInfoHandler;

        public ScrapMesstischblattCommandHandler(IScrapedInfoHandler scrapedInfoHandler)
        {
            _scrapedInfoHandler = scrapedInfoHandler;
        }

        public async Task<List<RegionInfo>> Handle(ScrapMesstischblattCommand request, CancellationToken cancellationToken)
        {
            var regions = await MapsterRunner.GetMesstischblattAsync();

            await _scrapedInfoHandler.Handle(regions, RegionType.Messtischblatt);

            return regions;
        }
    }
}
