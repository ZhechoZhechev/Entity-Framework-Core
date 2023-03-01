namespace P02_FootballBetting.Data;

using Microsoft.EntityFrameworkCore;

using Common;
using Models;

public class FootballBettingContext: DbContext
{
    public FootballBettingContext()
    {
        
    }

    public FootballBettingContext(DbContextOptions options)
        :base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(DbConfig.ConnectionString);
        }
    }
    public DbSet<Team> Teams { get; set; } = null!;

    public DbSet<Color> Colors { get; set; } = null!;

    public DbSet<Town> Towns { get; set; } = null!;

    public DbSet<Country> Countries { get; set; } = null!;

    public DbSet<Player> Players { get; set; } = null!;

    public DbSet<Position> Positions { get; set; } = null!;

    public DbSet<PlayerStatistic> PlayersStatistics { get; set; } = null!;

    public DbSet<Game> Games { get; set; } = null!;

    public DbSet<Bet> Bets { get; set; } = null!;

    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlayerStatistic>()
            .HasKey(pk => new { pk.PlayerId, pk.GameId });

        modelBuilder.Entity<Team>(e =>
        {
            e.HasOne(t => t.PrimaryKitColor)
            .WithMany(c => c.PrimaryKitTeams)
            .HasForeignKey(fk => fk.PrimaryKitColorId)
            .OnDelete(DeleteBehavior.NoAction);

            e.HasOne(t => t.SecondaryKitColor)
            .WithMany(c => c.SecondaryKitTeams)
            .HasForeignKey(fk => fk.SecondaryKitColorId)
            .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Game>(e =>
        {
            e.HasOne(g => g.AwayTeam)
            .WithMany(t => t.AwayGames)
            .HasForeignKey(fk => fk.AwayTeamId)
            .OnDelete(DeleteBehavior.NoAction);

            e.HasOne(g => g.HomeTeam)
            .WithMany(t => t.HomeGames)
            .HasForeignKey(fk => fk.HomeTeamId)
            .OnDelete(DeleteBehavior.NoAction);
        });
    }

}