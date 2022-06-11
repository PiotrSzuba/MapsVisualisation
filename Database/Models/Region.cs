using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models;

public class Region
{
    [Key]
    public int RegionId { get; set; }
    public string? PolishRegionName { get; set; }
    public string? GermanRegionName { get; set; }

    public virtual ICollection<Map>? Maps { get; set; }
}
