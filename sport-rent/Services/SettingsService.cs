using System.IO;
using System.Text.Json;
using sport_rent.Models;

namespace sport_rent.Services;

/// <summary>
/// Сервіс для управління конфігурацією та глобальними налаштуваннями застосунку.
/// Забезпечує збереження та завантаження параметрів (мова, тема, штрафи) у файл settings.json.
/// </summary>
public class SettingsService
{
    /// <summary>
    /// Екземпляр сервісу для глобального доступу (патерн Singleton).
    /// </summary>
    public static SettingsService Instance { get; } = new();
    /// <summary>
    /// Шлях до файлу конфігурації.
    /// </summary>
    private const string FilePath = "Data/settings.json";

    /// <summary>
    /// Поточні налаштування застосунку.
    /// </summary>
    public AppSettings Settings { get; private set; } = new();

    /// <summary>
    /// Закритий конструктор для ініціалізації сервісу.
    /// Автоматично завантажує дані при першому зверненні до Instance.
    /// </summary>
    private SettingsService()
    {
        Load();
        LocalizationService.Instance.CurrentLang = Settings.Language;
    }

    /// <summary>
    /// Завантажує налаштування з JSON-файлу. 
    /// У разі відсутності файлу або помилки читання — ініціалізує параметри за замовчуванням.
    /// </summary>
    private void Load()
    {
        Directory.CreateDirectory("Data");
        if (!File.Exists(FilePath)) return;
        try { Settings = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(FilePath)) ?? new(); }
        catch { Settings = new(); }
    }

    /// <summary>
    /// Зберігає поточні параметри застосунку у JSON-файл.
    /// </summary>
    public void Save()
    {
        Settings.Language = LocalizationService.Instance.CurrentLang;
        Directory.CreateDirectory("Data");
        File.WriteAllText(FilePath, JsonSerializer.Serialize(Settings, new JsonSerializerOptions { WriteIndented = true }));
    }
}
