using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using sport_rent.Models;

namespace sport_rent.Services;

public class JsonDataService
{
    private const string DataPath = "Data";

    public List<T> Load<T>(string filename) where T : class, new()
    {
        var path = Path.Combine(DataPath, filename);
        if (!File.Exists(path))
            return Seed<T>(filename);

        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
    }

    public void Save<T>(string filename, List<T> data)
    {
        Directory.CreateDirectory(DataPath);
        var path = Path.Combine(DataPath, filename);
        var options = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(path, JsonSerializer.Serialize(data, options));
    }

    private List<T> Seed<T>(string filename)
    {
        Directory.CreateDirectory(DataPath);
        var list = new List<T>();

        if (typeof(T) == typeof(User))
        {
            list.Add((T)(object)new User { Id = 1, Login = "admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"), Role = "Admin", FullName = "Адміністратор" });
            list.Add((T)(object)new User { Id = 2, Login = "operator", PasswordHash = BCrypt.Net.BCrypt.HashPassword("operator123"), Role = "Operator", FullName = "Оператор Іван" });
        }
        // Додай seed для Equipment, Customer, Rental при бажанні

        Save(filename, list);
        return list;
    }
}
