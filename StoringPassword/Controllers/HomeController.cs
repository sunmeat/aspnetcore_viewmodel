using Microsoft.AspNetCore.Mvc;

namespace StoringPassword.Controllers
{
    public class HomeController : Controller // контролер для головної сторінки та виходу
    {
        public ActionResult Index() // дія для відображення головної сторінки
        {
            if (HttpContext.Session.GetString("LastName") != null // перевірка наявності прізвища в сесії
                && HttpContext.Session.GetString("FirstName") != null) // перевірка наявності імені в сесії
                return View(); // повертаємо головне представлення, якщо користувач автентифікований
            else
                return RedirectToAction("Login", "Account"); // перенаправляємо на сторінку входу, якщо ні
        }

        public ActionResult Logout() // дія для виходу користувача з системи
        {
            HttpContext.Session.Clear(); // очищаємо всі дані сесії
            return RedirectToAction("Login", "Account"); // перенаправляємо на сторінку входу
        }
    }
}