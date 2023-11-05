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

    [HttpGet, Route("add")]
    public IActionResult AddContact([FromRoute] int id)
    {
        var contact = Contacts.GetContactById(id);
        if (contact is null)
        {
            return NotFound();
        }

        var model = new ContactViewModel(contact);
        return View("EditContact", model);
    }

    [HttpPost, Route("add")]
    public IActionResult AddContact([FromRoute] int id, [FromForm] ContactAddModel model)
    {
        var newContact = new Contact(-1, model.FirstName, model.LastName);

        var result = Contacts.AddContact(newContact);

        if (result != ContactResult.Success)
        {
            model = model with { Errors = result.Errors};
            return View("AddContact", model);
        }

        return RedirectToAction("ViewContact", new { Id = id });
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
        contact = contact with { FirstName = model.FirstName, LastName = model.LastName };

        var result = Contacts.UpdateContact(contact);

        if (result != ContactResult.Success)
        {
            model = model with { Errors = result.Errors};
            return View("AddContact", model);
        }

        return RedirectToAction("ViewContact", new { Id = id });
    }
}