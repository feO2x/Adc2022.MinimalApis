using LinqToDB.Mapping;
using MinimalApis.RealWorldApp.DataAccess.Model;
using Synnotech.Migrations.Linq2Db.Int64TimestampVersions;

namespace MinimalApis.RealWorldApp.DataAccess;

public static class Mappings
{
    public static void CreateMappings(MappingSchema schema)
    {
        var builder = schema.GetFluentMappingBuilder();

        builder.MapMigrationInfo();

#nullable disable
        builder.Entity<Contact>()
               .HasTableName("Contacts")
               .Property(c => c.Id).IsPrimaryKey().IsIdentity()
               .Association(c => c.Address, c => c.Id, a => a.ContactId);

        builder.Entity<Address>()
               .HasTableName("Addresses")
               .Property(a => a.Id).IsPrimaryKey().IsIdentity()
               .Association(a => a.Contact, a => a.ContactId, c => c.Id, false);
#nullable restore
    }
}