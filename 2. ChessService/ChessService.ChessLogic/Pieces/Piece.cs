using ChessGame.ChessService.ChessLogic.ChessboardComponents;
using ChessGame.ChessService.ChessLogic.ChessboardComponents.Moves;
using System.Diagnostics.CodeAnalysis;

namespace ChessGame.ChessService.ChessLogic.Pieces;

public abstract class Piece
{
    public readonly bool IsWhite;
    public Guid UId { get; set; } = Guid.NewGuid();
    public bool HasMoved { get; internal set; } = false;

    public Piece(bool isWhite)
    {
        IsWhite = isWhite;
    }

    public void PieceMoved()
    {
        HasMoved = true;
    }

    public Piece Clone()
    {
        var memberwiseClone = (Piece)this.MemberwiseClone();
        memberwiseClone.UId = UId;
        return memberwiseClone;
    }

    protected abstract int PieceId { get; }

    public int GetPieceId() => (IsWhite ? 0 : 6) + this.PieceId;

    public abstract IEnumerable<Move> GetPossibleMoves(Chessboard chessboard);

    public abstract IEnumerable<Move> GetPossibleAttacks(Chessboard chessboard, bool forThreatening);

    protected IEnumerable<Move> DirectionalMove(Chessboard chessboard, Field field, int vRow, int vCol, bool forThreatening = false)
    {
        for (int i = 1; i < 8; i++)
        {
            int newRow = field.Row + vRow * i;
            int newCol = field.Column + vCol * i;

            if (!IsValidNormalMove(chessboard, field, newRow, newCol, forThreatening, out var piecesMove))
                break;

            yield return piecesMove;

            if (piecesMove.TargetFieldIsNotEmpty())
                break;
        }
    }



    protected bool IsValidNormalMove(Chessboard chessboard, Field field, int newRow, int newCol, bool forThreatening, [NotNullWhen(true)] out Move? piecesMove)
    {
        piecesMove = null;
        if (!chessboard.IsValidField(newRow, newCol))
            return false;

        var targetField = chessboard[newRow, newCol];
        
        if (targetField.Piece == null)
        {
            piecesMove = new Move(field, targetField, forThreatening);
            return true;
        }

        piecesMove = new Move(field, targetField, forThreatening);
        return targetField.Piece.IsWhite != IsWhite || forThreatening;
    }

    public static Piece CreatePiece(int pieceId) => pieceId switch
    {
        1 => new Pawn(true),
        2 => new Rook(true),
        3 => new Knight(true),
        4 => new Bishop(true),
        5 => new Queen(true),
        6 => new King(true),
        7 => new Pawn(false),
        8 => new Rook(false),
        9 => new Knight(false),
        10 => new Bishop(false),
        11 => new Queen(false),
        12 => new King(false),
        _ => throw new ArgumentOutOfRangeException(nameof(pieceId), "Invalid piece ID")
    };

}
