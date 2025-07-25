using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.ChessService.Contracts.MoveResults;

public record InvalidMove : MoveResult
{
    public string Reason { get; }
    public InvalidMove(string reason)
    {
        Reason = reason;
    }
    public override string ToString()
    {
        return $"Invalid Move: {Reason}";
    }
}
