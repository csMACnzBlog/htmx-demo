namespace dotnet_demo.Models;

public record class ContactFormModel(string FirstName, string LastName, Dictionary<string, string>? Errors = null) { }
