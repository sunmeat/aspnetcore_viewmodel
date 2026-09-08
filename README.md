# ASP.NET Core ViewModel

Навчальний ASP.NET Core MVC застосунок, який демонструє використання **ViewModel** (моделей подання) для реєстрації та авторизації користувачів.

Проєкт показує розділення доменної моделі (`User`) і моделей подання (`LoginViewModel`, `RegisterViewModel`), безпечне зберігання паролів з використанням солі (salt) та хешування, а також роботу з сесіями та Entity Framework Core.

## ✨ Особливості

* Використання ViewModel для форм логіну та реєстрації
* Розділення доменної моделі та моделей подання
* Хешування паролів з унікальною сіллю (salt)
* ASP.NET Core MVC + Razor Views
* Entity Framework Core + SQL Server
* Робота з сесіями
* Валідація моделей через Data Annotations
* Захист від CSRF (`ValidateAntiForgeryToken`)

## 🛠️ Технології

* **C#**
* **ASP.NET Core** (.NET 10)
* **Entity Framework Core**
* **SQL Server**
* **Razor**
* **Session**

## 📁 Структура проєкту

```text
StoringPassword/
├── Controllers/
│   ├── AccountController.cs   # Логін / реєстрація
│   └── HomeController.cs
├── Models/
│   └── User.cs                # Доменна модель користувача
├── ViewModels/
│   ├── LoginViewModel.cs      # Модель подання для входу
│   └── RegisterViewModel.cs   # Модель подання для реєстрації
├── Contexts/
│   └── UserContext.cs         # DbContext
├── Views/
│   ├── Account/
│   ├── Home/
│   └── Shared/
├── wwwroot/
├── Program.cs
└── appsettings.json
```

### Навіщо ViewModel?

Доменна модель `User` містить поля для зберігання в базі (включаючи хеш пароля та сіль).  
Моделі подання (`LoginViewModel`, `RegisterViewModel`) містять лише ті поля, які потрібні для форм, з атрибутами валідації та відображення. Це дозволяє:

* не передавати зайві/чутливі дані у view
* використовувати різні правила валідації для різних сценаріїв
* чітко розділяти рівні застосунку

## 🔐 Зберігання паролів

При реєстрації:

1. Генерується випадкова **сіль** (16 байт → hex-рядок).
2. Пароль об’єднується з сіллю.
3. Обчислюється **MD5-хеш**.
4. У базу зберігаються хеш і сіль (сіль зберігається у відкритому вигляді).

> **Примітка:** MD5 використовується лише в навчальних цілях.  
> Для production рекомендуються Argon2id, bcrypt, scrypt або PBKDF2.

## 🚀 Запуск

### 1. Клонування репозиторію

```bash
git clone https://github.com/sunmeat/aspnetcore_viewmodel.git
cd aspnetcore_viewmodel/StoringPassword
```

### 2. Налаштування рядка підключення

Відредагуйте `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=Users;Integrated Security=SSPI;TrustServerCertificate=true"
  }
}
```

### 3. Встановлення пакетів (якщо потрібно)

```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

### 4. Запуск

```bash
dotnet run
```

Відкрийте адресу, яку виведе термінал (зазвичай `https://localhost:5xxx`).

## 🎯 Призначення

Це **навчальний проєкт**, створений для практики роботи з ViewModel в ASP.NET Core MVC, розділення моделей, хешування паролів та базової автентифікації через сесії.

Проєкт навмисно спрощений і не призначений для використання у production без доопрацювань.

## 📄 Ліцензія

MIT License
