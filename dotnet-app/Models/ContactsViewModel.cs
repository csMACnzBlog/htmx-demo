using dotnet_demo.Data;

namespace dotnet_demo.Models;

public record class ContactsViewModel(
    ICollection<Contact> Contacts,
    ContactResultCountModel ContactResultCount,
    string? Query,
    int? PreviousPage,
    int? NextPage)
{
}
