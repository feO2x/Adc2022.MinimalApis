using System.Threading.Tasks;
using LinqToDB;
using LinqToDB.Data;
using MinimalApis.RealWorldApp.DataAccess.Model;
using Synnotech.Linq2Db;

namespace MinimalApis.RealWorldApp.Contacts.NewContact;

public sealed class LinqToDbNewContactSession : AsyncSession, INewContactSession
{
    public LinqToDbNewContactSession(DataConnection dataConnection)
        : base(dataConnection) { }

    public Task<int> InsertContactAsync(Contact contact) =>
        DataConnection.InsertWithInt32IdentityAsync(contact);

    public Task<int> InsertAddressAsync(Address address) =>
        DataConnection.InsertWithInt32IdentityAsync(address);
}