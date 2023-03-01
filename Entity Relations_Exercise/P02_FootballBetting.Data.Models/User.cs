namespace P02_FootballBetting.Data.Models;

using P02_FootballBetting.Data.Common;
using System.ComponentModel.DataAnnotations;

public class User
{
    public User()
    {
        this.Bets = new HashSet<Bet>();
    }

    [Key]
    public int UserId { get; set; }

    [Required]
    [StringLength(ValidationConstants.UserUserNameMaxLength)]
    public string Username { get; set; } = null!;

    [Required]
    [StringLength(ValidationConstants.UserPasswordMaxLength)]
    public string Password { get; set; }

    [Required]
    [StringLength(ValidationConstants.UserEmailMaxLength)]
    public string Email { get; set; }

    [Required]
    [StringLength(ValidationConstants.UserNameMaxLength)]
    public string Name { get; set; }

    [Required]
    public decimal Balance { get; set; }
    public virtual ICollection<Bet> Bets { get; set; }
}