using ChessGame.ChessService.ChessLogic;
using ChessService.ChessLogic.Tests.TestContexts;
using Microsoft.Extensions.Configuration;

namespace ChessService.ChessLogic.Tests.Hooks;

[Binding]
public static class ChessManagerFeatureHooks
{
    [BeforeFeature()]
    public static void BeforeFeature(FeatureContext featureContext)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "MaxGameCount", "100" }
            })
            .Build();

        var chessManager = new ChessManager(configuration);
        var playerId = Guid.NewGuid();
        chessManager.CreateNewGame(playerId, null);

        var testContext = new ChessManagerTestContext
        {
            PlayerId = playerId,
            GameId = chessManager.CreateNewGame(playerId, null),
            ChessManager = chessManager
        };
        featureContext.FeatureContainer.RegisterInstanceAs(testContext);
    }
}
