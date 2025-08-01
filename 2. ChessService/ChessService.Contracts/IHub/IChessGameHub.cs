using ChessGame.ChessService.Contracts.HubMessages;

namespace ChessGame.ChessService.Contracts.IHub;

public interface IChessGameHub
{
    public const string HubUrl = "chess-game";
    public const string CorrectMoveChannel = "correct-move";
    public const string FinishMoveChannel = "finish-move";
    public const string InvalidMoveChannel = "invalid-move";

    Task LoginInfoAsync(AuthenticationInfo authenticationInfo);
    Task JoinGameAsync(Guid gameId);
    Task MovePieceAsync(string move);
}
