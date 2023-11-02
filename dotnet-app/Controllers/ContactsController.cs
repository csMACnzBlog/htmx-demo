using Microsoft.AspNetCore.Mvc;
using dotnet_demo.Models;
using backend;

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
        var contacts = Contacts.GetContacts(q, pageNumber: page);
        var resultCount = Contacts.GetContactsCount(q);

        var model = new ContactsViewModel(
            Contacts: contacts,
            ContactResultCount: new ContactResultCountModel(resultCount),
            Query: q,
            PreviousPage: page > 0 ? page - 1 : null,
            NextPage: contacts.Length > 0 ? page + 1 : null);

        return View(model);
    }

    [Route("{id}")]
    public IActionResult ViewContact([FromRoute] int id)
    {
        var contact = Contacts.GetContactById(id);
        if (contact is null)
        {
            return NotFound();
        }

        var model = new ContactViewModel(contact);
        return View(model);
    }

    [HttpGet, Route("{id}/edit")]
    public IActionResult EditContact([FromRoute] int id)
    {
        var contact = Contacts.GetContactById(id);
        if (contact is null)
        {
            return NotFound();
        }

        var model = new ContactViewModel(contact);
        return View(model);
    }

    [HttpPost, HttpPut, Route("{id}/edit")]
    public IActionResult EditContact([FromRoute] int id, [FromForm] ContactEditModel model)
    {
        var contact = Contacts.GetContactById(id);
        if (contact is null)
        {
            return NotFound();
        }

        // TODO: save

        return RedirectToAction("ViewContact", new { Id = id });
    }
}