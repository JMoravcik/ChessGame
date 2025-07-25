using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.ChessService.Contracts.MoveResults;

public record Promotion : MoveResult
{
    public required string Field { get; init; }

}
