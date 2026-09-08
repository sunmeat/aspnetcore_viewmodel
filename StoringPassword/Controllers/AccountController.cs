using Microsoft.AspNetCore.Mvc;
using StoringPassword.Contexts;
using StoringPassword.Models;
using StoringPassword.ViewModels;
using System.Security.Cryptography; // підключення криптографічних засобів
using System.Text;

namespace StoringPassword.Controllers
{
    public class AccountController : Controller 
    {
        private readonly UserContext _context;

        public AccountController(UserContext context)
        {
            _context = context;
        }

        public ActionResult Login() // дія для відображення форми входу
        {
            return View(); // повертаємо представлення Login
        }

        [HttpPost] // атрибут вказує, що дія обробляє POST-запити
        [ValidateAntiForgeryToken] // захист від CSRF-атак
        public IActionResult Login(LoginViewModel logon) // LoginViewModel, а не User!
        {
            if (ModelState.IsValid) 
            {
                if (_context.Users.ToList().Count == 0) // якщо в базі немає жодного користувача
                {
                    ModelState.AddModelError("", "Невірний логін або пароль!"); // повідомлення про помилку (максимально без подробиць)
                    return View(logon); // повертаємо форму з помилкою
                }

                var users = _context.Users.Where(a => a.Login == logon.Login); // пошук користувача за логіном
                if (users.ToList().Count == 0) // якщо користувача з таким логіном не знайдено
                {
                    ModelState.AddModelError("", "Невірний логін або пароль!"); // повідомлення про помилку
                    return View(logon);
                }

                var user = users.First(); // отримуємо першого (та єдиного) знайденого користувача
                string? salt = user.Salt; // отримуємо "сіль" користувача

                // перетворюємо пароль з сіллю в масив байтів
                byte[] password = Encoding.Unicode.GetBytes(salt + logon.Password);

                // створюємо об'єкт для обчислення MD5-хешу
                var md5 = MD5.Create();
                /* чесно кажучи, MD5 є застарілим і небезпечним алгоритмом для хешування паролів.
                 * він дуже швидкий, що дозволяє атакуючим швидко перебирати мільярди варіантів (brute-force) за допомогою сучасного обладнання (GPU/ASIC).
                 * і навіть з сіллю (salt) MD5 не рекомендується для зберігання паролів уже багато років.

сучасні та рекомендовані алгоритми:
за рекомендаціями OWASP (Password Storage Cheat Sheet) та експертів з криптографії, найкращі варіанти для хешування паролів користувачів:

1) Argon2 (переважно варіант Argon2id)
- переможець конкурсу Password Hashing Competition (2015)
- найсучасніший і найбезпечніший: стійкий до атак на GPU/ASIC завдяки memory-hard (вимагає багато пам'яті) та tunable параметрам (час, пам'ять, паралелізм)
- захищає від side-channel атак і brute-force
- ідеальний вибір для нових систем

2) scrypt
- memory-hard алгоритм, добрий захист від апаратних атак
- альтернатива, якщо Argon2 недоступний.

3) bcrypt
- класика, досі безпечний при правильних параметрах
- автоматично обробляє salt

4) PBKDF2
- прийнятний, але weakest серед сучасних
- використовується в .NET Identity за замовчуванням.
                 */

                // обчислюємо хеш у байтах
                byte[] byteHash = md5.ComputeHash(password);

                var hash = new StringBuilder(byteHash.Length); // будівельник для hex-рядка хешу
                for (int i = 0; i < byteHash.Length; i++) // цикл по всіх байтах хешу
                    hash.Append(string.Format("{0:X2}", byteHash[i])); // додаємо двозначне hex-подання

                if (user.Password != hash.ToString()) // порівняння збереженого хешу з обчисленим
                {
                    ModelState.AddModelError("", "Невірний логін або пароль!"); // повідомлення про помилку
                    return View(logon); // повертаємо форму з помилкою
                }

                HttpContext.Session.SetString("FirstName", user.FirstName!); // зберігаємо ім'я в сесії
                HttpContext.Session.SetString("LastName", user.LastName!); // зберігаємо прізвище в сесії
                return RedirectToAction("Index", "Home"); // перенаправлення на головну сторінку
            }
            return View(logon); // повертаємо форму при невалідних даних
        }

        public IActionResult Register() // дія для відображення форми реєстрації
        {
            return View(); // повертаємо представлення Register
        }

        [HttpPost] // атрибут вказує, що дія обробляє POST-запити
        [ValidateAntiForgeryToken] // захист від CSRF-атак
        public IActionResult Register(RegisterViewModel reg) // обробка форми реєстрації
        {
            if (ModelState.IsValid) // перевірка валідності моделі
            {
                var user = new User(); // створюємо нового користувача
                user.FirstName = reg.FirstName; // встановлюємо ім'я
                user.LastName = reg.LastName; // встановлюємо прізвище
                user.Login = reg.Login; // встановлюємо логін

                byte[] saltbuf = new byte[16]; // буфер для випадкової солі
                var r = RandomNumberGenerator.Create(); // генератор криптостійких байтів
                r.GetBytes(saltbuf); // заповнюємо буфер випадковими байтами

                var sb = new StringBuilder(16); // будівельник для hex-подання солі
                for (int i = 0; i < 16; i++) // цикл по всіх байтах солі
                    sb.Append(string.Format("{0:X2}", saltbuf[i])); // додаємо двозначне hex-подання

                var salt = sb.ToString(); // отримуємо рядок солі

                // перетворюємо пароль з сіллю в масив байтів
                byte[] password = Encoding.Unicode.GetBytes(salt + reg.Password);

                // створюємо об'єкт для обчислення MD5-хешу
                var md5 = MD5.Create();

                // обчислюємо хеш у байтах
                byte[] byteHash = md5.ComputeHash(password);

                var hash = new StringBuilder(byteHash.Length); // будівельник для hex-рядка хешу
                for (int i = 0; i < byteHash.Length; i++) // цикл по всіх байтах хешу
                    hash.Append(string.Format("{0:X2}", byteHash[i])); // додаємо двозначне hex-подання

                user.Password = hash.ToString(); // зберігаємо хеш пароля
                user.Salt = salt; // зберігаємо сіль
                _context.Users.Add(user); // додаємо користувача до контексту
                _context.SaveChanges(); // зберігаємо зміни в базі даних
                return RedirectToAction("Login"); // перенаправлення на сторінку входу
            }
            return View(reg); // повертаємо форму при невалідних даних
        }
    }
}