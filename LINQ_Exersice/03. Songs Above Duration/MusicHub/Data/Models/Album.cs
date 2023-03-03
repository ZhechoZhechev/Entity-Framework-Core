﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicHub.Data.Models;


public class Album
{
    public Album()
    {
        this.Songs = new HashSet<Song>();
    }
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(40)]
    public string Name { get; set; }

    [Required]
    public DateTime ReleaseDate { get; set; }

    public decimal Price => Songs.Sum(s => s.Price);

    [ForeignKey(nameof(Producer))]
    public int? ProducerId { get; set; }
    public Producer Producer { get; set; }

    public virtual ICollection<Song> Songs { get; set; } = null!;
}