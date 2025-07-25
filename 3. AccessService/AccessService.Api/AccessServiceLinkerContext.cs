using ChessGame.Backend.Common.Scopeds;
using ChessGame.Common.Bases;

namespace ChessGame.AccessService.Api;

public class AccessServiceLinkerContext : LinkerContext
{
    private readonly RequestContext _requestContext;

    public AccessServiceLinkerContext(RequestContext requestContext)
    {
        _requestContext = requestContext;
    }


    public override Task<string?> GetAuthTokenAsync()
    {
        return Task.FromResult(_requestContext.Device.Token);
    }

    public override Task<string> GetDeviceTokenAsync()
    {
        return Task.FromResult(_requestContext.Device.Id.ToString());
    }
}
