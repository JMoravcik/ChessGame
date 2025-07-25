namespace ChessGame.ChessService.Contracts.Responses;

public class GetGameStateResponse
{
    public required int[][]? Minimap { get; set; }
    public List<string> LegalMoves { get; set; } = new List<string>();
    public required bool IsFinished { get; set; }
}
