using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using dotnet_demo.Models;
using Htmx;
using dotnet_demo.Data;

namespace dotnet_demo.Controllers;

public class ContactsController : Controller
{
    private readonly ILogger<ContactsController> _logger;

    public ContactsController(ILogger<ContactsController> logger)
    {
        _logger = logger;
    }

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
        if (Request.IsHtmx())
        {
            if(model.NextPage == 1){
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

    public IActionResult ViewContact([FromRoute] int id)
    {
        var model = new ContactViewModel(Contacts.LoadContact(id));
        return View(model);
    }

    public IActionResult EditContact([FromRoute] int id)
    {
        var model = new ContactViewModel(Contacts.LoadContact(id));
        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
