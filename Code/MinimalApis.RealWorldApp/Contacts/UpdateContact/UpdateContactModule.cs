using Microsoft.Extensions.DependencyInjection;
using Synnotech.Linq2Db;

namespace MinimalApis.RealWorldApp.Contacts.UpdateContact;

public static class UpdateContactModule
{
    public static IServiceCollection AddUpdateContact(this IServiceCollection services) =>
        services.AddSessionFactoryFor<IUpdateContactSession, LinqToDbUpdateContactSession>()
                .AddSingleton<UpdateContactDtoValidator>();
}