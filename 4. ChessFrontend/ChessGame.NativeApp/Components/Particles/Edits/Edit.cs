using Microsoft.AspNetCore.Components;

namespace ChessGame.NativeApp.Components.Particles.Edits;

public class Edit : ComponentBase
{

}

public class Edit<TValue> : Edit
{
    [Parameter] public TValue Value { get; set; } = default!;
    [Parameter] public EventCallback<TValue> ValueChanged { get; set; }
    [Parameter] public string? Label { get; set; }
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public string? Id { get; set; }
    [Parameter] public string? Placeholder { get; set; }
}
