using ChessGame.Common.DI_Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Backend.Common.Scopeds.Database;

public class ChessGameBackendCommon_DatabaseSetup : Setup
{
    public ChessGameBackendCommon_DatabaseSetup(EnvironmentInfo environment) : base(environment)
    {
    }

    public override void AddDependencies(IServiceCollection serviceCollection, params object[] args)
    {
        serviceCollection.AddScoped<IAuthenticationRepository>(sp => sp.GetRequiredService<IRepository>());
        serviceCollection.AddScoped<IGameRepository>(sp => sp.GetRequiredService<IRepository>());
        base.AddDependencies(serviceCollection, args);
    }
}
