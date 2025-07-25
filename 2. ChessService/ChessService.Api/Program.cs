using ChessGame.Backend.Common.Scopeds.Database;
using ChessGame.Common.DI_Tools;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<IRepository, ChessgameDbContext>(options =>
{
    var connectionString = builder.Configuration.GetRequiredSection("Database:ConnectionString").Get<string>();
    options.UseNpgsql(connectionString);
});
builder.Services.SetupDependencies(new EnvironmentInfo(builder.Configuration));

var app = builder.Build();

app.StartupDependencies();
await app.Services.WarmUpAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

