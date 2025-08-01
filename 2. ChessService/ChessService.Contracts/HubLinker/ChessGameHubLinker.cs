using ChessGame.ChessService.Contracts.HubMessages;
using ChessGame.ChessService.Contracts.IHub;
using ChessGame.ChessService.Contracts.MoveResults;
using ChessGame.Common.Extensions;
using ChessService.Contracts.MoveResults;
using Microsoft.AspNetCore.SignalR.Client;

namespace ChessGame.ChessService.Contracts.HubLinker;

public class ChessGameHubLinker : IChessGameHub, IAsyncDisposable
{
    private readonly ChessEvents _chessEvents;
    private HubConnection? _hubConnection = null;
    public ChessGameHubLinker(ChessEvents chessEvents)
    {
        _chessEvents = chessEvents;
    }

    public async Task ConnectAsync(string baseUrl)
    {
        baseUrl = baseUrl.StandartizeUrl();
        _hubConnection = new HubConnectionBuilder()
            .WithUrl($"{baseUrl}/{IChessGameHub.HubUrl}")
            .Build();
        _hubConnection.On<InvalidMove>(IChessGameHub.InvalidMoveChannel, _chessEvents.PublishMoveResult);
        _hubConnection.On<CorrectMove>(IChessGameHub.CorrectMoveChannel, _chessEvents.PublishMoveResult);
        _hubConnection.On<FinishMove>(IChessGameHub.FinishMoveChannel, _chessEvents.PublishMoveResult);
        await _hubConnection.StartAsync();
    }

    public async Task DisconnectAsync()
    {
        if (_hubConnection == null)
            return;
        await _hubConnection.StopAsync();
    }

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection != null) 
            await _hubConnection.DisposeAsync();

    }

    public async Task JoinGameAsync(Guid gameId)
    {
        if (_hubConnection == null)
            return;

        await _hubConnection.SendAsync(nameof(JoinGameAsync), gameId);
    }

    public async Task LoginInfoAsync(AuthenticationInfo authenticationInfo)
    {
        if (_hubConnection == null)
            return;

        await _hubConnection.SendAsync(nameof(LoginInfoAsync), authenticationInfo);
    }

    public async Task MovePieceAsync(string move)
    {
        if (_hubConnection == null)
            return;

        await _hubConnection.SendAsync(nameof(MovePieceAsync), move);
    }
}
