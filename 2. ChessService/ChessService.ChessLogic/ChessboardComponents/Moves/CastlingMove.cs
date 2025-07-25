using ChessGame.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.ChessService.ChessLogic.ChessboardComponents.Moves;

public class CastlingMove : Move
{
    public CastlingMove(Move move) : base(move)
    {
    }

    public CastlingMove(string sourceField, string targetField, bool forThreatening) : base(sourceField, targetField, forThreatening)
    {
    }

    public CastlingMove(Field sourceField, Field targetField, bool forThreatening) : base(sourceField, targetField, forThreatening)
    {
    }

    public override void ExecuteMove(Chessboard chessboard)
    {
        base.ExecuteMove(chessboard);
        GetRookCastlePositions(chessboard, out var rookField, out var rookTargetField);

        if (rookField.Piece is null)
            throw new ChessCoreException($"If rook is not on field '{rookField}' then this move is illegal");

        chessboard.MovePiece(rookField.Piece, rookTargetField);
    }

    public override void RevertMove(Chessboard chessboard)
    {
        base.RevertMove(chessboard);
        GetRookCastlePositions(chessboard, out var rookField, out var rookTargetField);

        if (rookTargetField.Piece is null)
            throw new ChessCoreException($"If rook is not on field '{rookField}' then this move is illegal");

        chessboard.MovePiece(rookTargetField.Piece, rookField);
    }

    private void GetRookCastlePositions(Chessboard chessboard, out Field rookField, out Field rookTargetField)
    {
        var targetField = chessboard[TargetField];
        rookField = chessboard[targetField.Row, targetField.Column < 4 ? 0 : 7];
        rookTargetField = chessboard[targetField.Row, targetField.Column < 4 ? 3 : 5];
    }
}
