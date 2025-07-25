using System.ComponentModel.DataAnnotations.Schema;

namespace ChessGame.Common.Entities.Identity;

public class Device : Entity
{
    public string? Token { get; set; }

    [ForeignKey(nameof(UserProfile))]
    public Guid? LoggedUserProfileId { get; set; }

}
