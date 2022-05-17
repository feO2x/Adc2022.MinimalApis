using System.Threading.Tasks;
using LinqToDB;
using LinqToDB.Data;
using MinimalApis.RealWorldApp.DataAccess.Model;

namespace MinimalApis.RealWorldApp.DataAccess;

public static class CommonQueries
{
    public static Task<Contact?> GetContactWithAddressAsync(this DataConnection dataConnection, int id) =>
        dataConnection.GetTable<Contact>()
                      .LoadWith(c => c.Address)
                      .FirstOrDefaultAsync(c => c.Id == id);
}