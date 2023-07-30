#nullable disable

using Microsoft.EntityFrameworkCore;
using RockPaperScissors.Api.Data.Models;

namespace RockPaperScissors.Api.Data;

public class GameContext : DbContext
{
    public GameContext()
    {
    }

    public GameContext(DbContextOptions<GameContext> options) : base(options)
    {
    }

    public DbSet<Game> Games { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Move> Moves { get; set; }
    public DbSet<RestartRequest> RestartRequests { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }
}