using ChessGame.ChessService.ChessLogic.ChessboardComponents;
using ChessGame.ChessService.ChessLogic.ChessboardComponents.Moves;
using ChessGame.ChessService.ChessLogic.Pieces; 
using ChessGame.ChessService.Contracts.MoveResults;
using ChessService.Contracts.MoveResults;
using System.Diagnostics.CodeAnalysis;

namespace ChessGame.ChessService.ChessLogic;

public partial class ChessGame
{
    public Guid WhitePlayerId { get; private set; }

    public Guid BlackPlayerId { get; private set; }

    private readonly Chessboard _chessboard;
    private readonly LegalMoves _legalMoves;

    public bool WhiteOnMove { get; private set; } = true;
    public bool WhiteInWaiting => !WhiteOnMove;

    public ChessGame(Guid whitePlayerId, Guid? blackPlayerId = null)
    {
        WhitePlayerId = whitePlayerId;
        BlackPlayerId = blackPlayerId ?? Guid.Empty;
        _chessboard = new Chessboard();
        _chessboard.SetStartupPositions();
        _legalMoves = new LegalMoves();
        _legalMoves.RefreshLegalMoves(_chessboard);
    }

    public List<string> GetMoves(Guid playerId)
    {
        var result = new List<string>();
        if (playerId == WhitePlayerId)
        {
            result.AddRange(_legalMoves.WhiteLegalMoves.Keys);
        }
        if (playerId == BlackPlayerId)
        {
            result.AddRange(_legalMoves.BlackLegalMoves.Keys);
        }
        return result;
    }

    public bool JoinGame(Guid blackPlayerId)
    {
        if (BlackPlayerId != Guid.Empty)
            return false;

        BlackPlayerId = blackPlayerId;
        return true;
    }

    public MoveResult MovePlayerPiece(Guid playerId, string move)
    {
        if (!MoveMeetsBasicValidations(playerId, move, out var invalidMove, out var legalMove))
            return invalidMove;

        _chessboard.MakeMove(legalMove);
        _legalMoves.RefreshLegalMoves(_chessboard);

        WhiteOnMove = !WhiteOnMove;

        if (GameIsNotFinished())
            return new CorrectMove();

        Guid? winner = !_legalMoves.HasLegalMoves(WhiteOnMove) && _chessboard.IsKingInCheck(WhiteOnMove) ? playerId : null;

        return new FinishMove(winner);
    }

    private bool GameIsNotFinished()
    {
        if (_legalMoves.HasLegalMoves(WhiteOnMove) == false)
            return false;

        if (PlayersDidThreeFoldRepetition())
            return false;

        return true;
    }

    private bool PlayersDidThreeFoldRepetition()
    {
        if (_chessboard.History.Count < 10)
            return false;

        int lastMove = _chessboard.History.Count - 1;

        return CheckPlayersRepetition(lastMove) && CheckPlayersRepetition(lastMove - 1);
    }

    private bool CheckPlayersRepetition(int lastMove)
        => _chessboard.History[lastMove] == _chessboard.History[lastMove - 4]
        && _chessboard.History[lastMove] == _chessboard.History[lastMove - 8]
        && _chessboard.History[lastMove - 2] == _chessboard.History[lastMove - 6];


    private bool MoveMeetsBasicValidations(
        Guid playerId, 
        string move,
        [NotNullWhen(false)] out InvalidMove? invalidMove,
        [NotNullWhen(true)] out Move? legalMove
        )
    {
        invalidMove = null;
        legalMove = null;

        if (!IsPlayerOnMove(playerId))
        {
            invalidMove = new InvalidMove(ChessServiceChessLogicRes.InvalidMove_NotPlayersTurn);
            return false;
        }

        if (!_legalMoves.IsVerifiedLegalMove(move, WhiteOnMove, out legalMove))
        {
            invalidMove = new InvalidMove(ChessServiceChessLogicRes.InvalidMove_NotFoundInVerifiedLegalMoves);
            return false;
        }

        return true;
    }

    private bool IsPlayerOnMove(Guid playerId) => WhiteOnMove ? playerId == WhitePlayerId : playerId == BlackPlayerId;

    public int[][] GetMinimap()
        => _chessboard.GenerateMinimap();
}
