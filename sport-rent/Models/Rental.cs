using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace sport_rent.Models;

public class Rental
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int UserId { get; set; }
    public DateTime RentalDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public string Status { get; set; } = "Active"; 
    public decimal TotalAmount { get; set; }
    public decimal DepositPaid { get; set; }
    public decimal OverdueFine { get; set; }
    public decimal DamageFine { get; set; }
    public decimal ManualFine { get; set; }

    [JsonIgnore] public Customer? Customer { get; set; }

    [JsonIgnore]
    public decimal FinesTotal => OverdueFine + DamageFine + ManualFine;

    [JsonIgnore]
    public bool CanReturn => Status is "Active" or "Overdue";
    public List<RentalItem> Items { get; set; } = new();

    [JsonIgnore]
    public string ItemsSummary => Items.Count == 0
        ? string.Empty
        : string.Join(", ", Items.Select(i => $"{i.Equipment?.Name ?? i.EquipmentId.ToString()} ×{i.Quantity}"));
}