
using ChessGame.Common.Entities.Identity;

namespace ChessGame.Backend.Common.Scopeds.Database.DbResults;

public class LoginDbResult
{
    public required UserProfile UserProfile { get; set; }
    public required string Token { get; set; }
}
