using ChessGame.Backend.Common.Scopeds.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;

namespace ChessGame.AccessService.Api;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ChessgameDbContext>
{
    public ChessgameDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ChessgameDbContext>();
        optionsBuilder.UseNpgsql("YourConnectionString", optionsBuilder => optionsBuilder.MigrationsAssembly(Assembly.GetExecutingAssembly())); // nebo načti z configu

        return new ChessgameDbContext(optionsBuilder.Options, 
            new ConfigurationBuilder().Build(), 
            new Logger<ChessgameDbContext>(LoggerFactory.Create(config => config.AddConsole())));
    }
}
