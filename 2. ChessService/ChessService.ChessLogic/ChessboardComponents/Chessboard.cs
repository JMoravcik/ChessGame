using ChessGame.ChessService.ChessLogic.ChessboardComponents.Moves;
using ChessGame.ChessService.ChessLogic.Pieces;
using ChessGame.Common.Exceptions;
using ChessGame.Common.Extensions;
using System.ComponentModel;

namespace ChessGame.ChessService.ChessLogic.ChessboardComponents;

public partial class Chessboard : IDisposable
{
    private readonly Field[,] _fields;
    internal Dictionary<Piece, Field> WhitePieceFields { get; } = new();
    internal Dictionary<Piece, Field> BlackPieceFields { get; } = new();

    internal List<Piece> CapturedPieces { get; } = new();
    internal List<string> History { get; } = new();

    public Chessboard()
    {
        _fields = new Field[8, 8];
        InitializeFields();
    }

    public int MoveCount => History.Count;

    public int[][] GenerateMinimap()
    {
        int[][] minimap = new int[8][];
        for (int row = 0; row < 8; row++)
        {
            minimap[row] = new int[8];
            for (int column = 0; column < 8; column++)
            {
                var field = _fields[row, column];
                minimap[row][column] = field.Piece?.GetPieceId() ?? 0;
            }
        }
        return minimap;
    }

    public Field this[int row, int column] {
        get {
            return _fields[row, column];
        }
    }

    public Field this[string fieldCoordinate]
    {
        get
        {
            var (row, column) = Field.ParseFieldCoordinate(fieldCoordinate);
            return _fields[row, column];
        }
    }

    public (Field Field, Piece? Piece) ParseFieldData(string sourceFieldData)
    {
        var splittedSourceField = sourceFieldData.Split('-');
        var field = this[splittedSourceField[0]];
        if (splittedSourceField.Length == 1)
            return (field, null);

        int pieceId = int.Parse(splittedSourceField[1]);
        int thisPieceId = field.Piece?.GetPieceId() ?? 0;
        if (thisPieceId != pieceId)
            throw new ChessCoreException($"chessboard data '{sourceFieldData}' are not correct!");

        return (field, field.Piece);
    }

    public void MakeMove(Move move)
    {
        move.ExecuteMove(this);
        ComputeAllThreats();
        History.Add(move.GetMoveRecord());
    }

    public void RevertMove()
    {
        if (History.Count == 0) 
            return;

        string lastMoveRecord = History.Last();
        var lastMove = Move.Parse(lastMoveRecord);
        lastMove.RevertMove(this);
        ComputeAllThreats();
        History.RemoveAt(History.Count - 1);
    }

    public Field GetPiecePosition(Piece piece)
        => piece.IsWhite ? WhitePieceFields[piece] : BlackPieceFields[piece];

    public void SetStartupPositions()
    {
        SetStartupPositionsFor(false);
        SetStartupPositionsFor(true);
        ComputeAllThreats();
    }

    internal Chessboard CreateClone()
    {
        var clone = new Chessboard();

        foreach (var piece in WhitePieceFields)
        {
            var clonePieceField = clone[piece.Value.Row, piece.Value.Column];
            clone.PlacePiece(clonePieceField, piece.Key.Clone());
        }

        foreach (var piece in BlackPieceFields)
        {
            var clonePieceField = clone[piece.Value.Row, piece.Value.Column];
            clone.PlacePiece(clonePieceField, piece.Key.Clone());
        }

        clone.History.AddRange(History);
        clone.ComputeAllThreats();
        return clone;
    }

    internal bool IsValidField(int newRow, int newCol)
        => newCol.IsInRange(0, 7) && newRow.IsInRange(0, 7);

    internal void MovePiece(Piece piece, Field targetField)
    {
        var piecesField = GetPiecePosition(piece);
        piecesField.Piece = null;
        targetField.Piece = piece;
        var pieces = piece.IsWhite ? WhitePieceFields : BlackPieceFields;
        pieces[piece] = targetField;
    }

    internal void PlacePiece(Field field, Piece piece)
    {
        field.Piece = piece;

        if (piece.IsWhite)
            WhitePieceFields.Add(piece, field);
        else
            BlackPieceFields.Add(piece, field);
    }

    internal void RemovePiece(Field targetField, bool asCaptured = true)
    {
        if (targetField.Piece is Piece enemyPiece)
        {
            var playersPieces = enemyPiece.IsWhite ? WhitePieceFields : BlackPieceFields;
            playersPieces.Remove(enemyPiece);
            if (asCaptured)
                CapturedPieces.Add(enemyPiece);

            targetField.Piece = null;
        }
    }

    internal Piece ReturnPiece(Field targetField, int pieceId)
    {
        if (CapturedPieces.Count == 0)
            throw new ChessCoreException("No pieces to return!");

        var pieceToReturn = CapturedPieces.LastOrDefault(p => p.GetPieceId() == pieceId);
        if (pieceToReturn == null)
            throw new ChessCoreException($"No piece with id '{pieceId}' found to return!");

        CapturedPieces.Remove(pieceToReturn);
        PlacePiece(targetField, pieceToReturn);
        return pieceToReturn;
    }

    public void Dispose()
    {
    }

    #region Private methods
    private void InitializeFields()
    {
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                _fields[row, col] = new Field(row, col);
            }
        }
    }

    private void SetStartupPositionsFor(bool isWhite)
    {

        int row = isWhite ? 1 : 6;
        for (int i = 0; i < 8; i++)
            PlacePiece(_fields[row, i], new Pawn(isWhite));

        row = isWhite ? 0 : 7;
        PlacePiece(_fields[row, 0], new Rook(isWhite));
        PlacePiece(_fields[row, 1], new Knight(isWhite));
        PlacePiece(_fields[row, 2], new Bishop(isWhite));
        PlacePiece(_fields[row, 3], new Queen(isWhite));
        PlacePiece(_fields[row, 4], new King(isWhite));
        PlacePiece(_fields[row, 5], new Bishop(isWhite));
        PlacePiece(_fields[row, 6], new Knight(isWhite));
        PlacePiece(_fields[row, 7], new Rook(isWhite));
    }

    private void ComputeAllThreats()
    {
        foreach (var field in _fields)
            field.ThreatenedBy.Clear();

        ComputeAllThreatsFor(true);
        ComputeAllThreatsFor(false);
    }

    private void ComputeAllThreatsFor(bool isWhite)
    {
        foreach (var field in _fields)
            field.ThreatenedBy.Clear();
        var pieces = isWhite ? WhitePieceFields.Keys : BlackPieceFields.Keys;

        foreach (var piece in pieces)
        {
            foreach (var move in piece.GetPossibleAttacks(this, true))
            {
                if (move == null)
                    continue;
                var targetField = this[move.TargetField];
                if (!targetField.ThreatenedBy.Contains(!isWhite))
                    targetField.ThreatenedBy.Add(!isWhite);
            }
        }
    }

    public bool IsKingInCheck(bool isWhite)
    {
        var king = isWhite ? WhitePieceFields.Keys.OfType<King>().FirstOrDefault() : BlackPieceFields.Keys.OfType<King>().FirstOrDefault();
        if (king == null)
            throw new InvalidOperationException("King not found on the chessboard.");

        var kingField = GetPiecePosition(king);

        return kingField.IsThreatenedFor(!isWhite);
    }

    #endregion
}
