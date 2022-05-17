using System.Threading.Tasks;
using LinqToDB;
using LinqToDB.Data;
using MinimalApis.RealWorldApp.DataAccess;
using MinimalApis.RealWorldApp.DataAccess.Model;
using Synnotech.Linq2Db;

namespace MinimalApis.RealWorldApp.Contacts.UpdateContact;

public sealed class LinqToDbUpdateContactSession : AsyncSession, IUpdateContactSession
{
    public LinqToDbUpdateContactSession(DataConnection dataConnection) : base(dataConnection) { }

    public Task<Contact?> GetContactAsync(int id) =>
        DataConnection.GetContactWithAddressAsync(id);

    public Task UpdateContactAsync(Contact contact) =>
        DataConnection.UpdateAsync(contact);

    public Task UpdateAddressAsync(Address address) =>
        DataConnection.UpdateAsync(address);
}