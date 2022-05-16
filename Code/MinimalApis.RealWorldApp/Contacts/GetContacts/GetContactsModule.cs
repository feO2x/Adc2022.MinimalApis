using Microsoft.Extensions.DependencyInjection;
using Synnotech.Linq2Db;

namespace MinimalApis.RealWorldApp.Contacts.GetContacts;

public static class GetContactsModule
{
    public static IServiceCollection AddGetContacts(this IServiceCollection services) =>
        services.AddSessionFactoryFor<IGetContactsSession, LinqToDbGetContactsSession>();
}