using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using URLShortener.Models;

namespace URLShortener.Controllers;

public class HomeController : Controller
{
    private readonly Dictionary<string, string> urlDictionary = new();
    private static readonly Random random = new();
    private const int urlLength = 6;
    private const string baseUrl = "http://localhost:5275/";
    
    public IActionResult Index()
    {
        return View();
    }
/// <summary>
/// Сокращатель ссылки
/// </summary>
/// <param name="url">Оригинальный URL</param>
/// <returns></returns>
    [HttpPost]
    public IActionResult ShortenUrl(string originalUrl)
    {
        string shortKey;
        do
        {
            shortKey = GenerateShortKey(urlLength);
        } while (urlDictionary.ContainsKey(shortKey));
        urlDictionary[shortKey] = originalUrl;
        string shortUrl = baseUrl + shortKey; // создаем короткий URL 
        
        ViewBag.ShortUrl = shortUrl;

        Console.WriteLine(originalUrl);
        return View("Index");
    }

    private string GenerateShortKey(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        
        char[] charsarr = chars.ToCharArray();

        return new string(random.GetItems(charsarr, length));
    }
    
}