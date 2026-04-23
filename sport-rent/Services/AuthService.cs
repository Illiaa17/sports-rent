using System.Linq;
using sport_rent.Models;

namespace sport_rent.Services;

/// <summary>
/// Сервіс для керування автентифікацією та сесіями користувачів.
/// Реалізує логіку входу в систему та перевірки прав доступу.
/// </summary>
public class AuthService
{
    /// <summary>
    /// Екземпляр сервісу для доступу через патерн Singleton.
    /// Забезпечує єдину точку керування сесією користувача у всьому застосунку.
    /// </summary>
    public static AuthService Instance { get; } = new();

    /// <summary>
    /// Сервіс для завантаження даних користувачів із файлового сховища.
    /// </summary>
    private readonly JsonDataService _dataService = new();
    
    /// <summary>
    /// Поточний авторизований користувач. Має значення null, якщо вхід не виконано.
    /// </summary>
    public User? CurrentUser { get; private set; }
    
    /// <summary>
    /// Перевіряє, чи має поточний користувач права адміністратора.
    /// </summary>
    public bool IsAdmin => CurrentUser?.Role == "Admin";
    
    /// <summary>
    /// Виконує спробу входу в систему за логіном та паролем.
    /// </summary>
    /// <param name="login">Ім'я користувача.</param>
    /// <param name="password">Пароль у відкритому вигляді.</param>
    /// <returns>True, якщо дані вірні та вхід успішний; інакше — false.</returns>
    public bool Login(string login, string password)
    {
        var users = _dataService.Load<User>("users.json");
        var user = users.FirstOrDefault(u => u.Login == login);
        if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            CurrentUser = user;
            return true;
        }
        return false;
    }
}
