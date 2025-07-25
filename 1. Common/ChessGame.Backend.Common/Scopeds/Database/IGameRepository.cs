using ChessGame.Common.Entities.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Backend.Common.Scopeds.Database;

public interface IGameRepository
{
    public Task<GameRecord> CreateGameRecordAsync(Guid gameId, Guid whitePlayerId, Guid? blackPlayerId);
    Task<List<GameRecord>> GetGamesAsync();
    Task<GameRecord?> GetPlayersCurrentGameAsync(Guid playerId);
    Task<bool> UpdateGameRecordAsync(Guid gameId, Guid? blackPlayerId);
    Task UpdateGameRecordAsync(Guid gameId, bool isFinished);
}
