using MapsVisualisation.Database;
using MapsVisualisation.Domain.Entities;
using MapsVisualisation.Service.BuildingBlocks;
using MapsVisualisation.WebScrapers.OtherSourcesScrapers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MapsVisualisation.Service.Features.Scrapers.Commands;

public class ScrapKpbcCommand : ICommand
{
    internal class ScrapKpbcCommandHandler : ICommandHandler<ScrapKpbcCommand>
    {
        private readonly MapsVisualisationContext _context;

        public ScrapKpbcCommandHandler(MapsVisualisationContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ScrapKpbcCommand request, CancellationToken cancellationToken)
        {
            var regions = await _context.Regions
                .Include(r => r.OtherSources)
                .ToListAsync(cancellationToken: cancellationToken);

            foreach (var region in regions)
            {
                var sources = KpbcScraper.Run(region);

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
