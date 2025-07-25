using ChessGame.Backend.Common.Scopeds.Database;
using ChessGame.Common.Entities.Game;
using Microsoft.Extensions.Configuration;

namespace ChessGame.ChessService.ChessLogic;

public class ChessManager : IChessManager
{
    private readonly int _maxGameCount;
    private Dictionary<Guid, ChessGame> _games = new();
    public ChessManager(IConfiguration configuration)
    {
        _maxGameCount = configuration.GetSection("ChessLogic:ChessManager:MaxGameCount").Get<int>();
        _maxGameCount = _maxGameCount <= 0 ? 100 : _maxGameCount; // Default to 100 if not set or invalid
    }

    public int MaxGameCount => _maxGameCount;

    public int CurrentGameCount => _games.Count;

    public Guid CreateNewGame(Guid whitePlayerId, Guid? blackPlayerId)
    {
        var gameId = Guid.NewGuid();
        _games.Add(gameId, new(whitePlayerId, blackPlayerId));
        return gameId;
    }

    public List<string> GetMoves(Guid gameId, Guid playerId)
    {
        return _games[gameId].GetMoves(playerId);
    }

    public void JoinGame(Guid gameId, Guid blackPlayerId)
    {
        if (_games.TryGetValue(gameId, out var game))
        {
            game.JoinGame(blackPlayerId);
        }
        else
        {
            throw new KeyNotFoundException($"Game with ID {gameId} not found.");
        }

    }

    public int[][]? GetMinimap(Guid gameId)
        => _games.TryGetValue(gameId, out var game) ? game.GetMinimap() : null;

}
