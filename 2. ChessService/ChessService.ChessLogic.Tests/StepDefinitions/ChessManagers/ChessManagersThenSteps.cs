using ChessService.ChessLogic.Tests.TestContexts;
using NUnit.Framework;

namespace ChessService.ChessLogic.Tests.StepDefinitions.ChessManagers;

[Binding]
public class ChessManagersThenSteps
{
    private readonly ChessManagerTestContext _testContext;

    public ChessManagersThenSteps(ChessManagerTestContext testContext)
    {
        _testContext = testContext;
    }

    [Then("GameId should not be empty")]
    public void WhenICreateANewChessGame()
    {
        Assert.AreNotEqual(_testContext.GameId, Guid.Empty, "GameId should not be empty after creating a new chess game.");
    }
}
