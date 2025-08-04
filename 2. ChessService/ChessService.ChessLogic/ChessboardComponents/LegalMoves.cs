using ChessGame.ChessService.ChessLogic.ChessboardComponents.Moves;
using ChessGame.ChessService.ChessLogic.Pieces;
using System.Diagnostics.CodeAnalysis;

namespace ChessGame.ChessService.ChessLogic.ChessboardComponents;

public class LegalMoves
{
    public Dictionary<string, Move> BlackLegalMoves { get; private set; } = new Dictionary<string, Move>();

    public Dictionary<string, Move> WhiteLegalMoves { get; private set; } = new Dictionary<string, Move>();

    public void RefreshLegalMoves(Chessboard chessboard)
    {
        using var cloneChessboard = chessboard.CreateClone();
        WhiteLegalMoves = CheckPlayersPieces(cloneChessboard, cloneChessboard.WhitePieceFields, true);
        BlackLegalMoves = CheckPlayersPieces(cloneChessboard, cloneChessboard.BlackPieceFields, false);
    }

    public bool IsVerifiedLegalMove(string move, bool isWhite, [NotNullWhen(true)] out Move? legalMove)
    {
        var keyString = move.StartsWith(nameof(PromotionMove)) ? RemoveLastPromotionArgument(move) : move;
        var legalMoves = isWhite ? WhiteLegalMoves : BlackLegalMoves;
        bool result = legalMoves.TryGetValue(keyString, out legalMove);
        if (legalMove is PromotionMove promotion)
            promotion.PromotionPieceId = int.Parse(move.Split(' ').Last());

        return result;
    }

    public bool HasLegalMoves(bool isWhite)
        => isWhite ? WhiteLegalMoves.Any() : BlackLegalMoves.Any();


    private Dictionary<string, Move> CheckPlayersPieces(Chessboard chessboard, Dictionary<Piece, Field> pieceFields, bool isWhite)
    {
        var result = new Dictionary<string, Move>();
        foreach (var piece in pieceFields.Keys.ToList())
        {
            foreach (var possibleMove in piece.GetPossibleMoves(chessboard))
            {
                IfSetPromotion(possibleMove, (isWhite ? 0 : 6) + 5);
                chessboard.MakeMove(possibleMove);
                bool isLegal = !chessboard.IsKingInCheck(isWhite);
                chessboard.RevertMove();
                IfSetPromotion(possibleMove, 0);
                if (isLegal)
                    result.Add(possibleMove.GetMoveRecord(), possibleMove);
            }
        }

        return result;
    }

    private void IfSetPromotion(Move move, int promotionPieceId)
    {
        if (move is PromotionMove promotion)
            promotion.PromotionPieceId = promotionPieceId;
    }

    private string RemoveLastPromotionArgument(string move)
        => string.Join(' ', move.Split(' ').SkipLast(1));
}
