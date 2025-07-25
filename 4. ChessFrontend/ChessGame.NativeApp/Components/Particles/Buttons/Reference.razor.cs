using Microsoft.AspNetCore.Components;

namespace ChessGame.NativeApp.Components.Particles.Buttons;

public partial class Reference : ComponentBase
{
    [Parameter] public required string Title { get; set; }
    [Parameter] public required EventCallback Clicked { get; set; }

    private void OnClick()
    {
        Clicked.InvokeAsync();
    }
}
