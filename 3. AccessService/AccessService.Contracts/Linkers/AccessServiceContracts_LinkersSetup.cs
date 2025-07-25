using ChessGame.AccessService.Contracts.IServices;
using ChessGame.Common.Bases;
using ChessGame.Common.DI_Tools;
using Microsoft.Extensions.DependencyInjection;

namespace ChessGame.AccessService.Contracts.Linkers;

public class AccessServiceContracts_LinkersSetup : LinkerSetup
{
    public AccessServiceContracts_LinkersSetup(EnvironmentInfo environment) : base(environment, "AccessServiceLinkers")
    {
    }

    public override void AddDependencies(IServiceCollection serviceCollection, params object[] args)
    {
        if (IsSameApiLinkersAreReferencingTo("AccessService.Api"))
            return;

        RegisterLinker<IAuthenticationService, AuthenticationLinker>(serviceCollection);
        RegisterLinker<IChessAccessService, ChessAccessLinker>(serviceCollection);
    }
}
