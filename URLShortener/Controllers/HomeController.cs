using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using URLShortener.Models;

namespace URLShortener.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    
}