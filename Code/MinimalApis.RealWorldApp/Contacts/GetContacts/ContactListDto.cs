using System.Collections.Generic;
using MinimalApis.RealWorldApp.DataAccess.Model;

namespace MinimalApis.RealWorldApp.Contacts.GetContacts;

public readonly record struct ContactListDto(int Id, string FirstName, string LastName)
{
    public static ContactListDto[] FromContacts(List<Contact> contacts)
    {
        var length = contacts.Count;
        var array = new ContactListDto[length];
        var i = 0;
        while (i < length)
        {
            var contact = contacts[i];
            var dto = new ContactListDto(contact.Id, contact.FirstName, contact.LastName);
            array[i++] = dto;
        }

        return array;
    }
}