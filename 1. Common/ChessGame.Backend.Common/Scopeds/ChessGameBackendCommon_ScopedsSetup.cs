using ChessGame.Common.DI_Tools;
using Microsoft.Extensions.DependencyInjection;

namespace ChessGame.Backend.Common.Scopeds;

public class ChessGameBackendCommon_ScopedsSetup : Setup
{
    public ChessGameBackendCommon_ScopedsSetup(EnvironmentInfo environment) : base(environment)
    {
    }

    public override void AddDependencies(IServiceCollection serviceCollection, params object[] args)
    {
        serviceCollection.AddScoped<RequestContext>();
    }
}
