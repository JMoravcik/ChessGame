using ChessGame.ChessService.Contracts.IServices;
using ChessGame.ChessService.Contracts.Requests;
using ChessGame.ChessService.Contracts.Responses;
using ChessGame.Common.Bases;
using ChessGame.Common.Data;
using ChessGame.Common.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ChessGame.ChessService.Contracts.Linkers;

public class ChessLinker : Linker, IChessService
{
    public ChessLinker(LinkerContext linkerContext, IConfiguration configuration, ILogger<ChessLinker> logger) : base(linkerContext, configuration, logger, "ChessService", ChessServiceRoutes.Chess.BaseServiceUrl)
    {
    }

    public Task<Response<CreateGameResponse>> CreateGameAsync(CreateGameRequest request)
        => PostAsync<CreateGameResponse, CreateGameRequest>(ChessServiceRoutes.Chess.CreateGame, request);

    public Task<Response<GetGameStateResponse>> GetGameStateAsync(Guid gameId)
        => GetAsync<GetGameStateResponse>(ChessServiceRoutes.Chess.GetMinimap.FillRoute(gameId));
}
