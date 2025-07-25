using ChessGame.Common.Entities.Identity;

namespace ChessGame.Backend.Common.Scopeds;

public class RequestContext
{
    private Device? _device = null;
    public Device Device
    {
        get => _device ?? throw new InvalidOperationException("Device is not set. Use SetDevice method to set it.");
        private set => _device = value;
    }

    private UserProfile? _userProfile = null;
    public UserProfile UserProfile
    {
        get => _userProfile ?? throw new InvalidOperationException("UserProfile is not set. Use SetUserProfile method to set it.");
        private set => _userProfile = value;
    }

    internal void SetDevice(Device device)
    {
        Device = device;
    }

    public bool IsDeviceRegistered => _device != null;

    public bool IsLoggedIn => _userProfile != null;

    internal void SetUserProfile(UserProfile userProfile)
    {
        UserProfile = userProfile;
    }
}
