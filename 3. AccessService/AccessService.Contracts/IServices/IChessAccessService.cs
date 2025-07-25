using ChessGame.AccessService.Contracts.Responses.ChessAccess;
using ChessGame.ChessService.Contracts.Requests;
using ChessGame.ChessService.Contracts.Responses;
using ChessGame.Common.Data;
using ChessGame.Common.Entities.Game;

namespace ChessGame.AccessService.Contracts.IServices;

public interface IChessAccessService
{
    Task<Response<CreateGameResponse>> CreateGameAsync(CreateGameRequest request);
    Task<Response<GameRecord>> GetCurrentGameRecordAsync();
    Task<Response<GetGamesResponse>> GetGamesAsync();
}
