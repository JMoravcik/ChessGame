using ChessGame.Common.Entities.Identity;
using ChessGame.NativeApp.Components.LoginViews.Enums;
using ChessGame.NativeApp.Managers;
using Microsoft.AspNetCore.Components;

namespace ChessGame.NativeApp.Components.LoginViews;

public partial class RegistrationView : ComponentBase
{
    [Inject] public required AuthorizationManager AuthorizationManager { get; set; }
    [Parameter] public required EventCallback<LoginViewEnum> ViewChanged { get; set; }

    UserProfile _userProfile { get; set; } = new UserProfile
    {
        NickName = string.Empty,
        Email = string.Empty
    };

    private string Password { get; set; } = string.Empty;
    private string RepeatPassword { get; set; } = string.Empty;

    private void OnChangeView() => ViewChanged.InvokeAsync(LoginViewEnum.LoginView);
    private Task OnRegisterAsync() => AuthorizationManager.RegisterUserProfileAsync(_userProfile, Password, RepeatPassword);
}
