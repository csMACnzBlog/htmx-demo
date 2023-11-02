using Microsoft.AspNetCore.Mvc;
using dotnet_demo.Models;
using Htmx;
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

        var outOfBandSwap = Request.IsHtmx();
        var model = new ContactsViewModel(
            Contacts: contacts.ToArray(),
            ContactResultCount: new ContactResultCountModel(resultCount),
            Query: q,
            NextPage: page + 1,
            OutofBandSwap: outOfBandSwap);
        if (Request.IsHtmx() && !Request.IsHtmxBoosted())
        {
            if (model.NextPage == 1)
            {
                Response.Headers.Add("HX-Trigger", "contactCountChanged");
            }
            if (Request?.Headers.TryGetValue("HX-Trigger", out var input) ?? false && (input == "search" || input == "nextPage"))
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
        var contacts = Contacts.LoadContacts();

        if (!string.IsNullOrEmpty(q))
        {
            contacts = contacts.Where(c => c.LastName.StartsWith(q.ToUpperInvariant()));
        }
        var resultCount = contacts.Count();
        var model = new ContactResultCountModel(Count: resultCount);
        return PartialView("ContactCount", model);
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