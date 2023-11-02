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

    [Route("/")]
    public IActionResult Index()
    {
        
        if (Request.IsHtmx())
        {
            // When we respond to HTMX
            return PartialView();
        }

        return View();
    }

    [Route("/Privacy")]
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
    [Route("/Error")]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
