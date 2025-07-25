using Microsoft.AspNetCore.Components;

namespace ChessGame.NativeApp.Components.Particles.Buttons;

public partial class Button : ComponentBase
{
    [Parameter] public required string Title { get; set; }
    [Parameter] public required EventCallback Clicked { get; set; }

    bool _clicked = false;
    private async Task OnClickAsync()
    {
        if (_clicked)
            return;
        _clicked = true;
        await Clicked.InvokeAsync();
        _clicked = false;
        _ = InvokeAsync(StateHasChanged);
    }
}
