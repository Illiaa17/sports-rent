namespace sport_rent.Models;

public class AppSettings
{
    public decimal FinePerDay { get; set; } = 50m;
    public string Language { get; set; } = "uk";
    public string Theme { get; set; } = "Dark";
}
