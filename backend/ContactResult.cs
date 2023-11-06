namespace backend;

public interface IContactResult
{ }

public sealed record class ContactSuccessResult(int Id) : IContactResult;

public sealed record class ContactErrorResult(Dictionary<string, string> Errors) : IContactResult
{
    public ContactErrorResult(string propertyName, string value) : this(new Dictionary<string, string> { { propertyName, value } })
    { }
}