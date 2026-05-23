namespace sport_rent.Models;

public class AppSettings
{
    public decimal FinePerDay { get; set; } = 50m;
    /// <summary>Відсоток застави за одну пошкоджену одиницю.</summary>
    public decimal DamageFinePercent { get; set; } = 100m;
    public string Language { get; set; } = "uk";
    public string Theme { get; set; } = "Dark";
}
