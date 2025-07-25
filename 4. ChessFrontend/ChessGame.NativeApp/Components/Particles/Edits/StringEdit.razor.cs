using Microsoft.AspNetCore.Components;

namespace ChessGame.NativeApp.Components.Particles.Edits;

public enum StringEditTypeEnum
{
    text,
    password,
    email,
    url,
    search,
    tel
}

public partial class StringEdit
{
    [Parameter] public StringEditTypeEnum Type { get; set; } = StringEditTypeEnum.text;
    public void OnChange(ChangeEventArgs changeEventArgs)
    {
        Value = changeEventArgs.Value?.ToString() ?? string.Empty;
        ValueChanged.InvokeAsync(Value);
    }

}
