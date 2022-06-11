using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models;

public class Image
{
    [Key]
    public int ImageId { get; set; }
    [Required]
    [ForeignKey("Map")]
    public int MapId { get; set; }
    [Required]
    [Column(TypeName = "varbinary(max)")]
    public byte[]? imageData { get; set; }

    public virtual Map? map { get; set; }
}
