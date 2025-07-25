
using ChessService.ChessLogic.Tests.TestContexts;

namespace ChessService.ChessLogic.Tests.StepDefinitions.ChessManagers;

[Binding]
public class ChessManagersWhenSteps
{
    private readonly ChessManagerTestContext _testContext;

    public ChessManagersWhenSteps(ChessManagerTestContext testContext)
    {
        _testContext = testContext;
    }

    [When("I create a new chess game")]
    public void WhenICreateANewChessGame()
    {
        // Act: Create a new chess game
        _testContext.GameId = _testContext.ChessManager.CreateNewGame(_testContext.PlayerId, _testContext.PlayerId);
    }

}
