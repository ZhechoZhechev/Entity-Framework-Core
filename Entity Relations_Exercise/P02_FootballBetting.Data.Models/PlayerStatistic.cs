namespace P02_FootballBetting.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class PlayerStatistic
{
    [Required]
    [ForeignKey(nameof(Game))]
    public int GameId { get; set; }
    public virtual Game Game { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(Player))]
    public int PlayerId { get; set; }
    public virtual Player Player { get; set; } = null!;

    [Required]
    public byte ScoredGoals { get; set; }

    [Required]
    public byte Assists { get; set; }

    [Required]
    public byte MinutesPlayed { get; set; }
}
