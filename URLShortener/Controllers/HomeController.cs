using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using URLShortener.Models;
using System.Text.Json;

namespace URLShortener.Controllers;

public class HomeController : Controller
{
    private static readonly Random random = new();
    private const int urlLength = 6;
    private const string baseUrl = "https://92.39.211.86:7193/";
    private const string filePath = "urls.json";

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
    public async Task<IActionResult> ShortenUrl(string originalUrl)
    {
        string shortKey;
        
        var urls = await ReadUrlsAsync();

        // Если оригинальная ссылка существует
        if (urls.Exists(u => u.OriginalURL == originalUrl))
        {
            // Получаем ее
            shortKey = urls.Find(u => u.OriginalURL == originalUrl).ShortKey;
        }
        // Иначе генерируем новую
        else
        {
            do
            {
                shortKey = GenerateShortKey(urlLength);
            } while (urls.Exists(u => u.ShortKey == shortKey));
        
            urls.Add(new URL(originalUrl, shortKey));
            await WriteUrlsAsync(urls);
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
    
    public async Task<IActionResult> RedirectOnOriginalUrl(string id)
    {
        // Если ссылка существует
        var urls = await ReadUrlsAsync();
        var url = urls.Find(u => u.ShortKey == id);

        if (url != null)
        {
            // Увеличиваем счётчик переходов
            url.RedirectCount++;
            await WriteUrlsAsync(urls);

            // Редирект
            return Redirect(url.OriginalURL);
        }
        // Иначе 404
        return NotFound("Ссылка не найдена.");
    }

    /// <summary>
    /// Чтение из JSON
    /// </summary>
    /// <returns>Возвращает список ссылок</returns>
    private async Task<List<URL>> ReadUrlsAsync()
    {
        if (!System.IO.File.Exists(filePath))
        {
            return new List<URL>();
        }
        
        using var stream = new FileStream(filePath, FileMode.OpenOrCreate);
        var urls = await JsonSerializer.DeserializeAsync<List<URL>>(stream);
        return urls ?? new List<URL>();
    }

    /// <summary>
    /// Запись в JSON
    /// </summary>
    /// <param name="urls">Список ссылок</param>
    private async Task WriteUrlsAsync(List<URL> urls)
    {
        using var stream = new FileStream(filePath, FileMode.OpenOrCreate);
        await JsonSerializer.SerializeAsync(stream, urls);
    }
}