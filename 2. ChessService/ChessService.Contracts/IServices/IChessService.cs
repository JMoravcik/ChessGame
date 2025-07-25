using ChessGame.ChessService.Contracts.Requests;
using ChessGame.ChessService.Contracts.Responses;
using ChessGame.Common.Data;

namespace ChessGame.ChessService.Contracts.IServices;

public interface IChessService
{
    Task<Response<CreateGameResponse>> CreateGameAsync(CreateGameRequest request);
    Task<Response<GetGameStateResponse>> GetGameStateAsync(Guid gameId);
}
