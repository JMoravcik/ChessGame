using ChessGame.AccessService.Contracts.IServices;
using ChessGame.AccessService.Contracts.Responses.ChessAccess;
using ChessGame.ChessService.Contracts.Requests;
using ChessGame.ChessService.Contracts.Responses;
using ChessGame.Common.Bases;
using ChessGame.Common.Data;
using ChessGame.Common.Entities.Game;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ChessGame.AccessService.Contracts.Linkers;

public class ChessAccessLinker : Linker, IChessAccessService
{
    public ChessAccessLinker(LinkerContext linkerContext, IConfiguration configuration, ILogger<ChessAccessLinker> logger)
         : base(linkerContext, configuration, logger, "AccessService", AccessServiceRoutes.GameAccess.BaseServiceUrl)
    {

    }

    public Task<Response<CreateGameResponse>> CreateGameAsync(CreateGameRequest request)
    {
        return PostAsync<CreateGameResponse, CreateGameRequest>(AccessServiceRoutes.GameAccess.CreateGame, request);
    }

    public Task<Response<GameRecord>> GetCurrentGameRecordAsync()
    {
        return GetAsync<GameRecord>(AccessServiceRoutes.GameAccess.GetCurrentGameRecord);
    }

    public Task<Response<GetGamesResponse>> GetGamesAsync()
    {
        return GetAsync<GetGamesResponse>(AccessServiceRoutes.GameAccess.GetGames);
    }
}
