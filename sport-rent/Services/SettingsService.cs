using System.IO;
using System.Text.Json;
using sport_rent.Models;

namespace sport_rent.Services;

public class SettingsService
{
    public static SettingsService Instance { get; } = new();
    private const string FilePath = "Data/settings.json";

    public AppSettings Settings { get; private set; } = new();

    private SettingsService()
    {
        Load();
        LocalizationService.Instance.CurrentLang = Settings.Language;
    }

    private void Load()
    {
        Directory.CreateDirectory("Data");
        if (!File.Exists(FilePath)) return;
        try { Settings = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(FilePath)) ?? new(); }
        catch { Settings = new(); }
    }

    public void Save()
    {
        Settings.Language = LocalizationService.Instance.CurrentLang;
        Directory.CreateDirectory("Data");
        File.WriteAllText(FilePath, JsonSerializer.Serialize(Settings, new JsonSerializerOptions { WriteIndented = true }));
    }
}
