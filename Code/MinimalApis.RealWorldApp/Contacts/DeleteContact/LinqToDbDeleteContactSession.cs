using System.Threading.Tasks;
using LinqToDB;
using LinqToDB.Data;
using MinimalApis.RealWorldApp.DataAccess;
using MinimalApis.RealWorldApp.DataAccess.Model;
using Synnotech.Linq2Db;

namespace MinimalApis.RealWorldApp.Contacts.DeleteContact;

public sealed class LinqToDbDeleteContactSession : AsyncSession, IDeleteContactSession
{
    public LinqToDbDeleteContactSession(DataConnection dataConnection) : base(dataConnection) { }

    public Task<Contact?> GetContactAsync(int id) =>
        DataConnection.GetContactWithAddressAsync(id);

    public Task DeleteAddressAsync(Address address) => DataConnection.DeleteAsync(address);

    public Task DeleteContactAsync(Contact contact) => DataConnection.DeleteAsync(contact);
}