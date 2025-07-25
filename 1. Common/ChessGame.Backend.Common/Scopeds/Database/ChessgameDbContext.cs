using ChessGame.Common.Entities.Game;
using ChessGame.Common.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ChessGame.Backend.Common.Scopeds.Database;

public partial class ChessgameDbContext : DbContext, IRepository
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ChessgameDbContext> _logger;

    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<BasicAuth> BasicAuths { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<GameRecord> GameRecords { get; set; }

    public ChessgameDbContext(DbContextOptions<ChessgameDbContext> options, IConfiguration configuration, ILogger<ChessgameDbContext> logger)
        : base(options)
    {
        _configuration = configuration;
        _logger = logger;
    }
}
