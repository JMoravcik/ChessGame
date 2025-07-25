using ChessGame.ChessService.Contracts;
using ChessGame.Common.DI_Tools;
using Microsoft.Extensions.DependencyInjection;

namespace ChessGame.ChessService.ChessLogic;

public class ChessServiceChessLogic_Setup : Setup
{
    public ChessServiceChessLogic_Setup(EnvironmentInfo environment) : base(environment)
    {
    }

    public override void AddDependencies(IServiceCollection serviceCollection, params object[] args)
    {
        serviceCollection.AddSingleton<IChessManager, ChessManager>();
    }
}
