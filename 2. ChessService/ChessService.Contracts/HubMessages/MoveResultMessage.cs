using ChessGame.ChessService.Contracts.MoveResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.ChessService.Contracts.HubMessages;

public class MoveResultMessage
{
    public MoveResult MoveResult { get; set; }
}
