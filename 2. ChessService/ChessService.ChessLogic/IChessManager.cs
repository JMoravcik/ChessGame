using ChessGame.Common.Entities.Game;

namespace ChessGame.ChessService.ChessLogic;

public interface IChessManager
{
    int MaxGameCount { get; }
    int CurrentGameCount { get; }

    bool NoMoreSpace => CurrentGameCount >= MaxGameCount;

    Guid CreateNewGame(Guid whitePlayerId, Guid? blackPlayerId);
    int[][]? GetMinimap(Guid gameId);
    List<string> GetMoves(Guid gameId, Guid playerId);
    void JoinGame(Guid gameId, Guid blackPlayerId);
}
