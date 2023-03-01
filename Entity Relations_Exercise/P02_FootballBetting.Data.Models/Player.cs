namespace P02_FootballBetting.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;

public class Player
{
    public Player()
    {
        this.PlayersStatistics = new HashSet<PlayerStatistic>();
    }

    [Key]
    public int PlayerId { get; set; }

    [Required]
    [StringLength(ValidationConstants.PlayerNameMaxLength)]
    public string Name { get; set; } = null!;

    [Required]
    public int SquadNumber { get; set; }

    [Required]
    [ForeignKey(nameof(Team))]
    public int TeamId { get; set; }
    public Team Team { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(Position))]
    public int PositionId { get; set; }
    public Position Position { get; set; } = null!;

    [Required]
    public bool IsInjured { get; set; }

    public virtual ICollection<PlayerStatistic> PlayersStatistics { get; set; } = null!;

}
