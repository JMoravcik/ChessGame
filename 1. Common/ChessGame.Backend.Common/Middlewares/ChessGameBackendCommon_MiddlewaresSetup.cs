using ChessGame.Common.DI_Tools;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace ChessGame.Backend.Common.Middlewares;

public class ChessGameBackendCommon_MiddlewaresSetup : Setup
{
    public ChessGameBackendCommon_MiddlewaresSetup(EnvironmentInfo environment) : base(environment)
    {
    }

    public override void Startup(IHost host, params object[] args)
    {
        if (host is not WebApplication webApplicationHost)
        {
            return;
        }
        webApplicationHost.UseMiddleware<DeviceRecognizeMiddleware>();
        webApplicationHost.UseMiddleware<UserAuthorizeMiddleware>();
    }
}
