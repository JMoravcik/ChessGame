using ChessGame.ChessService.ChessLogic.Pieces;
using ChessGame.Common.Exceptions;

namespace ChessGame.ChessService.ChessLogic.ChessboardComponents.Moves;

public class PawnSprintMove : Move
{
    public PawnSprintMove(Move move) : base(move)
    {
    }

    public PawnSprintMove(string sourceField, string targetField, bool forThreatening) : base(sourceField, targetField, forThreatening)
    {
    }

    public override void ExecuteMove(Chessboard chessboard)
    {
        base.ExecuteMove(chessboard);
        if (chessboard[TargetField].Piece is not Pawn pawn)
            throw new ChessCoreException($"There is not pawn on target field after pawn sprint at '{TargetField}'");

        pawn.SprintedMove = chessboard.MoveCount;
    }

    public override void RevertMove(Chessboard chessboard)
    {
        base.RevertMove(chessboard);
        if (chessboard[SourceField].Piece is not Pawn pawn)
            throw new ChessCoreException($"There is not pawn on target field after pawn sprint at '{TargetField}'");

        pawn.SprintedMove = -1;
    }
}
