using System.Globalization;
using System.Net.Http.Headers;

namespace backend;
public static class Contacts
{
    private const int GeneratedContactCount = 26 * 26 * 26 * 26; // [A-Z]{4} == 26^4

    private static readonly Dictionary<int, Contact?> EditedContacts = new();

    public static Contact? GetContactById(int id)
    {
        if (EditedContacts.TryGetValue(id, out var contact))
        {
            return contact;
        }
        if (id < GeneratedContactCount)
        {
            return GenerateContacts().Skip(id - 1).Take(1).Single();
        }
        return null;
    }

    public static Contact[] GetContacts(string? surnameStartsWith, int pageSize = 10, int pageNumber = 0)
    {
        return LoadContacts(surnameStartsWith)
            .Skip(pageNumber * pageSize)
            .Take(10)
            .ToArray();

    }

    public static int GetContactsCount(string? surnameStartsWith)
    {
        return LoadContacts(surnameStartsWith).Count();
    }

    public static IContactResult AddContact(Contact contact)
    {
        var nextId = Math.Max(GeneratedContactCount, EditedContacts.Any() ? EditedContacts.Keys.Max() : 0) + 1;

        if (EditedContacts.ContainsKey(nextId)) throw new Exception("Developer Math Error");

        var newContact = contact with { Id = nextId };

        var validationResult = Validate(newContact);
        if (validationResult is null)
        {
            EditedContacts[nextId] = newContact;
            return new ContactSuccessResult(nextId);
        }

        return validationResult;
    }

    public static IContactResult UpdateContact(Contact contact)
    {
        if (!EditedContacts.ContainsKey(contact.Id))
        {
            if (contact.Id >= GeneratedContactCount)
            {
                return new ContactErrorResult(nameof(Contact.Id), "Contact Not Found");
            }
        }

        var validationResult = Validate(contact);
        if (validationResult is null)
        {
            EditedContacts[contact.Id] = contact;
            return new ContactSuccessResult(contact.Id);
        }

        return validationResult;
    }

    private static ContactErrorResult? Validate(Contact contact)
    {
        var errors = new Dictionary<string, string>();
        if (string.IsNullOrWhiteSpace(contact.FirstName))
        {
            errors[nameof(Contact.FirstName)] = "First Name is required";
        }
        else if (contact.FirstName.Contains("@"))
        {
            errors[nameof(Contact.FirstName)] = "First Name cannot contain an '@' symbol";
        }

        if (string.IsNullOrWhiteSpace(contact.LastName))
        {
            errors[nameof(Contact.LastName)] = "Last Name is required";
        }
        else if (contact.LastName.Length <= 2)
        {
            errors[nameof(Contact.LastName)] = "Last must be longer than 2 characters";
        }

        return errors.Any()
            ? new ContactErrorResult(errors)
            : null;
    }

    private static IEnumerable<Contact> LoadContacts(string? surnameStartsWith)
    {
        IEnumerable<Contact> contacts = GenerateContacts()
                .Where(c => c != null)!;

        if (!string.IsNullOrEmpty(surnameStartsWith))
        {
            contacts = contacts
                .Where(c => c.LastName.StartsWith(surnameStartsWith, StringComparison.InvariantCultureIgnoreCase));
        }

        return contacts;
    }

    private static IEnumerable<Contact?> GenerateContacts()
    {
        int id = 0;
        string name = "AAA@"; // @ comes before A
        while (id < GeneratedContactCount)
        {
            id++;
            name = NextName(name);
            if (EditedContacts.TryGetValue(id, out var contact))
            {
                yield return contact;
            }
            else
            {
                yield return new Contact(id, GenerateName(id), name);
            }
        }
        foreach (var kvp in EditedContacts.Where(x => x.Key >= id))
        {
            if (kvp.Value is not null)
            {
                yield return kvp.Value;
            }
        }
    }

    private static string NextName(string lastName)
    {
        char[] result = lastName.ToCharArray();
        var index = result.Length;
        while (index > 0)
        {
            index--;

            if (result[index] != 'Z')
            {
                result[index]++;
                break;
            }
            else
            {
                result[index] = 'A';
                continue;
            }
        }
        return new string(result);
    }

    private static string GenerateName(int id)
    {
        Random rand = new(id);
        return Names[rand.Next(Names.Length)];
    }

    /* https://en.wikipedia.org/wiki/Category:English_given_names */
    private static string[] Names = new[]
    {
        "Adi",
        "Alex",
        "Alex",
        "Angie",
        "Ashleig",
        "Ashton",
        "Aubre",
        "Barnes",
        "Barry",
        "Basil",
        "Bernadin",
        "Bethany",
        "Braden",
        "Bradley",
        "Brent",
        "Bret",
        "Bret",
        "Burdin",
        "Caden",
        "Cadence",
        "Carrington",
        "Charlene",
        "Charle",
        "Charlton",
        "Chay",
        "Che",
        "Christophe",
        "Clinto",
        "Cowden",
        "Dari",
        "Darlee",
        "Darlene",
        "Darnel",
        "Deb",
        "Dem",
        "Denni",
        "Diamond",
        "Doreen",
        "Dorothy",
        "Dustin",
        "Earlen",
        "Elaine",
        "Elfried",
        "Emery",
        "Emory",
        "Eva",
        "Gabriel",
        "Georgian",
        "Gladys",
        "Greenbur",
        "Gregory",
        "Greig",
        "Gwen",
        "Harley",
        "Hastings",
        "Hazel",
        "Heather",
        "Helton",
        "Henrietta",
        "Heston",
        "Holly",
        "Hulda",
        "Increase",
        "India",
        "Irene",
        "Jackie",
        "Jade",
        "January",
        "Jaylo",
        "Jean",
        "Jemma",
        "Jenny",
        "Jeral",
        "Jerrol",
        "Jerry",
        "Jessie",
        "Jethr",
        "Jigar",
        "Jil",
        "Jocely",
        "Jodi",
        "Joey",
        "Justin",
        "Kate",
        "Kathry",
        "Keaton",
        "Kendr",
        "Kerr",
        "Kimball",
        "Kitty",
        "Krist",
        "Kylie",
        "Lare",
        "Lawrence",
        "Lawson",
        "Leann",
        "Liann",
        "Louise",
        "Luc",
        "Maddox",
        "Malfor",
        "Marlene",
        "Mau",
        "Melind",
        "Melville",
        "Miley",
        "Millicen",
        "Mindi",
        "Mind",
        "Molly",
        "Mort",
        "Nancy",
        "Nelson",
        "Nige",
        "Osber",
        "Ottili",
        "Pamela",
        "Pasco",
        "Perc",
        "Piper",
        "Pippa",
        "Poppy",
        "Raleigh",
        "Rebecca",
        "Reynol",
        "Rhoda",
        "Riley",
        "Roland",
        "Rosalee",
        "Rosalie",
        "Rosie",
        "Ruby",
        "Rupert",
        "Ruth",
        "Savannah",
        "Scarlett",
        "Sharo",
        "Sheridan",
        "Shiloh",
        "Sidney",
        "Stacy",
        "Sue",
        "Sydney",
        "Tammy",
        "Tim",
        "Timm",
        "Timothy",
        "Tracy",
        "Travis",
        "Trent",
        "Trudi",
        "Tucker",
        "Velm",
        "Vicar",
        "Violet",
        "Walker",
        "Warren",
        "Whitney",
        "Wilfrie",
        "Woodrow"
    };
}
