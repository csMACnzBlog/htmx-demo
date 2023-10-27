using dotnet_demo.Data;

namespace dotnet_demo.Models;

public record class ContactsViewModel(ICollection<Contact> Contacts, string? Query, int NextPage)
{
}
