using ChessGame.ChessService.ChessLogic.Pieces;
using ChessGame.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.ChessService.ChessLogic.ChessboardComponents.Moves;

public class Move
{
    public string SourceField { get; init; }
    public string TargetField { get; init; }
    public bool ForThreatening { get; init; }

    public bool TargetFieldIsNotEmpty() => TargetField.Contains('-');

    public bool TargetFieldIsEmpty() => !TargetFieldIsNotEmpty();

    public Move(string sourceField, string targetField, bool forThreatening)
    {
        SourceField = sourceField;
        TargetField = targetField;
        ForThreatening = forThreatening;
    }

    public Move(Field sourceField, Field targetField, bool forThreatening)
    {
        SourceField = GetFieldString(sourceField);
        TargetField = GetFieldString(targetField);
        ForThreatening = forThreatening;
    }

    public Move(Move move)
    {
        SourceField = move.SourceField;
        TargetField = move.TargetField;
        ForThreatening = move.ForThreatening;
    }

    private string GetFieldString(Field field)
        => field.Piece == null ? field.GetFieldCoordinate() : $"{field.GetFieldCoordinate()}-{field.Piece.GetPieceId()}";

    public virtual string GetMoveRecord()
        => $"{this.GetType().Name}: {SourceField} {TargetField}";

    public virtual void ExecuteMove(Chessboard chessboard)
    {
        var sourceFieldData = chessboard.ParseFieldData(SourceField);
        var targetFieldData = chessboard.ParseFieldData(TargetField);
        
        if (sourceFieldData.Piece is null)
            throw new ChessCoreException($"Source data '{SourceField}' did not return piece. Source data must have always piece on field!");

        if (targetFieldData.Piece is not null)
            chessboard.RemovePiece(targetFieldData.Field);

        chessboard.MovePiece(sourceFieldData.Piece, targetFieldData.Field);
    }

    public virtual void RevertMove(Chessboard chessboard)
    {
        var parsedTargetField = TargetField.Split('-');
        var parsedSourceField = SourceField.Split('-');

        var currentPosition = $"{parsedTargetField[0]}-{parsedSourceField[1]}";
        var currentTargetFieldData = chessboard.ParseFieldData(currentPosition);
        if (currentTargetFieldData.Piece is null)
            throw new ChessCoreException($"Current position '{currentPosition}' did not return piece. Current position data must have always piece on field!");


        var sourceField = chessboard[parsedSourceField[0]];

        chessboard.MovePiece(currentTargetFieldData.Piece, sourceField);
        if (parsedTargetField.Length == 2)
        {
            int pieceId = int.Parse(parsedTargetField[1]);
            chessboard.ReturnPiece(currentTargetFieldData.Field, pieceId);
        }
    }

    public static Move Parse(string record)
    {
        var parsedRecord = record.Split(' ');
        switch (parsedRecord[0])
        {
            case $"{nameof(Move)}:":
                return new Move(parsedRecord[1], parsedRecord[2], false);

            case $"{nameof(CastlingMove)}:":
                return new CastlingMove(parsedRecord[1], parsedRecord[2], false);

            case $"{nameof(EnPassantMove)}:":
                return new EnPassantMove(parsedRecord[1], parsedRecord[2], false);

            case $"{nameof(PawnSprintMove)}:":
                return new PawnSprintMove(parsedRecord[1], parsedRecord[2], false);

            case $"{nameof(PromotionMove)}:":
                return new PromotionMove(parsedRecord[1], parsedRecord[2], false, int.Parse(parsedRecord[3]));

            default:
                throw new ChessCoreException($"Couldn't parse record. Move '{parsedRecord[0]}' was not recognized!");
        }
    }
}
