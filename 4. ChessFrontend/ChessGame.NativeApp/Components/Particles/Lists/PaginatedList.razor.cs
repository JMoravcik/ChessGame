using ChessGame.Common.Data;
using ChessGame.NativeApp.Components.FlowControl;
using Microsoft.AspNetCore.Components;

namespace ChessGame.NativeApp.Components.Particles.Lists;

public partial class PaginatedList<TItem> : ComponentBase, IInitializable
    where TItem : class
{
    [Parameter] public required Func<int, Task<ListResponse<TItem>>> GetItemsAsync { get; set; }
    [Parameter] public required RenderFragment<TItem> ItemTemplate { get; set; }

    private int _currentPage = 1;

    private int TotalPages => CurrentPageResponse.PageCount;

    private async Task PreviousPageAsync()
    {
        if (_currentPage > 1)
            await GetPageAsync(_currentPage - 1);
    }

    private async Task GetPageAsync(int page)
    {
        CurrentPageResponse = await GetItemsAsync(_currentPage);
        _currentPage = CurrentPageResponse.CurrentPage;
    }

    private async Task NextPageAsync()
    {
        if (_currentPage < TotalPages) 
            await GetPageAsync(_currentPage + 1);
    }

    ListResponse<TItem> CurrentPageResponse { get; set; } = new ListResponse<TItem>() { Data = new List<TItem>() };
    public async Task InitializeAsync()
    {
        await GetPageAsync(1);
    }
}
