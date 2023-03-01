﻿namespace P02_FootballBetting.Data.Models;

using System.ComponentModel.DataAnnotations;

public class Position
{
    public Position()
    {
        this.Players = new HashSet<Player>();
    }

    [Key]
    public int PositionId { get; set; }

    [Required]
    [StringLength(25)]
    public string Name { get; set; } = null!;

    public virtual ICollection<Player> Players { get; set; } = null!;
}