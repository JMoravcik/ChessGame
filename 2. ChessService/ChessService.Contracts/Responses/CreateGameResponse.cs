using ChessGame.Common.Entities.Game;

namespace ChessGame.ChessService.Contracts.Responses;

public class CreateGameResponse
{
    public required GameRecord GameRecord { get; set; }
}