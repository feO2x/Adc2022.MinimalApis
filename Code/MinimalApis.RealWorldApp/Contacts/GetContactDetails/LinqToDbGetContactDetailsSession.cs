using System.Threading.Tasks;
using LinqToDB.Data;
using MinimalApis.RealWorldApp.DataAccess;
using MinimalApis.RealWorldApp.DataAccess.Model;
using Synnotech.Linq2Db;

namespace MinimalApis.RealWorldApp.Contacts.GetContactDetails;

public sealed class LinqToDbGetContactDetailsSession : AsyncReadOnlySession, IGetContactDetailsSession
{
    public LinqToDbGetContactDetailsSession(DataConnection dataConnection) : base(dataConnection) { }

    public Task<Contact?> GetContactAsync(int id) =>
        DataConnection.GetContactWithAddressAsync(id);
}