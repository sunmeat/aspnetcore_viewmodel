using Microsoft.EntityFrameworkCore;
using StoringPassword.Contexts;

// dotnet add package Microsoft.EntityFrameworkCore !!! встановлюємо пакети, інакше код не працюватиме, View > Terminal (або NuGet)
// dotnet add package Microsoft.EntityFrameworkCore.SqlServer !!!

var builder = WebApplication.CreateBuilder(args);

// всі сесії працюють поверх об'єкта IDistributedCache,
// ASP.NET Core надає вбудовану реалізацію IDistributedCache
builder.Services.AddDistributedMemoryCache(); // додаємо IDistributedMemoryCache
builder.Services.AddSession(); // додаємо сервіси сесії

// отримуємо рядок підключення з файлу конфігурації
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

// додаємо контекст UserContext як сервіс у застосунок
builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer(connection));

// додаємо сервіси MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseSession(); // додаємо проміжний компонент для роботи з сесіями
app.UseStaticFiles(); // обробляє запити до файлів у папці wwwroot

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();