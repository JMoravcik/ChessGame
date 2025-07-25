using ChessGame.AccessService.Contracts;
using ChessGame.AccessService.Contracts.IServices;
using ChessGame.AccessService.Contracts.Requests.Authentication;
using ChessGame.AccessService.Contracts.Responses.Authentication;
using ChessGame.Backend.Common.Attributes;
using ChessGame.Backend.Common.Bases;
using ChessGame.Backend.Common.Scopeds;
using ChessGame.Backend.Common.Scopeds.Database;
using ChessGame.Common.Data;
using ChessGame.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ChessGame.AccessService.Api.Controllers;

public class AuthenticationController : BaseController, IAuthenticationService
{
    private readonly RequestContext _requestContext;
    private readonly IAuthenticationRepository _authenticationRepository;

    public AuthenticationController(RequestContext requestContext, IAuthenticationRepository authenticationRepository)
    {
        _requestContext = requestContext;
        _authenticationRepository = authenticationRepository;
    }

    [DeviceRecognize]
    [HttpPost(AccessServiceRoutes.Authentication.BasicLogin)]
    public async Task<Response<AuthenticatedResponse>> BasicLoginAsync([FromBody] BasicAuthenticationRequest basicAuthenticationRequest)
    {
        var loginResult = await _authenticationRepository.BasicLoginAsync(_requestContext.Device.Id, basicAuthenticationRequest.PasswordHash);

        if (loginResult == null)
        {
            return new(AccessServiceApiRes.BasicLoginAsync_LoginFailed);
        }

        return new(new AuthenticatedResponse()
        {
            Token = loginResult.Token,
            UserProfile = loginResult.UserProfile
        });
    }

    [HttpGet(AccessServiceRoutes.Authentication.IsDeviceRegistered)]
    public Task<Response<bool>> IsDeviceRegisteredAsync()
    {
        return Task.FromResult(new Response<bool>(_requestContext.IsDeviceRegistered));
    }

    [DeviceRecognize]
    [HttpGet(AccessServiceRoutes.Authentication.IsLoggedIn)]
    public Task<Response<AuthenticatedResponse>> IsLoggedInAsync()
    {
        if (!_requestContext.IsLoggedIn)
            return Task.FromResult(new Response<AuthenticatedResponse>());

        return Task.FromResult(new Response<AuthenticatedResponse>(new AuthenticatedResponse()
        {
            Token = _requestContext.Device.Token!,
            UserProfile = _requestContext.UserProfile
        }));
    }

    [DeviceRecognize, UserAuthorize]
    [HttpGet(AccessServiceRoutes.Authentication.BasicLogout)]
    public async Task<Response<bool>> LogoutAsync()
    {
        if (_requestContext.Device.Token is null)
            return new(false);

        var logoutResult = await _authenticationRepository.LogoutAsync(_requestContext.Device.Id, _requestContext.Device.Token);
        return new(logoutResult);
    }

    [HttpGet(AccessServiceRoutes.Authentication.RegisterDevice)]
    public async Task<Response<bool>> RegisterDeviceAsync()
    {
        var deviceToken = this.HttpContext.Request.Headers.GetDeviceToken();
        if (!Guid.TryParse(deviceToken, out var deviceId))
        {
            return new(AccessServiceApiRes.RegisterDeviceAsync_BadDeviceToken);
        }
        var registerResult = await _authenticationRepository.RegisterDeviceAsync(deviceId);
        return new(registerResult);
    }

    [DeviceRecognize]
    [HttpPost(AccessServiceRoutes.Authentication.RegisterUserProfile)]
    public async Task<Response> RegisterUserProfileAsync([FromBody] RegisterUserProfileRequest registerUserProfileRequest)
    {
        registerUserProfileRequest.BasicAuth.UserProfileId = registerUserProfileRequest.UserProfile.Id;
        var registerResult = await _authenticationRepository.RegisterUserAsync(registerUserProfileRequest.UserProfile, registerUserProfileRequest.BasicAuth);

        return new()
        {
            StatusCode = string.IsNullOrEmpty(registerResult) ? System.Net.HttpStatusCode.Created : System.Net.HttpStatusCode.BadRequest,
            ErrorMessage = registerResult
        };
    }
}
