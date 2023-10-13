using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using dotnet_demo.Models;
using Htmx;

namespace dotnet_demo.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
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

    public IActionResult Privacy()
    {
        if (Request.IsHtmx())
        {
            // When we respond to HTMX
            return PartialView();
        }

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
