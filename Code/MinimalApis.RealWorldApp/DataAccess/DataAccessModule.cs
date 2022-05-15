using Microsoft.Extensions.DependencyInjection;
using Synnotech.Linq2Db.MsSqlServer;
using Synnotech.Migrations.Linq2Db.Int64TimestampVersions;

namespace MinimalApis.RealWorldApp.DataAccess;

public static class DataAccessModule
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services) =>
        services.AddLinq2DbForSqlServer()
                .AddSynnotechMigrations();
}