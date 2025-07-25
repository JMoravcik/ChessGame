using ChessGame.ChessService.ChessLogic.ChessboardComponents;
using ChessGame.ChessService.ChessLogic.ChessboardComponents.Moves;

namespace ChessGame.ChessService.ChessLogic.Pieces;

public class Knight : Piece
{
    protected override int PieceId => 3;
    public Knight(bool isWhite) : base(isWhite)
    {
    }

    public override IEnumerable<Move> GetPossibleAttacks(Chessboard chessboard, bool forThreatening)
    {
        return GetKnightMoves(chessboard, forThreatening);
    }

    public override IEnumerable<Move> GetPossibleMoves(Chessboard chessboard)
    {
        return GetKnightMoves(chessboard, false);
    }

    private IEnumerable<Move> GetKnightMoves(Chessboard chessboard, bool forThreatening)
    {
        var field = chessboard.GetPiecePosition(this);
        int[] rowMoves = { -2, -1, 1, 2, 2, 1, -1, -2 };
        int[] colMoves = { -1, -2, -2, -1, 1, 2, 2, 1 };

        foreach (var (rowMove, colMove) in rowMoves.Zip(colMoves))
        {
            int newRow = field.Row + rowMove;
            int newCol = field.Column + colMove;

            if (IsValidNormalMove(chessboard, field, newRow, newCol, forThreatening, out var targetField))
            {
                yield return targetField;
            }
        }
    }
}
