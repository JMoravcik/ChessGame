
using ChessGame.Common.Entities.Game;
using Microsoft.EntityFrameworkCore;

namespace ChessGame.Backend.Common.Scopeds.Database;

public partial class ChessgameDbContext : IGameRepository
{
    public async Task<GameRecord> CreateGameRecordAsync(Guid gameId, Guid whitePlayerId, Guid? blackPlayerId)
    {
        var gameRecord = new GameRecord
        {
            Id = gameId,
            WhitePlayerId = whitePlayerId,
            BlackPlayerId = null, // Initially no black player
            ServerUrl = _configuration["BaseUrl"] ?? string.Empty
        };
        GameRecords.Add(gameRecord);
        await SaveChangesAsync();
        return gameRecord;
    }

    public async Task<bool> UpdateGameRecordAsync(Guid gameId, Guid? blackPlayerId)
    {
        var gameRecord = await GameRecords.FindAsync(gameId);
        if (gameRecord == null)
            return false;

        gameRecord.BlackPlayerId = blackPlayerId;
        await SaveChangesAsync();
        return true;
    }

    public async Task<List<GameRecord>> GetGamesAsync()
    {
        return await GameRecords.ToListAsync();
    }

    public async Task<GameRecord?> GetPlayersCurrentGameAsync(Guid playerId)
    {
        return await GameRecords
            .FirstOrDefaultAsync(gr => (gr.WhitePlayerId == playerId || gr.BlackPlayerId == playerId) && !gr.IsFinished);
    }

    public async Task UpdateGameRecordAsync(Guid gameId, bool isFinished)
    {
        var gameRecord = await GameRecords.FirstOrDefaultAsync(record => record.Id == gameId);
        if (gameRecord == null)
            return;

        gameRecord.IsFinished = isFinished;
        await SaveChangesAsync();
    }
}
