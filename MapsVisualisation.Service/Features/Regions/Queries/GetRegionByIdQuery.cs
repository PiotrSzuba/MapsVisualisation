using MapsVisualisation.Database;
using Microsoft.EntityFrameworkCore;
using MapsVisualisation.Service.BuildingBlocks;
using MapsVisualisation.Domain.Entities;
using MapsVisualisation.Service.Features.Regions.Shared;
using MapsVisualisation.Domain.Exceptions;
using MapsVisualisation.Domain.Helpers;

namespace MapsVisualisation.Service.Features.Regions.Queries;

public class GetRegionByIdQuery : IQuery<RegionDto>
{
    public Guid Id { get; set; }

    internal class GetRegionByIdHandler : IQueryHandler<GetRegionByIdQuery, RegionDto>
    {
        private readonly MapsVisualisationContext _context;

        public GetRegionByIdHandler(MapsVisualisationContext context)
        {
            _context = context;
        }

        public async Task<RegionDto> Handle(GetRegionByIdQuery request, CancellationToken cancellationToken)
        {
            var region = await _context.Regions
                .Include(r => r.Maps)
                .SingleOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

            if (region is null)
                throw new EntityNotFoundException(Errors.General.NotFound<Region>(request.Id));

            return new RegionDto
            {
                Id = region.Id,
                RegionIdentity = region.RegionIdentity,
                RegionName1 = region.RegionName1 ?? "",
                RegionName2 = region.RegionName2 ?? "",
                RegionName3 = region.RegionName3 ?? "",
                NELat = region.NELat,
                NELong = region.NELong,
                NWLat = region.NWLat,
                NWLong = region.NWLong,
                SELat = region.SELat,
                SELong = region.SELong,
                SWLat = region.SWLat,
                SWLong = region.SWLong,
                Type = region.RegionType,
                Maps = RegionDto.Mapper.MapMaps(region.Maps.ToList()),
            };
        }
    }
}
