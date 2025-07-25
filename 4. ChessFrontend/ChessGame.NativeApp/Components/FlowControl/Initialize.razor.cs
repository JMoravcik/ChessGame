using Microsoft.AspNetCore.Components;

namespace ChessGame.NativeApp.Components.FlowControl;

public interface IInitializable
{
    Task InitializeAsync();
}

public partial class Initialize
{
    [Parameter] public required IInitializable Initializable { get; set; }
    [Parameter] public required RenderFragment ChildContent { get; set; }
    [Parameter] public EventCallback<bool> AfterRender { get; set; }

    bool _initialized = false;
    bool _afterRenderAlreadyCalled = false;
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (_initialized)
        {
            _ = AfterRender.InvokeAsync(_afterRenderAlreadyCalled);
            _afterRenderAlreadyCalled = true;
        }

        if (_initialized == false)
        {
            if (Initializable != null)
                await Initializable.InitializeAsync();
            _initialized = true;
            _ = InvokeAsync(StateHasChanged);
        }
    }
}