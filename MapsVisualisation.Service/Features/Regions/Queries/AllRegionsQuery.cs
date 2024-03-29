﻿using MapsVisualisation.Database;
using Microsoft.EntityFrameworkCore;
using MapsVisualisation.Service.BuildingBlocks;
using MapsVisualisation.Domain.Exceptions;
using MapsVisualisation.Domain.Entities;
using MapsVisualisation.Service.Features.Regions.Shared;
using MapsVisualisation.Domain.Helpers;
using MapsVisualisation.Service.Services;

namespace MapsVisualisation.Service.Features.Regions.Queries;

public class AllRegionsQuery : IQuery<List<RegionDto>>
{
    internal class AllRegionsQueryHandler : IQueryHandler<AllRegionsQuery, List<RegionDto>>
    {
        private readonly MapsVisualisationContext _context;
        private readonly IDomainProvider _domainProvider;
        private readonly IPathProvider _pathProvider;

        public AllRegionsQueryHandler(
            MapsVisualisationContext context,
            IDomainProvider domainProvider,
            IPathProvider pathProvider)
        {
            _context = context;
            _pathProvider = pathProvider;
            _domainProvider = domainProvider;
        }

        public async Task<List<RegionDto>> Handle(AllRegionsQuery request, CancellationToken cancellationToken)
        {
            var regions = await _context.Regions
                .Include(r => r.Maps)
                .Include(r => r.OtherSources)
                .ToListAsync(cancellationToken);

            if (regions is null || regions.Count == 0)
                throw new EntityNotFoundException(Errors.General.EmptyTable<Region>());

            return regions.Select(region => new RegionDto
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
                Maps = RegionDto.Mapper.MapMaps(region.Maps.ToList(), _pathProvider, _domainProvider),
                OtherSources = RegionDto.Mapper.MapOtherSources(region.OtherSources.ToList()),
            }).ToList();
        }
    }
}