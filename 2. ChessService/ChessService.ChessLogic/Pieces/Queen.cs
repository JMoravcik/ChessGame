using ChessGame.ChessService.ChessLogic.ChessboardComponents;
using ChessGame.ChessService.ChessLogic.ChessboardComponents.Moves;

namespace ChessGame.ChessService.ChessLogic.Pieces;

public class Queen : Piece
{
    protected override int PieceId => 5;
    public Queen(bool isWhite) : base(isWhite)
    {
    }

    public override IEnumerable<Move> GetPossibleAttacks(Chessboard chessboard, bool forThreatening)
    {
        return GetQueenMoves(chessboard, forThreatening);
    }

    public override IEnumerable<Move> GetPossibleMoves(Chessboard chessboard)
    {
        return GetQueenMoves(chessboard, false);
    }

    private IEnumerable<Move> GetQueenMoves(Chessboard chessboard, bool forThreatening)
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
        foreach (var move in DirectionalMove(chessboard, field, 1, 1, forThreatening))
            yield return move;
        foreach (var move in DirectionalMove(chessboard, field, -1, 1, forThreatening))
            yield return move;
        foreach (var move in DirectionalMove(chessboard, field, 1, -1, forThreatening))
            yield return move;
        foreach (var move in DirectionalMove(chessboard, field, -1, -1, forThreatening))
            yield return move;
    }
}
