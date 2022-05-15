using LinqToDB.Data;
using Synnotech.Migrations.Linq2Db.Int64TimestampVersions;
using System.Threading;
using System.Threading.Tasks;
using Light.EmbeddedResources;
using Synnotech.Migrations.Core.Int64TimestampVersions;

// ReSharper disable once CheckNamespace -- all migrations must be in the MinimalApis.RealWorldApp.DataAccess.Migrations namespace
namespace MinimalApis.RealWorldApp.DataAccess.Migrations;

[MigrationVersion(20220515164423L)]
public sealed class InitialSetup : Migration
{
    public override async Task ApplyAsync(DataConnection dataConnection, CancellationToken cancellationToken = default)
    {
        await dataConnection.ExecuteAsync(this.GetEmbeddedResource("Initial Structure.sql"), cancellationToken);

    }
}