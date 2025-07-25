using ChessGame.NativeApp.Managers;
using Microsoft.AspNetCore.Components;


namespace ChessGame.NativeApp.Components.FlowControl;

public partial class Authorization : ComponentBase, IDisposable
{
    [Inject] public required AuthorizationManager AuthorizationManager { get; set; }
    [Parameter] public required RenderFragment Authorized { get; set; }
    [Parameter] public required RenderFragment NotAuthorized { get; set; }

    protected override void OnInitialized()
    {
        AuthorizationManager.StateChanged += StateHasChanged;
        base.OnInitialized();
    }

    public void Dispose()
    {
        AuthorizationManager.StateChanged -= StateHasChanged;
    }
}
