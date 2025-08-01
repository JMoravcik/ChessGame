using ChessGame.Backend.Common.Scopeds.Database;
using ChessGame.ChessService.ChessLogic;
using ChessGame.ChessService.Contracts.HubMessages;
using ChessGame.ChessService.Contracts.IHub;
using ChessGame.ChessService.Contracts.MoveResults;
using ChessService.Contracts.MoveResults;
using Microsoft.AspNetCore.SignalR;

namespace ChessGame.ChessService.Api.Hubs;

public class ChessGameHub : Hub, IChessGameHub
{
    const string UserProfileIdKey = "user_id";
    const string UserGameIdKey = "game_id";

    private readonly IChessManager _chessManager;
    private readonly IAuthenticationRepository _repository;
    private readonly ILogger<ChessGameHub> _logger;

    public ChessGameHub(IChessManager chessManager, IAuthenticationRepository repository, ILogger<ChessGameHub> logger)
    {
        _chessManager = chessManager;
        _repository = repository;
        _logger = logger;
    }

    public async Task LoginInfoAsync(AuthenticationInfo authenticationInfo)
    {
        var userProfile = await _repository.GetProfileByDeviceAndTokenAsync(Guid.Parse(authenticationInfo.DeviceToken), authenticationInfo.UserToken);
        if (userProfile == null)
        {
            _logger.LogWarning("Invalid try for authentication to chess game!");
            return;
        }

        this.Context.Items.Add(UserProfileIdKey, userProfile.Id);
        if (_chessManager.IsInGame(userProfile.Id, out var gameId))
        {
            LinkPlayerGame(gameId);
        }
    }

    public Task JoinGameAsync(Guid gameId)
    {
        var userProfileIdRecord = Context.Items[UserProfileIdKey];
        if (userProfileIdRecord is not Guid userProfileId)
        {
            _logger.LogWarning("Unauthenticated connected user tried to join game!");
            return Task.CompletedTask;
        }

        if (_chessManager.JoinGame(gameId, userProfileId))
        {
            LinkPlayerGame(gameId);
        }
        return Task.CompletedTask;
    }

    private void LinkPlayerGame(Guid gameId)
    {
        Context.Items.Add(UserGameIdKey, gameId);
        this.Groups.AddToGroupAsync(this.Context.ConnectionId, gameId.ToString());
    }

    public Task MovePieceAsync(string move)
    {
        var userProfileIdRecord = Context.Items[UserProfileIdKey];
        var gameIdRecord = Context.Items[UserGameIdKey];

        if (userProfileIdRecord is not Guid userProfileId)
        {
            _logger.LogWarning("Unauthenticated connected user tried to join game!");
            return Task.CompletedTask;
        }

        if (gameIdRecord is not Guid gameId)
        {
            _logger.LogWarning("User which is not playing any game, tried to move piece!");
            return Task.CompletedTask;
        }

        var result = _chessManager.MakeMove(userProfileId, gameId, move);
        switch (result)
        {
            case InvalidMove:
                Clients.Caller.SendAsync(IChessGameHub.InvalidMoveChannel, result);
                break;
            case CorrectMove:
                Clients.Group(gameId.ToString()).SendAsync(IChessGameHub.CorrectMoveChannel, result);
                break;
            case FinishMove:
                Clients.Group(gameId.ToString()).SendAsync(IChessGameHub.FinishMoveChannel, result);
                break;
        }

        return Task.CompletedTask;
    }
}
