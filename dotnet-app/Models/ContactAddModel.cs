
namespace dotnet_demo.Models;

public record class ContactAddModel(string FirstName, string LastName, Dictionary<string, string>? Errors = null)
{
}
