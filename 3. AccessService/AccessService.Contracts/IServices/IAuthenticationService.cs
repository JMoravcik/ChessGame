using ChessGame.AccessService.Contracts.Responses.Authentication;
using ChessGame.AccessService.Contracts.Requests.Authentication;
using ChessGame.Common.Data;

namespace ChessGame.AccessService.Contracts.IServices;

public interface IAuthenticationService
{
    Task<Response<bool>> IsDeviceRegisteredAsync();
    Task<Response<bool>> RegisterDeviceAsync();
    Task<Response> RegisterUserProfileAsync(RegisterUserProfileRequest registerUserProfileRequest); 
    Task<Response<AuthenticatedResponse>> BasicLoginAsync(BasicAuthenticationRequest basicAuthenticationRequest);
    Task<Response<bool>> LogoutAsync();
    Task<Response<AuthenticatedResponse>> IsLoggedInAsync();
}
