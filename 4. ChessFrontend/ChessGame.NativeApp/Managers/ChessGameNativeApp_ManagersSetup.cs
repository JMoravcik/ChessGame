
using ChessGame.Common.DI_Tools;

namespace ChessGame.NativeApp.Managers;

public class ChessFrontendNativeApp_ManagersSetup : Setup
{
    public ChessFrontendNativeApp_ManagersSetup(EnvironmentInfo environment) : base(environment)
    {
    }

    public override void AddDependencies(IServiceCollection serviceCollection, params object[] args)
    {
        serviceCollection.AddScoped<AuthorizationManager>();
        serviceCollection.AddScoped<JavascriptManager>();
        serviceCollection.AddScoped<GameplayManager>();
    }
}
