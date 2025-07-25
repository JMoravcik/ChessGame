namespace ChessGame.ChessService.Contracts.MoveResults;

public record FinishMove : MoveResult
{
    public Guid? Winner { get; }
    public FinishMove(Guid? winner)
    {
        Winner = winner;
    }
}
