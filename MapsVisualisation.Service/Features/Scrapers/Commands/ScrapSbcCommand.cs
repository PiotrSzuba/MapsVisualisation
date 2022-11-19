using MapsVisualisation.Database;
using MapsVisualisation.Domain.Entities;
using MapsVisualisation.Service.BuildingBlocks;
using MapsVisualisation.WebScrapers.OtherSourcesScrapers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MapsVisualisation.Service.Features.Scrapers.Commands;

public class ScrapSbcCommand : ICommand
{
    internal class ScrapSbcCommandHandler : ICommandHandler<ScrapSbcCommand>
    {
        private readonly MapsVisualisationContext _context;

        public ScrapSbcCommandHandler(MapsVisualisationContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ScrapSbcCommand request, CancellationToken cancellationToken)
        {
            var regions = await _context.Regions
                .Where(r => r.RegionType == RegionType.MapyAmzp)
                .Include(r => r.OtherSources)
                .ToListAsync(cancellationToken: cancellationToken);

            foreach (var region in regions)
            {
                var sources = SbcScrapper.Run(region);

                foreach (var source in sources)
                {
                    if (region.OtherSources.SingleOrDefault(os => os.Url == source.Url) is not null) 
                        continue;

                    region.AddOtherSource(source.Name, source.Url, source.Year);
                }
                if (sources.Count > 0)
                {
                    _context.Regions.Update(region);
                }
            }

            return Unit.Value;
        }
    }
}
