namespace backend;

public record class ContactResult(bool Successful, Dictionary<string, string> Errors)
{
    public static ContactResult Success => new(true, new());

    public static ContactResult Error(params (string propertyName, string errorMessage)[] Errors)
        => new(false, Errors.ToDictionary(kvp => kvp.propertyName, kvp => kvp.errorMessage));
}