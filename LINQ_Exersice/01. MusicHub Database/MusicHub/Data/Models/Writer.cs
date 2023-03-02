namespace MusicHub.Data.Models;

using System.Collections;
using System.ComponentModel.DataAnnotations;


public class Writer
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public string Name { get; set; } = null!;

    public string? Pseudonym { get; set; }

    public virtual ICollection<Song> Songs { get; set; } = null!;

}