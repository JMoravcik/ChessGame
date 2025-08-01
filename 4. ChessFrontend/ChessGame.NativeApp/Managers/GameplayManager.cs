using ChessGame.AccessService.Contracts.IServices;
using ChessGame.ChessService.Contracts.HubLinker;
using ChessGame.ChessService.Contracts.HubMessages;
using ChessGame.ChessService.Contracts.IHub;
using ChessGame.ChessService.Contracts.IServices;
using ChessGame.ChessService.Contracts.Responses;
using ChessGame.Common.Bases;
using ChessGame.Common.Data;
using ChessGame.Common.Entities.Game;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace ChessGame.NativeApp.Managers;

public class GameplayManager
{
    private readonly NavigationManager _navigationManager;
    private readonly AuthorizationManager _authorizationManager;
    private readonly IChessAccessService _chessAccessService;
    private readonly IChessService _chessService;
    private readonly IChessGameHub _chessGameHub;
    private readonly LinkerContext _linkerContext;

    public GameRecord? GameRecord { get; private set; }
    public GetGameStateResponse? GameState { get; private set; }

    public GameplayManager(NavigationManager navigationManager, AuthorizationManager authorizationManager, IChessAccessService chessAccessService, IChessService chessService, IChessGameHub chessGameHub, LinkerContext linkerContext)
    {
        _navigationManager = navigationManager;
        _authorizationManager = authorizationManager;
        _chessAccessService = chessAccessService;
        _chessService = chessService;
        _chessGameHub = chessGameHub;
        _linkerContext = linkerContext;
    }

    [MemberNotNullWhen(true, nameof(GameRecord))]
    public bool IsPlaying() => GameRecord != null;
    public async Task PlayAloneAsync()
    {
        var response = await _chessAccessService.CreateGameAsync(new() { BlackPlayerId = _authorizationManager.UserProfile!.Id });
        if (!response.ResponseIsValid())
            return;

        await SetGameRecordAsync(response.Data.GameRecord);
    }

    public async Task GetGameStateAsync()
    {
        if (GameRecord == null)
        {
            Response<GameRecord> gameRecordResponse = await _chessAccessService.GetCurrentGameRecordAsync();
            GameRecord = gameRecordResponse.Data;
            if (GameRecord == null)
                throw new Exception("No game record found for the current user.");
            else
                await SetGameRecordAsync(GameRecord);
        }

        var response = await _chessService.GetGameStateAsync(GameRecord.Id);
        if (!response.ResponseIsValid())
            return;

        if (response.Data.IsFinished)
        {
            await UnsetGameRecordAsync();
            _navigationManager.NavigateTo("/");
            return;
        }
        GameState = response.Data;
    }

    public async Task<List<GameRecord>> GetGamesAsync()
    {
        var response = await _chessAccessService.GetGamesAsync();
        if (!response.ResponseIsValid())
            return new List<GameRecord>();

        if (response.Data.Playing != null)
        {
            await SetGameRecordAsync(response.Data.Playing);
        }

        return response.Data.GameRecords;
    }

    private async Task SetGameRecordAsync(GameRecord gameRecord)
    {
        GameRecord = gameRecord;
        if (_chessService is Linker linker)
        {
            linker.SetServerBaseUrl(gameRecord.ServerUrl);
        }
        if (_chessGameHub is ChessGameHubLinker hubLinker)
        {
            await hubLinker.ConnectAsync(gameRecord.ServerUrl);
            await AuthenticateToHubAsync();
        }

        _navigationManager.NavigateTo("/gameplay");
    }

    private async Task UnsetGameRecordAsync()
    {
        GameRecord = null;
        GameState = null;
        if (_chessGameHub is ChessGameHubLinker hubLinker)
        {
            await hubLinker.DisconnectAsync();
        }

    }

    private async Task AuthenticateToHubAsync()
    {
        await _chessGameHub.LoginInfoAsync(new AuthenticationInfo()
        {
            DeviceToken = await _linkerContext.GetDeviceTokenAsync(),
            UserToken = (await _linkerContext.GetAuthTokenAsync())!
        });
    }
}
