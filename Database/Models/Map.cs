using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models;

public class Map
{
    [Key]
    public int MapId { get; set; }
    [Required]
    [ForeignKey("Region")]
    public int RegionId { get; set; }
    public int PublishYear { get; set; }
    public int dpi { get; set; }
    public string? CollectionName { get; set; }

    public virtual Region? Region { get; set; }
    public virtual Image? Image { get; set; }
}
