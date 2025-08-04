using Microsoft.AspNetCore.Components;

namespace ChessGame.NativeApp.Components.Layout;

public interface IModalManager
{
    public void Show<TComponent>(string title, Dictionary<string, object> parameters);
    public void Hide();
}

public partial class ModalManager : ComponentBase, IModalManager 
{
    [Parameter] public required RenderFragment ChildContent { get; set; }


    private string? Title { get; set; } = string.Empty;
    private Type? _modalType = null;
    private Dictionary<string, object> _modalParameters = new();
    public void Show<TComponent>(string title, Dictionary<string, object> parameters)
    {
        Title = title;
        _modalType = typeof(TComponent);
        _modalParameters = parameters;
        InvokeAsync(StateHasChanged);
    }

    public void Hide()
    {
        _modalType = null;
        _modalParameters.Clear();
    }


}
