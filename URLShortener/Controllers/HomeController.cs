using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using URLShortener.Models;

namespace URLShortener.Controllers;

public class HomeController : Controller
{
    private static readonly Dictionary<string, string> urlDictionary = new();
    private static readonly Random random = new();
    private const int urlLength = 6;
    private const string baseUrl = "http://localhost:5025/";
    
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
        
        // Если оригинальная ссылка существует
        if (urlDictionary.ContainsValue(originalUrl))
        {
            // Получаем ее
            shortKey = urlDictionary.First(x => x.Value == originalUrl).Key;
            
        }
        // Иначе генерируем новую
        else
        {
            do
            {
                shortKey = GenerateShortKey(urlLength);
            } while (urlDictionary.ContainsKey(shortKey));
        
            urlDictionary.Add(shortKey, originalUrl);
        }
        
        string shortUrl = baseUrl + shortKey; // создаем короткий URL 
        ViewBag.ShortUrl = shortUrl;
        
        return View("Index");
    }

    /// <summary>
    /// Генерация короткой ссылки
    /// </summary>
    /// <param name="length">длинна</param>
    /// <returns></returns>
    private string GenerateShortKey(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        
        char[] charsarr = chars.ToCharArray();

        return new string(random.GetItems(charsarr, length));
    }
    
    public IActionResult RedirectOnOriginalUrl(string id)
    {
        // Если ссылка существует
        if (urlDictionary.TryGetValue(id, out string longUrl))
        {
            // Редирект
            return Redirect(longUrl);
        }
        // Иначе 404
        return NotFound("Ссылка не найдена.");
    }
    
}