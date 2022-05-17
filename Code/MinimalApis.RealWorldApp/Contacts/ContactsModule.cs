using Microsoft.Extensions.DependencyInjection;
using MinimalApis.RealWorldApp.Contacts.DeleteContact;
using MinimalApis.RealWorldApp.Contacts.GetContactDetails;
using MinimalApis.RealWorldApp.Contacts.GetContacts;
using MinimalApis.RealWorldApp.Contacts.NewContact;

namespace MinimalApis.RealWorldApp.Contacts;

public static class ContactsModule
{
    public static IServiceCollection AddContactsModule(this IServiceCollection services) =>
        services.AddGetContacts()
                .AddGetContactDetails()
                .AddNewContact()
                .AddDeleteContact();
}