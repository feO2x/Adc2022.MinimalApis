using System.Data;
using System.Threading.Tasks;
using MinimalApis.RealWorldApp.DataAccess.Model;
using Synnotech.DatabaseAbstractions;

namespace MinimalApis.RealWorldApp.Contacts.UpdateContact;

public interface IUpdateContactSession : IAsyncSession
{
    Task<Contact?> GetContactAsync(int id);
    Task UpdateContactAsync(Contact contact);
    Task UpdateAddressAsync(Address address);
}