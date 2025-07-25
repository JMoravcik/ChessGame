using System.ComponentModel.DataAnnotations.Schema;

namespace ChessGame.Common.Entities.Identity;

public class BasicAuth : Entity
{
    public required string PasswordHash { get; set; }

    [ForeignKey(nameof(UserProfile))]
    public required Guid UserProfileId { get; set; }
}
