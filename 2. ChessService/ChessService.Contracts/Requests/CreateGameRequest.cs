namespace ChessGame.ChessService.Contracts.Requests;

public class CreateGameRequest
{
    public Guid? BlackPlayerId { get; set; } = null; // Optional, can be null if the game is created without a black player
}
