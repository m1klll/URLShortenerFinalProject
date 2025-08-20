var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();

app.MapStaticAssets();

// Главный контролер
app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

 // Контроллер, отвечающий за проверку короткой ссылки и редиректа в дальнейшем
app.MapControllerRoute(
    name: "shortUrl",
    pattern: "{id}",
    defaults: new { controller = "Home", action = "RedirectOnOriginalUrl" });

app.Run();