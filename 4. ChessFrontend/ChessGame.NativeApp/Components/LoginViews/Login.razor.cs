using ChessGame.NativeApp.Components.LoginViews.Enums;
using Microsoft.AspNetCore.Components;

namespace ChessGame.NativeApp.Components.LoginViews;

public partial class Login : ComponentBase
{
    private string Email { get; set; } = string.Empty;
    private LoginViewEnum CurrentView { get; set; } = LoginViewEnum.LoginView;

    private void OnChangeView(LoginViewEnum view)
    {
        CurrentView = view;
    }

}
