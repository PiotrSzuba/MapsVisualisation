using MapsVisualisation.Domain.Exceptions;

namespace MapsVisualisation.Domain.Entities;

public class Region
{
    private List<Map> _maps = new();
    private List<OtherSource> _OtherSources = new();

    public Guid Id { get; private set; }
    public string? RegionName1 { get; private set; } = string.Empty;
    public string? RegionName2 { get; private set; } = string.Empty;
    public string? RegionName3 { get; private set; } = string.Empty;
    public string RegionIdentity { get; private set; } = string.Empty;
    public double NWLat { get; private set; }
    public double NWLong { get; private set; }
    public double NELat { get; private set; }
    public double NELong { get; private set; }
    public double SELat { get; private set; }
    public double SELong { get; private set; }
    public double SWLat { get; private set; }
    public double SWLong { get; private set; }
    public RegionType RegionType { get; private set; }
    public IReadOnlyCollection<Map> Maps => _maps.AsReadOnly();
    public IReadOnlyCollection<OtherSource> OtherSources => _OtherSources.AsReadOnly();

    public Region() { } //ef

    private Region(string regionName1, string regionName2, string regionName3, string regionIdentity)
    {
        RegionName1 = regionName1;
        RegionName2 = regionName2;
        RegionName3 = regionName3;
        RegionIdentity = regionIdentity;
        RegionType = GetRegionType();
        SetGeoPoints();
    }

    public static Region Create(string regionName1, string regionName2, string regionName3, string regionIdentity)
    {
        var newRegion = new Region(regionName1, regionName2, regionName3, regionIdentity);

        return newRegion;
    }

    public void AddMap(int year, int dpi, string imageUrl, string? collectionName = null, string? thumbnail = null)
    {
        _maps.Add(Map.Create(this, year, dpi, imageUrl, collectionName, thumbnail));
    }

    private RegionType GetRegionType()
    {
        if (RegionIdentity.ToLower().Contains('p') || RegionIdentity.ToLower().Contains('s'))
            return RegionType.IgrekAmzp;

        return RegionType.MapyAmzp;
    }

    private void SetGeoPoints()
    {
        if (RegionType == RegionType.IgrekAmzp)
        {
            SetGeoPointsForIgrek();
            return;
        }
        SetGeoPointsForMapy();
    }

    private void SetGeoPointsForIgrek()
    {
        var nums = RegionIdentity.Split(' ');

        if (nums.Length != 2)
            throw new EntityInvalidStateException("Failed to split Region identity");

        if (!nums[0].ToLower().Contains('p'))
            throw new EntityInvalidStateException("First half of Region identity is wrong");

        if (!nums[1].ToLower().Contains('s'))
            throw new EntityInvalidStateException("First half of Region identity is wrong");

        var pas = new string(nums[0].Where(char.IsDigit).ToArray());
        var slup = new string(nums[1].Where(char.IsDigit).ToArray());

        var baseLat = int.Parse(pas);
        var baseLong = int.Parse(slup);

        NWLat = 62.25 - (float)baseLat / 4;
        NWLong = (float)baseLong / 2 + (float)29 / 6;
        NELat = NWLat;
        NELong = NWLong + (float)1 / 2;
        SELat = NWLat - 0.25;
        SELong = NWLong + (float)1 / 2;
        SWLat = NWLat - 0.25;
        SWLong = NWLong;
    }

    private void SetGeoPointsForMapy()
    {
        if (RegionIdentity.Length == 4)
        {
            NWLat = 56 - (float)Convert.ToInt32(RegionIdentity.Substring(0, 2)) / 10;
            NWLong = (float)Convert.ToInt32(RegionIdentity.Substring(2, 2)) / 6 + (float)17 / 3;
        }
        else if (RegionIdentity.Length == 5)
        {
            NWLat = 56 - (float)Convert.ToInt32(RegionIdentity.Substring(0, 2)) / 10;
            NWLong = (float)Convert.ToInt32(RegionIdentity.Substring(2, 3)) / 6 + (float)17 / 3;
        }
        else
        {
            throw new Exception("New kind of number");
        }

        NELat = NWLat;
        NELong = NWLong + (float)1 / 6;
        SELat = NWLat - 0.1;
        SELong = NWLong + (float)1 / 6;
        SWLat = NWLat - 0.1;
        SWLong = NWLong;
    }
}

public enum RegionType
{
    IgrekAmzp,
    MapyAmzp,
    unknown,
}
