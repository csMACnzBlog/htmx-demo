using dotnet_demo.Data;

namespace dotnet_demo.Models;

public record class ContactsViewModel(IEnumerable<Contact> Contacts)
{
}
