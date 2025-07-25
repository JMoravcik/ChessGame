using ChessGame.AccessService.Contracts.IServices;
using ChessGame.AccessService.Contracts.Requests.Authentication;
using ChessGame.AccessService.Contracts.Responses.Authentication;
using ChessGame.Common.Bases;
using ChessGame.Common.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ChessGame.AccessService.Contracts.Linkers;

public class AuthenticationLinker : Linker, IAuthenticationService
{
    public AuthenticationLinker(LinkerContext linkerContext, IConfiguration configuration, ILogger<AuthenticationLinker> logger) : base(linkerContext, configuration, logger, "AccessService", AccessServiceRoutes.Authentication.BaseServiceUrl)
    {
    }

    public Task<Response<AuthenticatedResponse>> BasicLoginAsync(BasicAuthenticationRequest basicAuthenticationRequest)
    {
        return PostAsync<AuthenticatedResponse, BasicAuthenticationRequest>(AccessServiceRoutes.Authentication.BasicLogin, basicAuthenticationRequest);
    }

    public Task<Response<bool>> IsDeviceRegisteredAsync()
    {
        return GetAsync<bool>(AccessServiceRoutes.Authentication.IsDeviceRegistered);
    }

    public Task<Response<AuthenticatedResponse>> IsLoggedInAsync()
    {
        return GetAsync<AuthenticatedResponse>(AccessServiceRoutes.Authentication.IsLoggedIn);
    }

    public Task<Response<bool>> LogoutAsync()
    {
        return GetAsync<bool>(AccessServiceRoutes.Authentication.BasicLogout);
    }

    public Task<Response<bool>> RegisterDeviceAsync()
    {
        return GetAsync<bool>(AccessServiceRoutes.Authentication.RegisterDevice);
    }

    public Task<Response> RegisterUserProfileAsync(RegisterUserProfileRequest registerUserProfileRequest)
    {
        return PostAsync(AccessServiceRoutes.Authentication.RegisterUserProfile, registerUserProfileRequest);
    }
}
