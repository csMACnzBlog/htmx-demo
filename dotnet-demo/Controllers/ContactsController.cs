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

    public IActionResult List()
    {
        var contacts = Contacts.LoadContacts()
            // .Skip(10000)
            // .Where(c => c.LastName.StartsWith("ZZE"))
            .Take(10);
        var model = new ContactsViewModel(contacts);
        if (Request.IsHtmx())
        {
            // When we respond to HTMX
            return PartialView(model);
        }

        return View(model);
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
