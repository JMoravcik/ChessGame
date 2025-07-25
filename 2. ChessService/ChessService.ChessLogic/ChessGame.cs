using ChessGame.ChessService.ChessLogic.ChessboardComponents;
using ChessGame.ChessService.ChessLogic.ChessboardComponents.Moves;
using ChessGame.ChessService.ChessLogic.Pieces; 
using ChessGame.ChessService.Contracts.MoveResults;
using ChessService.Contracts.MoveResults;
using System.Diagnostics.CodeAnalysis;

namespace ChessGame.ChessService.ChessLogic;

public partial class ChessGame
{
    private readonly Guid _whitePlayerId;
    private Guid _blackPlayerId;

    private readonly Chessboard _chessboard;
    private readonly LegalMoves _legalMoves;
    private Guid _playerOnMove;
    private Guid _playerInWaiting => _playerOnMove == _whitePlayerId ? _blackPlayerId : _whitePlayerId;
    public ChessGame(Guid whitePlayerId, Guid? blackPlayerId = null)
    {
        _whitePlayerId = whitePlayerId;
        _blackPlayerId = blackPlayerId ?? Guid.Empty;
        _chessboard = new Chessboard();
        _chessboard.SetStartupPositions();
        _legalMoves = new LegalMoves();
        _legalMoves.RefreshLegalMoves(_chessboard);
        _playerOnMove = _whitePlayerId; // White starts first
    }

    public List<string> GetMoves(Guid playerId)
    {
        var result = new List<string>();
        if (playerId == _whitePlayerId)
        {
            result.AddRange(_legalMoves.WhiteLegalMoves.Keys);
        }
        if (playerId == _blackPlayerId)
        {
            result.AddRange(_legalMoves.BlackLegalMoves.Keys);
        }
        return result;
    }

    public void JoinGame(Guid blackPlayerId)
    {
        if (_blackPlayerId != Guid.Empty)
            throw new InvalidOperationException("Game already has a black player.");

        _blackPlayerId = blackPlayerId;
    }

    public MoveResult MovePlayerPiece(Guid playerId, string move)
    {
        if (!MoveMeetsBasicValidations(playerId, move, out var invalidMove, out var legalMove))
            return invalidMove;

        _chessboard.MakeMove(legalMove);
        _legalMoves.RefreshLegalMoves(_chessboard);

        _playerOnMove = _playerOnMove == _whitePlayerId ? _blackPlayerId : _whitePlayerId;

        if (GameIsNotFinished())
            return new CorrectMove();

        bool isWhite = _playerOnMove == _whitePlayerId;
        Guid? winner = !_legalMoves.HasLegalMoves(isWhite) && _chessboard.IsKingInCheck(isWhite) ? _playerInWaiting : null;

        return new FinishMove(winner);
    }

    private bool GameIsNotFinished()
    {
        if (_legalMoves.HasLegalMoves(_playerOnMove == _whitePlayerId) == false)
            return false;

        if (!PlayersDidThreeFoldRepetition())
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

        if (_playerOnMove != playerId)
        {
            invalidMove = new InvalidMove(ChessServiceChessLogicRes.InvalidMove_NotPlayersTurn);
            return false;
        }

        if (!_legalMoves.IsVerifiedLegalMove(move, _playerOnMove == _whitePlayerId, out legalMove))
        {
            invalidMove = new InvalidMove(ChessServiceChessLogicRes.InvalidMove_NotFoundInVerifiedLegalMoves);
            return false;
        }

        return true;
    }

    public int[][] GetMinimap()
        => _chessboard.GenerateMinimap();
}
