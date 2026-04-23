
using System;

namespace sport_rent.Models;

public class Equipment
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal DailyRate { get; set; }
    public decimal DepositAmount { get; set; }
    public int Quantity { get; set; }
    public string Condition { get; set; } = "Good";
    public string ImagePath { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}