using ChessGame.ChessService.ChessLogic.ChessboardComponents;
using ChessGame.ChessService.ChessLogic.ChessboardComponents.Moves;
using System.Diagnostics.CodeAnalysis;

namespace ChessGame.ChessService.ChessLogic.Pieces;

public class Pawn : Piece
{
    public int SprintedMove { get; set; } = -1;

    protected override int PieceId => 1;

    public Pawn(bool isWhite) : base(isWhite)
    {
    }

    public override IEnumerable<Move> GetPossibleAttacks(Chessboard chessboard, bool forThreatening)
    {
        var field = chessboard.GetPiecePosition(this);
        int row = IsWhite ? field.Row + 1 : field.Row - 1; // White moves up, Black moves down

        if (IsValidNormalMove(chessboard, field, row, field.Column + 1, forThreatening, out var piecesMove))
        {
            var targetField = chessboard[piecesMove.TargetField];

            if (PawnCanAttack(targetField))
            {
                if (IsPromotingField(targetField))
                {
                    yield return new PromotionMove(piecesMove);
                }
                else
                {
                    yield return piecesMove;
                }
            }
            else if (CanEnPassant(chessboard, targetField))
            {
                yield return new EnPassantMove(piecesMove);
            }
        }

        if (IsValidNormalMove(chessboard, field, row, field.Column - 1, forThreatening, out piecesMove))
        {
            var targetField = chessboard[piecesMove.TargetField];

            if (PawnCanAttack(targetField))
            {
                if (IsPromotingField(targetField))
                {
                    yield return new PromotionMove(piecesMove);
                }
                else
                {
                    yield return piecesMove;
                }
            }
            else if (CanEnPassant(chessboard, targetField))
            {
                yield return new EnPassantMove(piecesMove);
            }
        }
    }

    private bool PawnCanAttack(Field targetField)
        => targetField.Piece != null && targetField.Piece.IsWhite != IsWhite;

    private bool CanEnPassant(Chessboard chessboard, Field targetField)
    {
        int row = IsWhite ? targetField.Row - 1 : targetField.Row + 1; // White moves up, Black moves down

        if (!chessboard.IsValidField(row, targetField.Column))
            return false;

        var enPassantField = chessboard[row, targetField.Column];
        
        if (enPassantField.Piece is not Pawn pawn)
            return false;

        return pawn.IsWhite != IsWhite && pawn.SprintedMove == chessboard.MoveCount - 1;
    }


    public override IEnumerable<Move> GetPossibleMoves(Chessboard chessboard)
    {
        var field = chessboard.GetPiecePosition(this);
        int row = IsWhite ? field.Row + 1 : field.Row - 1; // White moves up, Black moves down
        if (IsValidNormalMove(chessboard, field, row, field.Column, false, out var piecesMove) && piecesMove.TargetFieldIsEmpty())
        {
            var targetField = chessboard[piecesMove.TargetField];
            if (IsPromotingField(targetField))
                yield return new PromotionMove(piecesMove);
            else
                yield return piecesMove;
        }
        if (PawnCanSprint(chessboard, field, row, ref piecesMove))
        {
            yield return new PawnSprintMove(piecesMove);
        }

        foreach (var attackField in GetPossibleAttacks(chessboard, false))
            yield return attackField;
    }

    private bool PawnCanSprint(Chessboard chessboard, Field field, int row, [NotNullWhen(true)] ref Move? piecesMove) 
        => field.Row == (IsWhite ? 1 : 6) 
        && IsValidNormalMove(chessboard, field, row + (IsWhite ? 1 : -1), field.Column, false, out piecesMove)
        && piecesMove.TargetFieldIsEmpty();

    private bool IsPromotingField(Field targetField)
        => targetField.Row == (IsWhite ? 7 : 0);

}
