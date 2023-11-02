using backend;

namespace dotnet_demo.Models;

public record class ContactsViewModel(ICollection<Contact> Contacts, ContactResultCountModel ContactResultCount, string? Query, int NextPage, bool OutofBandSwap = false)
{
}
