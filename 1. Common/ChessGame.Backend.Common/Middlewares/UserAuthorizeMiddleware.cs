using ChessGame.Backend.Common.Attributes;
using ChessGame.Backend.Common.Extensions;
using ChessGame.Backend.Common.Scopeds;
using ChessGame.Backend.Common.Scopeds.Database;
using ChessGame.Common.Extensions;
using Microsoft.AspNetCore.Http;

namespace ChessGame.Backend.Common.Middlewares;

public class UserAuthorizeMiddleware
{
    private readonly RequestDelegate _next;

    public UserAuthorizeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext, RequestContext requestContext, IAuthenticationRepository repository)
    {
        if (await SetRequestContextUserProfileAsync(httpContext, requestContext, repository))
        {
            await _next(httpContext);
            return;
        }

        if (!httpContext.HasAttribute<UserAuthorizeAttribute>(out _))
        {
            await _next(httpContext);
            return;
        }

        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
        await httpContext.Response.WriteAsync(ChessGameBackendCommonRes.DeviceRecognizeMiddleware_DeviceWasNotRecognized);
    }

    private async Task<bool> SetRequestContextUserProfileAsync(HttpContext httpContext, RequestContext requestContext, IAuthenticationRepository repository)
    {
        var deviceIdString = httpContext.Request.Headers.GetDeviceToken();
        if (deviceIdString is null)
            return false;

        var authToken = httpContext.Request.Headers.GetAuthToken(deviceIdString);
        if (authToken is null)
            return false;

        if (!Guid.TryParse(deviceIdString, out var deviceId))
            return false;

        var userProfile = await repository.GetProfileByDeviceAndTokenAsync(deviceId, authToken);
        if (userProfile is null)
            return false;

        requestContext.SetUserProfile(userProfile);
        return true;
    }
}
