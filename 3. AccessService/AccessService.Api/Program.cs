using ChessGame.AccessService.Api;
using ChessGame.Backend.Common.Scopeds.Database;
using ChessGame.Common.Bases;
using ChessGame.Common.DI_Tools;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<IRepository, ChessgameDbContext>(options =>
{
    var connectionString = builder.Configuration.GetRequiredSection("Database:ConnectionString").Get<string>();
    options.UseNpgsql(connectionString, optionsBuilder => optionsBuilder.MigrationsAssembly(Assembly.GetExecutingAssembly()!));
});

builder.Services.AddScoped<LinkerContext, AccessServiceLinkerContext>();
builder.Services.SetupDependencies(new(builder.Configuration));
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.StartupDependencies();
await app.Services.WarmUpAsync();
UpdateDatabase(app);

app.Run();

void UpdateDatabase(IHost buildedApp)
{
    using var serviceScope = buildedApp.Services.CreateScope();
    using var databaseContext = serviceScope.ServiceProvider.GetRequiredService<IRepository>() as ChessgameDbContext
                             ?? throw new Exception("Database not registered in dependencies!");

    Console.WriteLine($"count of pending migrations: {databaseContext.Database.GetPendingMigrations().Count()}");
    databaseContext.Database.Migrate();
}