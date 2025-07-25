using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Common.DI_Tools;

/// <summary>
/// Informations about environment which is currently calling dependency injection setups
/// </summary>
public class EnvironmentInfo
{
    public readonly IConfiguration Configuration;

    public EnvironmentInfo(IConfiguration configuration)
    {
        Configuration = configuration;
    }
}
