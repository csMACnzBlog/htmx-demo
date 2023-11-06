namespace dotnet_demo.Models;
public record class ContactEditModel(long Id, string FirstName, string LastName, Dictionary<string, string>? Errors = null) : ContactFormModel(FirstName, LastName, Errors)
{
}
