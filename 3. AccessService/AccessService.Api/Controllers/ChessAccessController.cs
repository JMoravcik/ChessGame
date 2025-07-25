using ChessGame.AccessService.Contracts;
using ChessGame.AccessService.Contracts.IServices;
using ChessGame.AccessService.Contracts.Responses.ChessAccess;
using ChessGame.Backend.Common.Attributes;
using ChessGame.Backend.Common.Bases;
using ChessGame.Backend.Common.Scopeds;
using ChessGame.Backend.Common.Scopeds.Database;
using ChessGame.ChessService.Contracts.IServices;
using ChessGame.ChessService.Contracts.Requests;
using ChessGame.ChessService.Contracts.Responses;
using ChessGame.Common.Data;
using ChessGame.Common.Entities.Game;
using Microsoft.AspNetCore.Mvc;

namespace ChessGame.AccessService.Api.Controllers;

[DeviceRecognize]
[UserAuthorize]
public class ChessAccessController : BaseController, IChessAccessService
{
    private readonly RequestContext _requestContext;
    private readonly IChessService _chessService;
    private readonly IGameRepository _gameRepository;

    public ChessAccessController(RequestContext requestContext, IChessService chessService, IGameRepository gameRepository)
    {
        _requestContext = requestContext;
        _chessService = chessService;
        _gameRepository = gameRepository;
    }

    [HttpPost(AccessServiceRoutes.GameAccess.CreateGame)]
    public Task<Response<CreateGameResponse>> CreateGameAsync(CreateGameRequest request)
    {
        return _chessService.CreateGameAsync(request);
    }

    [HttpGet(AccessServiceRoutes.GameAccess.GetCurrentGameRecord)]
    public async Task<Response<GameRecord>> GetCurrentGameRecordAsync()
    {
        var result = await _gameRepository.GetPlayersCurrentGameAsync(_requestContext.UserProfile.Id);
        return new(result);
    }

    [HttpGet(AccessServiceRoutes.GameAccess.GetGames)]
    public async Task<Response<GetGamesResponse>> GetGamesAsync()
    {
        var gameRecords = await _gameRepository.GetGamesAsync();
        return new(new GetGamesResponse()
        { 
            GameRecords = gameRecords,
            Playing = await _gameRepository.GetPlayersCurrentGameAsync(_requestContext.UserProfile.Id)
        });
    }
}
