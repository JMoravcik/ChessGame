using ChessService.Contracts.MoveResults;
using System.Text.Json.Serialization;

namespace ChessGame.ChessService.Contracts.MoveResults;

[JsonDerivedType(typeof(CorrectMove))]
[JsonDerivedType(typeof(InvalidMove))]
[JsonDerivedType(typeof(FinishMove))]
public abstract record MoveResult
{
}
