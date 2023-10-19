using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using dotnet_demo.Models;
using Htmx;

namespace dotnet_demo.Controllers;

public class ContactsController : Controller
{
    private readonly ILogger<ContactsController> _logger;

    public ContactsController(ILogger<ContactsController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        if (Request.IsHtmx())
        {
            // When we respond to HTMX
            return PartialView();
        }

        return View();
    }

    public IActionResult ViewContact()
    {
        return View();
    }

    public IActionResult EditContact()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
