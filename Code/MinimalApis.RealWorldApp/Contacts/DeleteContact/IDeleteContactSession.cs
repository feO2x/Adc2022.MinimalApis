using System.Threading.Tasks;
using MinimalApis.RealWorldApp.DataAccess.Model;
using Synnotech.DatabaseAbstractions;

namespace MinimalApis.RealWorldApp.Contacts.DeleteContact;

public interface IDeleteContactSession : IAsyncSession
{
    Task<Contact?> GetContactAsync(int id);
    Task DeleteAddressAsync(Address address);
    Task DeleteContactAsync(Contact contact);
}