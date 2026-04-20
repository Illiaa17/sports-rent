using System;
using System.Collections.Generic;

namespace sport_rent.Models;

public class Rental
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int UserId { get; set; }
    public DateTime RentalDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public string Status { get; set; } = "Active"; // Active, Returned, Overdue
    public decimal TotalAmount { get; set; }
    public decimal DepositPaid { get; set; }

    public Customer? Customer { get; set; }
    public List<RentalItem> Items { get; set; } = new();
}