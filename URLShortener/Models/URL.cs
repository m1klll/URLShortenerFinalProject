namespace URLShortener.Models;

/// <summary>
/// 
/// </summary>
public class URL
{
    public string OriginalURL { get; set; }
    public string ShortKey { get; set; }
    public int RedirectCount { get; set; }

    public URL(string originalURL, string shortKey)
    {
        OriginalURL = originalURL;
        ShortKey = shortKey;
        RedirectCount = 0;
    }
}

