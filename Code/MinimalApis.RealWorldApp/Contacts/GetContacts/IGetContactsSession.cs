using System.Collections.Generic;
using System.Threading.Tasks;
using MinimalApis.RealWorldApp.DataAccess.Model;
using Synnotech.DatabaseAbstractions;

namespace MinimalApis.RealWorldApp.Contacts.GetContacts;

public interface IGetContactsSession : IAsyncReadOnlySession
{
    Task<List<Contact>> GetContactsAsync(int skip, int take, string? searchTerm);
}