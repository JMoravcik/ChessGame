using ChessGame.AccessService.Contracts.IServices;
using ChessGame.AccessService.Contracts.Responses.Authentication;
using ChessGame.Common.Entities.Identity;
using ChessGame.Common.Extensions;

namespace ChessGame.NativeApp.Managers;

public class AuthorizationManager
{
    public event Action? StateChanged;

    private readonly JavascriptManager _javascriptManager;
    private readonly IAuthenticationService _authenticationService;
    private readonly NativeAppLinkerContext _nativeAppLinkerContext;


    public UserProfile? UserProfile { get; private set; } = null;

    public bool IsAuthenticated => UserProfile != null;

    public AuthorizationManager(JavascriptManager javascriptManager, IAuthenticationService authenticationService, NativeAppLinkerContext nativeAppLinkerContext)
    {
        _javascriptManager = javascriptManager;
        _authenticationService = authenticationService;
        _nativeAppLinkerContext = nativeAppLinkerContext;
    }

    public bool Initialized { get; private set; } = false;

    public async Task InitializeAsync()
    {
        var isRegisteredResponse = await _authenticationService.IsDeviceRegisteredAsync();
        if (!isRegisteredResponse.Data)
        {
            var registerResponse = await _authenticationService.RegisterDeviceAsync();
            Initialized = true;
            StateChanged?.Invoke();
            return;
        }

        var isLoggedInResponse = await _authenticationService.IsLoggedInAsync();
        Initialized = true;
        if (isLoggedInResponse.ResponseIsValid())
        {
            Login(isLoggedInResponse.Data);
        }
        else
        {
            StateChanged?.Invoke();
        }

    }

    public async Task<bool> BasicLoginAsync(string email, string password)
    {
        var loginResponse = await _authenticationService.BasicLoginAsync(new AccessService.Contracts.Requests.Authentication.BasicAuthenticationRequest()
        {
            PasswordHash = $"{email}:{password.ToSHA256()}".ToSHA256()
        });
        if (loginResponse.ResponseIsValid())
        {
            Login(loginResponse.Data);
            return true;
        }
        else
        {
            await _javascriptManager.AlertAsync(loginResponse.ErrorMessage);
        }
        return false;
    }

    public async Task<bool> RegisterUserProfileAsync(UserProfile userProfile, string password, string repeatPassword)
    {
        if (password != repeatPassword)
        {
            await _javascriptManager.AlertAsync(ChessGameNativeAppRes.RegistrationView_PasswordDoNotMatch);
            return false;
        }

        var passwordHash = $"{userProfile.Email}:{password.ToSHA256()}".ToSHA256();
        var registerResponse = await _authenticationService.RegisterUserProfileAsync(new AccessService.Contracts.Requests.Authentication.RegisterUserProfileRequest()
        {
            UserProfile = userProfile,
            BasicAuth = new BasicAuth()
            {
                PasswordHash = passwordHash,
                UserProfileId = userProfile.Id
            }
        });

        if (registerResponse.ResponseIsValid())
        {
            var loginResponse = await _authenticationService.BasicLoginAsync(new AccessService.Contracts.Requests.Authentication.BasicAuthenticationRequest()
            {
                PasswordHash = passwordHash
            });
            if (loginResponse.ResponseIsValid())
            {
                Login(loginResponse.Data);
                return true;
            }
        }

        return false;
    }

    public async Task LogoutAsync()
    {
        var response = await _authenticationService.LogoutAsync();
        if (response.ResponseIsValid())
            Logout();
    }


    private void Login(AuthenticatedResponse response)
    {
        UserProfile = response.UserProfile;
        _nativeAppLinkerContext.SetAuthToken(response.Token);

        StateChanged?.Invoke();
    }

    private void Logout()
    {
        UserProfile = null;
        _nativeAppLinkerContext.SetAuthToken(null);
        StateChanged?.Invoke();
    }
}
