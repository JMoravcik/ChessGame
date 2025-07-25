using ChessGame.Common.Extensions;
using Microsoft.AspNetCore.Components;

namespace ChessGame.NativeApp.Components.Chessboard;

public partial class ChessboardView : ComponentBase
{
    [Parameter] public required int[][] Minimap { get; set; }
    [Parameter] public required EventCallback<string> MoveSelected { get; set; }
    [Parameter] public required List<string> LegalMoves { get; set; }


    IEnumerable<(int Row, int Column, int PieceId, string notation)> GetFields()
    {
        for (int row = 0; row < Minimap.Length; row++)
        {
            for (int column = 0; column < Minimap[row].Length; column++)
            {
                yield return (row, column, Minimap[row][column], (row, column).ToChessNotation());
            }
        }
    }

    private List<string> _selectableLegalMoves = new();
    private string _selectedField = string.Empty;
    private void OnFieldClick(string selectedField)
    {
        if (_selectedField == selectedField)
        {
            _selectedField = string.Empty;
            _selectableLegalMoves.Clear();
            return;
        }
        _selectedField = selectedField;

        _selectableLegalMoves = LegalMoves.Where(IsSelectedFieldsLegalMove).ToList();
        if (_selectableLegalMoves.Count == 0)
        {
            _selectedField = string.Empty;
            return;
        }
    }

    private bool IsSelectedFieldsLegalMove(string legalMove) => GetSourceField(legalMove).StartsWith(_selectedField);

    private string GetTargetField(string legalMove)
    {
        var targetField = legalMove.Split(' ')[2];
        return targetField;
    }

    private string GetSourceField(string legalMove)
    {
        var targetField = legalMove.Split(' ')[1];
        return targetField;
    }

    private bool IsSelected(string field) => _selectedField == field;

    private bool IsSelectable(string field) => _selectableLegalMoves.Any(selectableField => GetTargetField(selectableField).StartsWith(field));
}
