using System.Threading.Tasks;
using MinimalApis.RealWorldApp.DataAccess.Model;
using Synnotech.DatabaseAbstractions;

namespace MinimalApis.RealWorldApp.Contacts.NewContact;

public interface INewContactSession : IAsyncSession
{
    Task<int> InsertContactAsync(Contact contact);
    Task<int> InsertAddressAsync(Address address);
}