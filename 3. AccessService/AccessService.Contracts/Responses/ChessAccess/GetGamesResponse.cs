using ChessGame.Common.Entities.Game;

namespace ChessGame.AccessService.Contracts.Responses.ChessAccess;

public class GetGamesResponse
{
    public required List<GameRecord> GameRecords { get; set; }
    public GameRecord? Playing { get; set; }
}
