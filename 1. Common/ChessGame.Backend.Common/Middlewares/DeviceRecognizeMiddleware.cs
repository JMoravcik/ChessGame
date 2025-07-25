using ChessGame.Backend.Common.Attributes;
using ChessGame.Backend.Common.Extensions;
using ChessGame.Backend.Common.Scopeds;
using ChessGame.Backend.Common.Scopeds.Database;
using ChessGame.Common.Extensions;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;

namespace ChessGame.Backend.Common.Middlewares;

public class DeviceRecognizeMiddleware
{
    private readonly RequestDelegate _next;

    public DeviceRecognizeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext, RequestContext requestContext, IAuthenticationRepository repository)
    {
        if (await SetRequestContextDeviceAsync(httpContext, requestContext, repository))
        {
            await _next(httpContext);
            return;
        }

        if (!httpContext.HasAttribute<DeviceRecognizeAttribute>(out _))
        {
            await _next(httpContext);
            return;
        }

        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
        await httpContext.Response.WriteAsync(ChessGameBackendCommonRes.DeviceRecognizeMiddleware_DeviceWasNotRecognized);
    }

    private async Task<bool> SetRequestContextDeviceAsync(HttpContext httpContext, RequestContext requestContext, IAuthenticationRepository repository)
    {
        var deviceIdString = httpContext.Request.Headers.GetDeviceToken();
        if (!Guid.TryParse(deviceIdString, out var deviceId))
            return false;

        var device = await repository.GetDeviceByIdAsync(deviceId);
        if (device is null)
            return false;

        requestContext.SetDevice(device);
        return true;
    }
}
