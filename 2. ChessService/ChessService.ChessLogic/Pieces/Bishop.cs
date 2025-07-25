using ChessGame.ChessService.ChessLogic.ChessboardComponents;
using ChessGame.ChessService.ChessLogic.ChessboardComponents.Moves;

namespace ChessGame.ChessService.ChessLogic.Pieces;

public class Bishop : Piece
{
    protected override int PieceId => 4;
    public Bishop(bool isWhite) : base(isWhite)
    {
    }

    public override IEnumerable<Move> GetPossibleAttacks(Chessboard chessboard, bool forThreatening) 
        => GetBishopMoves(chessboard, forThreatening);

    public override IEnumerable<Move> GetPossibleMoves(Chessboard chessboard) 
        => GetBishopMoves(chessboard, false);

    #region Private methods

    private IEnumerable<Move> GetBishopMoves(Chessboard chessboard, bool forThreatening)
    {
        var field = chessboard.GetPiecePosition(this);

        foreach (var move in DirectionalMove(chessboard, field, 1, 1, forThreatening))
            yield return move;

        foreach (var move in DirectionalMove(chessboard, field, -1, 1, forThreatening))
            yield return move;

        foreach (var move in DirectionalMove(chessboard, field, 1, -1, forThreatening))
            yield return move;

        foreach (var move in DirectionalMove(chessboard, field, -1, -1, forThreatening))
            yield return move;
    }

    #endregion
}
