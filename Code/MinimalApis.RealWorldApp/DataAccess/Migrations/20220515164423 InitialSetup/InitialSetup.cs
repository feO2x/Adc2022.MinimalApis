using System.Threading;
using System.Threading.Tasks;
using Bogus;
using Light.EmbeddedResources;
using LinqToDB;
using LinqToDB.Data;
using MinimalApis.RealWorldApp.DataAccess.Model;
using Synnotech.Migrations.Core.Int64TimestampVersions;
using Synnotech.Migrations.Linq2Db.Int64TimestampVersions;

// ReSharper disable once CheckNamespace -- all migrations must be in the MinimalApis.RealWorldApp.DataAccess.Migrations namespace
namespace MinimalApis.RealWorldApp.DataAccess.Migrations;

[MigrationVersion(20220515164423L)]
public sealed class InitialSetup : Migration
{
    public override async Task ApplyAsync(DataConnection dataConnection, CancellationToken cancellationToken = default)
    {
        await dataConnection.ExecuteAsync(this.GetEmbeddedResource("Initial Structure.sql"), cancellationToken);

        var contactFaker = new Faker<Contact>().RuleFor(c => c.FirstName, f => f.Name.FirstName())
                                               .RuleFor(c => c.LastName, f => f.Name.LastName())
                                               .RuleFor(c => c.Email, (f, c) => f.Internet.Email(c.FirstName, c.LastName))
                                               .UseSeed(42);
        var addressFaker = new Faker<Address>().RuleFor(a => a.Street, f => f.Address.StreetName())
                                               .RuleFor(a => a.ZipCode, f => f.Address.ZipCode("#####"))
                                               .RuleFor(a => a.Location, f => f.Address.City())
                                               .UseSeed(42);
        for (var i = 0; i < 1000; i++)
        {
            var contact = contactFaker.Generate();
            contact.Id = await dataConnection.InsertWithInt32IdentityAsync(contact, token: cancellationToken);

            var address = addressFaker.Generate();
            address.ContactId = contact.Id;
            await dataConnection.InsertAsync(address, token: cancellationToken);
        }
    }
}