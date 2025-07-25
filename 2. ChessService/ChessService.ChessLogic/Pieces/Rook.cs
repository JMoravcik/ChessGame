using ChessGame.ChessService.ChessLogic.ChessboardComponents;
using ChessGame.ChessService.ChessLogic.ChessboardComponents.Moves;

namespace ChessGame.ChessService.ChessLogic.Pieces;

public class Rook : Piece
{
    protected override int PieceId => 2;
    
    public Rook(bool isWhite) : base(isWhite)
    {
    }

    public override IEnumerable<Move> GetPossibleAttacks(Chessboard chessboard, bool forThreatening)
    {
        return GetRookMoves(chessboard, forThreatening);
    }

    public override IEnumerable<Move> GetPossibleMoves(Chessboard chessboard)
    {
        return GetRookMoves(chessboard, false);
    }

    private IEnumerable<Move> GetRookMoves(Chessboard chessboard, bool forThreatening)
    {
        var field = chessboard.GetPiecePosition(this);

        foreach (var move in DirectionalMove(chessboard, field, 1, 0, forThreatening))
            yield return move;

        foreach (var move in DirectionalMove(chessboard, field, -1, 0, forThreatening))
            yield return move;

        foreach (var move in DirectionalMove(chessboard, field, 0, -1, forThreatening))
            yield return move;

        foreach (var move in DirectionalMove(chessboard, field, 0, 1, forThreatening))
            yield return move;
    }


}
