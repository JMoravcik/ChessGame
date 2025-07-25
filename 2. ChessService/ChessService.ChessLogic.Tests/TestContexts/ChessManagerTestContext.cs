using ChessGame.ChessService.ChessLogic;

namespace ChessService.ChessLogic.Tests.TestContexts;

public class ChessManagerTestContext
{
    public required Guid PlayerId { get; set; }
    public Guid GameId { get; set; } 
    public required ChessManager ChessManager { get; set; }
}
