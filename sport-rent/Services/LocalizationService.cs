using System;

namespace sport_rent.Services;

public class LocalizationService
{
    public static LocalizationService Instance { get; } = new();
    public event Action? LanguageChanged;

    private string _lang = "uk";
    public string CurrentLang
    {
        get => _lang;
        set { _lang = value; LanguageChanged?.Invoke(); }
    }

    private readonly Dictionary<string, Dictionary<string, string>> _dict = new()
    {
        ["uk"] = new() { {"Login", "Вхід"}, {"Dashboard", "Головна"}, {"Equipment", "Спорядження"}, {"Customers", "Клієнти"}, {"Rentals", "Оренди"}, {"Settings", "Налаштування"}, {"Logout", "Вийти"} },
        ["en"] = new() { {"Login", "Login"}, {"Dashboard", "Dashboard"}, {"Equipment", "Equipment"}, {"Customers", "Customers"}, {"Rentals", "Rentals"}, {"Settings", "Settings"}, {"Logout", "Logout"} }
    };

    public string this[string key] => _dict[_lang].GetValueOrDefault(key, key);
}
