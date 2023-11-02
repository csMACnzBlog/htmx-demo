namespace dotnet_demo.Data;
public static class Contacts
{
    private static readonly Dictionary<int, Contact> EditedContacts = new();

    public static Contact LoadContact(int id)
    {
        if (EditedContacts.ContainsKey(id))
        {
            return EditedContacts[id];
        }
        else
        {
            return LoadContacts().Skip(id - 1).Take(1).Single();
        }
    }

    public static IEnumerable<Contact> LoadContacts()
    {
        int id = 0;
        string name = "AAA@"; // @ comes before A
        while (id < 26 * 26 * 26 * 26)
        {
            id++;
            name = NextName(name);
            if (EditedContacts.ContainsKey(id))
            {
                yield return EditedContacts[id];
            }
            else
            {
                yield return new Contact(id, GenerateName(id), name);
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
