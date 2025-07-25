using ChessGame.ChessService.ChessLogic.Pieces;
using ChessGame.Common.Exceptions;

namespace ChessGame.ChessService.ChessLogic.ChessboardComponents.Moves;

public class PromotionMove : Move
{
    public int PromotionPieceId { get; set; }
    public PromotionMove(Move move, int promotionPieceId = 0) 
         : base(move)
    {
        PromotionPieceId = promotionPieceId;
    }

    public PromotionMove(string sourceField, string targetField, bool forThreatening, int promotionPieceId = 0) 
         : base(sourceField, targetField, forThreatening)
    {
        PromotionPieceId = promotionPieceId;
    }

    public override void ExecuteMove(Chessboard chessboard)
    {
        base.ExecuteMove(chessboard);
        if (PromotionPieceId == 0)
            throw new ChessCoreException("Promotion piece id was not set!");

        var newPiece = Piece.CreatePiece(PromotionPieceId);
        var targetField = chessboard[TargetField];

        if (targetField.Piece is not Pawn pawn)
            throw new ChessCoreException("On promoting field must be pawn!");

        chessboard.RemovePiece(targetField);
        chessboard.PlacePiece(targetField, newPiece);
    }

    public override void RevertMove(Chessboard chessboard)
    {
        if (PromotionPieceId == 0)
            throw new ChessCoreException("Promotion piece id was not set!");

        var targetField = chessboard[TargetField];
        bool isWhite = PromotionPieceId < 7;

        chessboard.RemovePiece(targetField, false);
        chessboard.PlacePiece(targetField, new Pawn(isWhite));

        base.RevertMove(chessboard);
    }

    public override string GetMoveRecord()
    {
        return base.GetMoveRecord() + (PromotionPieceId != 0 ? $" {PromotionPieceId}" : string.Empty);
    }
}
