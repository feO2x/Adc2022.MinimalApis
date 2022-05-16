using System.Threading.Tasks;
using LinqToDB.DataProvider.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Synnotech.Linq2Db.MsSqlServer;
using Synnotech.Migrations.Linq2Db.Int64TimestampVersions;
using Synnotech.MsSqlServer;

namespace MinimalApis.RealWorldApp.DataAccess;

public static class DataAccessModule
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services,
                                                   string configurationSectionName = Linq2DbSettings.DefaultSectionName) =>
        services.AddLinq2DbForSqlServer(Mappings.CreateMappings,
                                        configurationSectionName: configurationSectionName,
                                        registerFactoryDelegateForDataConnection: false,
                                        sqlServerProvider: SqlServerProvider.SystemDataSqlClient)
                .AddSynnotechMigrations(typeof(DataAccessModule).Assembly);

    public static async Task MigrateDatabaseAsync(this WebApplication app)
    {
        var dbSettings = app.Services.GetRequiredService<Linq2DbSettings>();
        await Database.TryCreateDatabaseAsync(dbSettings.ConnectionString);
        var logger = app.Services.GetRequiredService<ILogger>();
        var migrationEngine = app.Services.GetRequiredService<MigrationEngine>();
        await migrationEngine.MigrateAndLogAsync(logger);
    }

    public static async Task MigrateAndLogAsync(this MigrationEngine migrationEngine, ILogger logger)
    {
        var summary = await migrationEngine.MigrateAsync();

        if (summary.TryGetAppliedMigrations(out var appliedMigrations))
        {
            logger.Information("The following migrations were applied:");
            foreach (var appliedMigration in appliedMigrations)
            {
                logger.Information("{Migration}", appliedMigration.ToString());
            }
        }

        summary.EnsureSuccess();
    }
}