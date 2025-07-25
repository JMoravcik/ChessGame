namespace ChessGame.Common.Entities.Game;

public class GameRecord : Entity
{
    public string ServerUrl { get; set; } = string.Empty;
    public Guid WhitePlayerId { get; set; }
    public Guid? BlackPlayerId { get; set; }
    public bool IsFinished { get; set; } = false;
}
