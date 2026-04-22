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
        else if (typeof(T) == typeof(Equipment))
        {
            list.Add((T)(object)new Equipment { Id = 1, Name = "Велосипед гірський", Category = "Велосипеди", DailyRate = 150m, DepositAmount = 1000m, Quantity = 5, Condition = "Відмінний" });
            list.Add((T)(object)new Equipment { Id = 2, Name = "Лижі гірські", Category = "Лижне спорядження", DailyRate = 200m, DepositAmount = 2000m, Quantity = 4, Condition = "Добрий" });
            list.Add((T)(object)new Equipment { Id = 3, Name = "Сноуборд", Category = "Сноубординг", DailyRate = 180m, DepositAmount = 1500m, Quantity = 3, Condition = "Добрий" });
            list.Add((T)(object)new Equipment { Id = 4, Name = "Намет туристичний", Category = "Кемпінг", DailyRate = 100m, DepositAmount = 800m, Quantity = 6, Condition = "Відмінний" });
            list.Add((T)(object)new Equipment { Id = 5, Name = "Скейтборд", Category = "Скейтинг", DailyRate = 80m, DepositAmount = 500m, Quantity = 8, Condition = "Добрий" });
        }
        else if (typeof(T) == typeof(Customer))
        {
            list.Add((T)(object)new Customer { Id = 1, FullName = "Іваненко Олексій Петрович", Phone = "0501234567", PassportNumber = "АА123456" });
            list.Add((T)(object)new Customer { Id = 2, FullName = "Петренко Марія Іванівна", Phone = "0677654321", PassportNumber = "АВ654321" });
            list.Add((T)(object)new Customer { Id = 3, FullName = "Сидоренко Дмитро Олегович", Phone = "0939876543", PassportNumber = "ВА987654" });
        }

        Save(filename, list);
        return list;
    }
}
