
using ChessGame.ChessService.ChessLogic.ChessboardComponents;
using ChessGame.ChessService.ChessLogic.ChessboardComponents.Moves;
using System.Diagnostics.CodeAnalysis;

namespace ChessGame.ChessService.ChessLogic.Pieces;

public class King : Piece
{
    protected override int PieceId => 6;
    public King(bool isWhite) : base(isWhite)
    {
    }

    public override IEnumerable<Move> GetPossibleAttacks(Chessboard chessboard, bool forThreatening)
    {
        return GetKingMoves(chessboard, forThreatening);
    }

    public override IEnumerable<Move> GetPossibleMoves(Chessboard chessboard)
    {
        foreach (var move in GetKingMoves(chessboard, false))
            yield return move;

        foreach (var castlingMove in GetCastlingMoves(chessboard))
            yield return castlingMove;
    }

    private IEnumerable<Move> GetKingMoves(Chessboard chessboard, bool forThreatening)
    {
        var field = chessboard.GetPiecePosition(this);
        for (int row = -1; row <= 1; row++)
        {
            for (int col = -1; col <= 1; col++)
            {
                if (row == 0 && col == 0) continue; // Skip the current field
                int newRow = field.Row + row;
                int newCol = field.Column + col;

                if (IsValidNormalMove(chessboard, field, newRow, newCol, forThreatening, out var piecesMove))
                {
                    yield return piecesMove;
                }
            }
        }
    }

    public IEnumerable<Move> GetCastlingMoves(Chessboard chessboard)
    {
        var field = chessboard.GetPiecePosition(this);
        if (HasMoved || field.IsThreatenedFor(IsWhite) || IsNotAtBasePosition(field))
            yield break;

        if (CanDoCastling(chessboard, field, 0, out var castlingField))
            yield return new CastlingMove(field, castlingField, false);

        if (CanDoCastling(chessboard, field, 7, out castlingField))
            yield return new CastlingMove(field, castlingField, false);
    }

    private bool IsNotAtBasePosition(Field field)
        => field.Row != (IsWhite ? 0 : 7) || field.Column != 4;

    private bool CanDoCastling(Chessboard chessboard, Field kingField, int rookColumn, [NotNullWhen(true)] out Field? castlingField)
    {
        castlingField = null;
        int rookRow = IsWhite ? 0 : 7;
        var rookField = chessboard[rookRow, rookColumn];

        if (rookField.Piece is not Rook rook || rook.HasMoved)
            return false;

        int direction = rookColumn < kingField.Column ? -1 : 1;

        if (FieldsBetweenRookAndKingAreOccupied(chessboard, kingField, rookField, direction))
            return false;

        if (chessboard[kingField.Row, kingField.Column + direction].IsThreatenedFor(IsWhite))
            return false;

        castlingField = chessboard[kingField.Row, kingField.Column + direction * 2];

        if (castlingField.IsThreatenedFor(IsWhite))
            return false;

        return true;
    }

    private bool FieldsBetweenRookAndKingAreOccupied(Chessboard chessboard, Field kingField, Field rookField, int direction)
    {
        for (int col = kingField.Column + direction; col != rookField.Column; col += direction)
        {
            if (chessboard[kingField.Row, col].IsOccupied)
                return true;
        }

        return false;
    }



}
