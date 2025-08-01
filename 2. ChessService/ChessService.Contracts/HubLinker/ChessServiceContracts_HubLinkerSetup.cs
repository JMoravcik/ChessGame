using ChessGame.ChessService.Contracts.IHub;
using ChessGame.Common.Bases;
using ChessGame.Common.DI_Tools;
using Microsoft.Extensions.DependencyInjection;

namespace ChessGame.ChessService.Contracts.HubLinker;

public class ChessServiceContracts_HubLinkerSetup : ConfigurableSetup
{
    public ChessServiceContracts_HubLinkerSetup(EnvironmentInfo environment) : base(environment, "HubLinkerSetup")
    {
    }

    public override void AddDependencies(IServiceCollection serviceCollection, params object[] args)
    {
        Register<ChessEvents>(serviceCollection);
        Register<IChessGameHub, ChessGameHubLinker>(serviceCollection);
    }
}
