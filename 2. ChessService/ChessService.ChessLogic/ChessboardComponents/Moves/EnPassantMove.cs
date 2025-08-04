using ChessGame.ChessService.ChessLogic.Pieces;
using ChessGame.Common.Exceptions;

namespace ChessGame.ChessService.ChessLogic.ChessboardComponents.Moves;

public class EnPassantMove : Move
{
    public EnPassantMove(Move move) : base(move)
    {
    }

    public EnPassantMove(string sourceField, string targetField, bool forThreatening) : base(sourceField, targetField, forThreatening)
    {
    }

    public override void ExecuteMove(Chessboard chessboard)
    {
        base.ExecuteMove(chessboard);
        bool isWhite = chessboard[TargetField].Piece!.IsWhite;
        Field capturingPawnField = GetEnPassantCapturingPawnPosition(chessboard, isWhite);

        if (capturingPawnField.Piece is not Pawn pawn)
            throw new ChessCoreException($"Field '{capturingPawnField}' must have pawn on it!");

        chessboard.RemovePiece(capturingPawnField);
    }

    public override void RevertMove(Chessboard chessboard)
    {
        base.RevertMove(chessboard);
        bool isWhite = chessboard[SourceField].Piece!.IsWhite;
        Field capturingPawnField = GetEnPassantCapturingPawnPosition(chessboard, isWhite);
        chessboard.ReturnPiece(capturingPawnField, !isWhite ? 1 : 7);
    }

    private Field GetEnPassantCapturingPawnPosition(Chessboard chessboard, bool isWhite)
    {
        var targetField = chessboard[TargetField];
        int capturingPawnFieldRow = isWhite ? targetField.Row - 1 : targetField.Row + 1;
        var capturingPawnField = chessboard[capturingPawnFieldRow, targetField.Column];
        return capturingPawnField;
    }
}
