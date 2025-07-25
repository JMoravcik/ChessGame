using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Common.DI_Tools;

public interface IWarmupOperation
{
    int OrderNumber { get; }
    Task WarmUpAsync();
}
