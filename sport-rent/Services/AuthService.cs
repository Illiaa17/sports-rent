using sport_rent.Models;

namespace sport_rent.Services;

public class AuthService
{
    private readonly JsonDataService _dataService = new();
    public User? CurrentUser { get; private set; }

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

    public bool IsAdmin => CurrentUser?.Role == "Admin";
}

