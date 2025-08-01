using ChessGame.ChessService.Contracts.IServices;
using ChessGame.Common.Bases;
using ChessGame.Common.DI_Tools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ChessGame.ChessService.Contracts.Linkers;

public class ChessServiceContracts_LinkersSetup : ConfigurableSetup
{
    public ChessServiceContracts_LinkersSetup(EnvironmentInfo environment) : base(environment, "ChessServiceLinkers")
    {
    }

    public override void AddDependencies(IServiceCollection serviceCollection, params object[] args)
    {
        if (IsSameApiLinkersAreReferencingTo("ChessService.Api"))
            return;

        Register<IChessService, ChessLinker>(serviceCollection);
    }
}
