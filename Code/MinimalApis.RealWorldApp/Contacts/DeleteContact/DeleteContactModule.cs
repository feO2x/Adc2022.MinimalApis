using Microsoft.Extensions.DependencyInjection;
using Synnotech.Linq2Db;

namespace MinimalApis.RealWorldApp.Contacts.DeleteContact;

public static class DeleteContactModule
{
    public static IServiceCollection AddDeleteContact(this IServiceCollection services) =>
        services.AddSessionFactoryFor<IDeleteContactSession, LinqToDbDeleteContactSession>();
}