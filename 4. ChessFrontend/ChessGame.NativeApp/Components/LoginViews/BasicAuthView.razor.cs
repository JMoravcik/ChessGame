using ChessGame.AccessService.Contracts.Requests.Authentication;
using ChessGame.NativeApp.Components.LoginViews.Enums;
using ChessGame.NativeApp.Managers;
using Microsoft.AspNetCore.Components;

namespace ChessGame.NativeApp.Components.LoginViews;

public partial class BasicAuthView : ComponentBase
{
    [Inject] public required AuthorizationManager AuthorizationManager { get; set; }
    [Parameter] public required EventCallback<LoginViewEnum> ViewChanged { get; set; }

    private void OnChangeView() => ViewChanged.InvokeAsync(LoginViewEnum.RegistrationView);

    private string Email { get; set; } = string.Empty;
    private string Password { get; set; } = string.Empty;

    private Task OnLoginAsync() => AuthorizationManager.BasicLoginAsync(Email, Password);
}
