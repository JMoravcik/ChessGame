using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Common.Bases;

public abstract class LinkerContext
{
    public abstract Task<string> GetDeviceTokenAsync();
    public abstract Task<string?> GetAuthTokenAsync();
}
