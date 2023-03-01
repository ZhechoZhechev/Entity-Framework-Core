namespace P02_FootballBetting.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Common;


public class Game
{
    public Game()
    {
        this.PlayersStatistics = new HashSet<PlayerStatistic>();
        this.Bets = new HashSet<Bet>();
    }

    [Key]
    public int GameId { get; set; }

    [Required]
    [ForeignKey(nameof(HomeTeam))]
    public int HomeTeamId { get; set; }
    public Team HomeTeam { get; set; }

    [Required]
    [ForeignKey(nameof(AwayTeam))]
    public int AwayTeamId { get; set; }
    public Team AwayTeam { get; set; }

    [Required]
    public byte HomeTeamGoals { get; set; }

    [Required]
    public byte AwayTeamGoals { get; set; }

    [Required]
    public DateTime DateTime { get; set; }

    [Required]
    public double HomeTeamBetRate { get; set; }

    [Required]
    public double AwayTeamBetRate { get; set; }

    [Required]
    public double DrawBetRate { get; set; }

    [Required]
    [StringLength(ValidationConstants.GameResultMaxLength)]
    public string? Result { get; set; }

    public virtual ICollection<PlayerStatistic> PlayersStatistics { get; set; } = null!;

    public virtual ICollection<Bet> Bets { get; set; } = null!;
}