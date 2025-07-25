using ChessGame.AccessService.Contracts.IServices;
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

    public GameRecord? GameRecord { get; private set; }
    public GetGameStateResponse? GameState { get; private set; }

    public GameplayManager(NavigationManager navigationManager, AuthorizationManager authorizationManager, IChessAccessService chessAccessService, IChessService chessService)
    {
        _navigationManager = navigationManager;
        _authorizationManager = authorizationManager;
        _chessAccessService = chessAccessService;
        _chessService = chessService;
    }

    [MemberNotNullWhen(true, nameof(GameRecord))]
    public bool IsPlaying() => GameRecord != null;
    public async Task PlayAloneAsync()
    {
        var response = await _chessAccessService.CreateGameAsync(new() { BlackPlayerId = _authorizationManager.UserProfile!.Id });
        if (!response.ResponseIsValid())
            return;

        SetGameRecord(response.Data.GameRecord);
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
                SetGameRecord(GameRecord);
        }

        var response = await _chessService.GetGameStateAsync(GameRecord.Id);
        if (!response.ResponseIsValid())
            return;

        if (response.Data.IsFinished)
        {
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
            SetGameRecord(response.Data.Playing);
        }

        return response.Data.GameRecords;
    }

    private void SetGameRecord(GameRecord gameRecord)
    {
        GameRecord = gameRecord;
        if (_chessService is Linker linker)
        {
            linker.SetServerBaseUrl(gameRecord.ServerUrl);
        }

        _navigationManager.NavigateTo("/gameplay");
    }
}
