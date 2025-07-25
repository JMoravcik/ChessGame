using ChessGame.ChessService.ChessLogic;
using Microsoft.AspNetCore.SignalR;

namespace ChessGame.ChessService.Api.Hubs;

public class ChessGameHub : Hub
{
    private readonly IChessManager _chessManager;

    public ChessGameHub(IChessManager chessManager)
    {
        _chessManager = chessManager;
    }
}
