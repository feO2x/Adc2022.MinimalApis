using System.Threading.Tasks;
using MinimalApis.RealWorldApp.DataAccess.Model;
using Synnotech.DatabaseAbstractions;

namespace MinimalApis.RealWorldApp.Contacts.GetContactDetails;

public interface IGetContactDetailsSession : IAsyncReadOnlySession
{
    Task<Contact?> GetContactAsync(int id);
}