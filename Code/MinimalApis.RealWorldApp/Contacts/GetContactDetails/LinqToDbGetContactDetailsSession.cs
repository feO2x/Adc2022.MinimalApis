using System.Threading.Tasks;
using LinqToDB;
using LinqToDB.Data;
using MinimalApis.RealWorldApp.DataAccess.Model;
using Synnotech.Linq2Db;

namespace MinimalApis.RealWorldApp.Contacts.GetContactDetails;

public sealed class LinqToDbGetContactDetailsSession : AsyncReadOnlySession, IGetContactDetailsSession
{
    public LinqToDbGetContactDetailsSession(DataConnection dataConnection) : base(dataConnection) { }

    public Task<Contact?> GetContactAsync(int id) =>
        DataConnection.GetTable<Contact>()
                      .LoadWith(c => c.Address)
                      .FirstOrDefaultAsync(c => c.Id == id);
}