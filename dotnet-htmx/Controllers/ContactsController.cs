using Microsoft.AspNetCore.Mvc;
using backend;
using dotnet_demo.Models;
using Htmx;

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

        var outOfBandSwap = Request.IsHtmx();
        var model = new ContactsViewModel(
            Contacts: contacts,
            ContactResultCount: new ContactResultCountModel(resultCount),
            Query: q,
            PreviousPage: page > 0 ? page - 1 : null,
            NextPage: contacts.Length > 0 ? page + 1 : null,
            OutofBandSwap: outOfBandSwap);
        if (Request.IsHtmx() && !Request.IsHtmxBoosted())
        {
            if (model.NextPage == 1)
            {
                Response.Headers.Add("HX-Trigger", "contactCountChanged");
            }
            if (Request?.Headers.TryGetValue("HX-Trigger", out var trigger) ?? false
                && (trigger == "search" || trigger == "nextPage" || trigger == "searchForm"))
            {
                return PartialView("ContactItems", model);
            }
            else
            {
                // When we respond to HTMX
                return PartialView(model);
            }
        }

        return View(model);
    }

    [Route("count")]
    public IActionResult ListCount([FromQuery] string? q = null)
    {
        var resultCount = Contacts.GetContactsCount(q);
        var model = new ContactResultCountModel(Count: resultCount);
        return PartialView("ContactCount", model);
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
    public IActionResult AddContact()
    {
        var model = new ContactAddModel("", "");
        return View("AddContact", model);
    }

    [HttpPost, Route("add")]
    public IActionResult AddContact([FromForm] ContactAddModel model)
    {
        var newContact = new Contact(-1, model.FirstName, model.LastName);

        var result = Contacts.AddContact(newContact);

        return result switch
        {
            ContactErrorResult e => View("AddContact", model with { Errors = e.Errors }),
            ContactSuccessResult s => RedirectToAction("ViewContact", new { s.Id }),
            _ => throw new NotSupportedException()
        };
    }

    [HttpGet, Route("{id}/edit")]
    public IActionResult EditContact([FromRoute] int id)
    {
        var contact = Contacts.GetContactById(id);
        if (contact is null)
        {
            return NotFound();
        }
        var model = new ContactEditModel(contact.Id, contact.FirstName, contact.LastName);
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

        return result switch
        {
            ContactErrorResult e => View("EditContact", model with { Errors = e.Errors }),
            ContactSuccessResult s => RedirectToAction("ViewContact", new { s.Id }),
            _ => throw new NotSupportedException()
        };
    }
}