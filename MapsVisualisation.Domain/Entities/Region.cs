using MapsVisualisation.Domain.Exceptions;
using MapsVisualisation.Domain.Helpers;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MapsVisualisation.Domain.Entities;

public class Region
{
    private List<Map> _maps = new();
    private List<OtherSource> _otherSources = new();

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
    public IReadOnlyCollection<OtherSource> OtherSources => _otherSources.AsReadOnly();

    public Region() { } //ef

    private Region(
        string regionName1, 
        string regionName2, 
        string regionName3, 
        string regionIdentity,
        RegionType regionType)
    {
        RegionName1 = regionName1;
        RegionName2 = regionName2;
        RegionName3 = regionName3;
        RegionIdentity = regionIdentity;
        RegionType = regionType;
        SetGeoPoints();
    }

    public static Region Create(
        string regionName1, 
        string regionName2, 
        string regionName3, 
        string regionIdentity,
        RegionType regionType)
    {
        var newRegion = new Region(regionName1, regionName2, regionName3, regionIdentity, regionType);

        return newRegion;
    }

    public void AddMap(
        int year, 
        int dpi, 
        string imageUrl, 
        string? collectionName = null, 
        string? thumbnail = null, 
        string? localImage = null)
    {
        _maps.Add(Map.Create(this, year, dpi, imageUrl, collectionName, thumbnail, localImage));
    }

    public void AddOtherSource(string name, string url, int? year)
    {
        _otherSources.Add(OtherSource.Create(this, name, url, year));
    }

    private void SetGeoPoints()
    {
        switch (RegionType)
        {
            case RegionType.Wig25:
                SetGeoPointsForWig25();
                break;
            case RegionType.Wig100:
                SetGeoPointsForWig100();
                break;
            case RegionType.Messtischblatt:
                SetGeoPointsForMesstischblatt();
                break;
            case RegionType.AusHun75:
                SetGeoPointsForAusHun75();
                break;
            default:
                throw new EntityInvalidStateException("Unknown Region");
        }      
    }

    private void SetGeoPointsForWig100()
    {
        var nums = RegionIdentity.Split(' ');

        if (nums.Length != 2)
            throw new EntityInvalidStateException("Failed to split Region identity");

        if (!nums[0].ToLower().Contains('p'))
            throw new EntityInvalidStateException("First half of Region identity is wrong");

        if (!nums[1].ToLower().Contains('s'))
            throw new EntityInvalidStateException("First half of Region identity is wrong");

        var pas = int.Parse(new string(nums[0].Where(char.IsDigit).ToArray()));
        var slup = int.Parse(new string(nums[1].Where(char.IsDigit).ToArray()));

        NWLat = 62.25 - pas / 4.0;
        NWLong = slup / 2.0 + 29.0 / 6.0;
        NELat = NWLat;
        NELong = NWLong + 1.0 / 2.0;
        SELat = NWLat - 0.25;
        SELong = NELong;
        SWLat = SELat;
        SWLong = NWLong;
    }

    private void SetGeoPointsForWig25()
    {
        var names = RegionIdentity.Split(' ');

        if (names.Length != 3)
            throw new EntityInvalidStateException("Failed to split Region identity");

        if (!names[0].ToLower().Contains('p'))
            throw new EntityInvalidStateException("Pas in Region identity is wrong");

        if (!names[1].ToLower().Contains('s'))
            throw new EntityInvalidStateException("Slup in Region identity is wrong");

        if (names[2].Length != 1 || names[2][0] < 'a' || names[2][0] > 'i')
            throw new EntityInvalidStateException("id in Region identity is wrong");


        var offsetLong = (names[2][0] - 'a') % 3;
        var offsetLat = (names[2][0] - 'a') / 3;

        var pas = int.Parse(new string(names[0].Where(char.IsDigit).ToArray()));
        var slup = int.Parse(new string(names[1].Where(char.IsDigit).ToArray()));

        NWLat = 62.25 - pas / 4.0 - offsetLat * 1.0 / 12.0;
        NWLong = slup / 2.0 + 29.0 / 6.0 + offsetLong * 1.0 / 6.0;
        NELat = NWLat;
        NELong = NWLong + 1.0 / 6.0;
        SELat = NWLat - 1.0 / 12.0;
        SELong = NELong;
        SWLat = SELat;
        SWLong = NWLong;
    }

    private void SetGeoPointsForMesstischblatt()
    {
        if (RegionIdentity.Length == 3)
        {
            NWLat = 56 - Convert.ToInt32(RegionIdentity.Substring(0, 1)) / 10.0;
            NWLong = Convert.ToInt32(RegionIdentity.Substring(1, 2)) / 6.0 + 17.0 / 3.0;
        }
        else if (RegionIdentity.Length == 4)
        {
            NWLat = 56 - Convert.ToInt32(RegionIdentity.Substring(0, 2)) / 10.0;
            NWLong = Convert.ToInt32(RegionIdentity.Substring(2, 2)) / 6.0 + 17.0 / 3.0;
        }
        else if (RegionIdentity.Length == 5)
        {
            NWLat = 56 - Convert.ToInt32(RegionIdentity.Substring(0, 2)) / 10.0;
            NWLong = Convert.ToInt32(RegionIdentity.Substring(2, 3)) / 6.0 + 17.0 / 3.0;
        }
        else
        {
            throw new EntityInvalidStateException("Not a messtischblatt !");
        }

        NELat = NWLat;
        NELong = NWLong + 1.0 / 6.0;
        SELat = NWLat - 0.1;
        SELong = NELong;
        SWLat = SELat;
        SWLong = NWLong;
    }

    private void SetGeoPointsForAusHun75()
    {
        var zone = -1;
        var column = -1;

        var names = RegionIdentity.Split(' ');

        if (names.Length != 2)
        {
            throw new EntityInvalidStateException("Failed to split RegionIdentity");
        }
        if (names[0].All(c => char.IsDigit(c)))
        {
            zone = Convert.ToInt32(names[0]);
        }
        else if (names[0].Length == 1)
        {
            zone = int.Parse(names[0], NumberStyles.HexNumber) - 10;
        }
        else
        {
            throw new EntityInvalidStateException("Failed to parse zone !");
        }
        
        column = ArabicConverter.FromRoman(names[1]);

        if (column <= 0)
        {
            throw new EntityInvalidStateException("Failed to parse column !");
        }

        NWLat = -0.25 * zone + 51.5;
        NWLong = 0.5 * column + (float)53/6;
        NELat = NWLat;
        NELong = NWLong + 0.5;
        SELat = NWLat - 0.25;
        SELong = NELong;
        SWLat = SELat;
        SWLong = NWLong;
    }
}

public enum RegionType
{
    IgrekAmzp,
    MapyAmzp,
    Wig25,
    Wig100,
    Messtischblatt,
    AusHun75,
    unknown,
}
