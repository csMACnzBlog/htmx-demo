using Microsoft.AspNetCore.Mvc;
using dotnet_demo.Models;
using dotnet_demo.Data;

namespace dotnet_demo.Controllers;

[Route("/contacts")]
public class ContactsController : Controller
{
    private readonly ILogger<ContactsController> _logger;

    public ContactsController(ILogger<ContactsController> logger)
    {
        _logger = logger;
    }

    [Route("")]
    public IActionResult List([FromQuery] string? q = null, [FromQuery] int page = 0)
    {
        var contacts = Contacts.LoadContacts();

        if (!string.IsNullOrEmpty(q))
        {
            contacts = contacts.Where(c => c.LastName.StartsWith(q.ToUpperInvariant()));
        }
        var resultCount = contacts.Count();
        contacts = contacts.Skip(page * 10).Take(10);

        var currentPageContacts = contacts.ToArray();
        var model = new ContactsViewModel(
            Contacts: currentPageContacts,
            ContactResultCount: new ContactResultCountModel(resultCount),
            Query: q,
            PreviousPage: page > 0 ? page - 1 : null,
            NextPage: currentPageContacts.Length > 0 ? page + 1 : null);

        return View(model);
    }

    [Route("{id}")]
    public IActionResult ViewContact([FromRoute] int id)
    {
        var model = new ContactViewModel(Contacts.LoadContact(id));
        return View(model);
    }

    [HttpGet, Route("{id}/edit")]
    public IActionResult EditContact([FromRoute] int id)
    {
        var model = new ContactViewModel(Contacts.LoadContact(id));
        return View(model);
    }

    [HttpPost, HttpPut, Route("{id}/edit")]
    public IActionResult EditContact([FromRoute] int id, [FromForm] ContactEditModel model)
    {
        var existingContact = Contacts.LoadContact(id);
        return RedirectToAction("ViewContact", new { Id = id });
    }
}