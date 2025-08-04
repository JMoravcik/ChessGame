using ChessGame.Backend.Common.Attributes;
using ChessGame.Backend.Common.Bases;
using ChessGame.Backend.Common.Scopeds;
using ChessGame.Backend.Common.Scopeds.Database;
using ChessGame.ChessService.ChessLogic;
using ChessGame.ChessService.Contracts;
using ChessGame.ChessService.Contracts.IServices;
using ChessGame.ChessService.Contracts.Requests;
using ChessGame.ChessService.Contracts.Responses;
using ChessGame.Common.Data;
using ChessGame.Common.Exceptions;
using ChessGame.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ChessGame.ChessService.Api.Controllers;

[DeviceRecognize]
[UserAuthorize]
public class ChessController : BaseController, IChessService
{
    private readonly IChessManager _chessManager;
    private readonly IGameRepository _gameRepository;
    private readonly RequestContext _requestContext;

    public ChessController(IChessManager chessManager, IGameRepository gameRepository, RequestContext requestContext)
    {
        _chessManager = chessManager;
        _gameRepository = gameRepository;
        _requestContext = requestContext;
    }

    [HttpPost(ChessServiceRoutes.Chess.CreateGame)]
    public async Task<Response<CreateGameResponse>> CreateGameAsync([FromBody] CreateGameRequest request)
    {
        if (_chessManager.NoMoreSpace)
            throw new HttpException(ChessServiceApiRes.StartGame_NoMoreSpace, System.Net.HttpStatusCode.BadRequest);

        var gameId = _chessManager.CreateNewGame(_requestContext.UserProfile.Id, request.BlackPlayerId);
        var gameRecord = await _gameRepository.CreateGameRecordAsync(gameId, _requestContext.UserProfile.Id, request.BlackPlayerId);

        return new(new CreateGameResponse
        {
            GameRecord = gameRecord
        });
    }

    [HttpGet(ChessServiceRoutes.Chess.GetMinimap)]
    public async Task<Response<GetGameStateResponse>> GetGameStateAsync([FromRoute] Guid gameId)
    {
        var minimap = _chessManager.GetMinimap(gameId);

        if (minimap == null)
        {
            await _gameRepository.UpdateGameRecordAsync(gameId, true);
            return new()
            {
                Data = new GetGameStateResponse
                {
                    IsFinished = true,
                    Minimap = null,
                    LegalMoves = new List<string>(),
                    WhiteOnTurn = false
                }
            };
        }

        var legalMoves = _chessManager.GetMoves(gameId, _requestContext.UserProfile.Id);

        return new (new GetGameStateResponse
        {
            IsFinished = false,
            Minimap = minimap,
            LegalMoves = legalMoves,
            WhiteOnTurn = _chessManager.IsWhiteOnTurn(gameId)
        });
    }

}
