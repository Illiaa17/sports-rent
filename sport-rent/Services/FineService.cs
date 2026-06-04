using sport_rent.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace sport_rent.Services;


public static class FineService
{
    public static int CalculateOverdueDays(DateTime dueDate, DateTime returnDate)
    {
        var days = (returnDate.Date - dueDate.Date).Days;
        return Math.Max(0, days);
    }

    public static decimal CalculateOverdueFine(DateTime dueDate, DateTime returnDate, decimal finePerDay)
    {
        return CalculateOverdueDays(dueDate, returnDate) * finePerDay;
    }

    public static decimal CalculateDamageFine(
        Rental rental,
        IReadOnlyCollection<int> damagedItemIndexes,
        IReadOnlyList<Equipment> equipment,
        decimal damageFinePercent)
    {
        if (damagedItemIndexes.Count == 0 || damageFinePercent <= 0)
            return 0m;

        decimal total = 0;
        for (var i = 0; i < rental.Items.Count; i++)
        {
            if (!damagedItemIndexes.Contains(i))
                continue;

            var item = rental.Items[i];
            var eq = equipment.FirstOrDefault(e => e.Id == item.EquipmentId);
            var deposit = eq?.DepositAmount ?? 0;
            total += item.Quantity * deposit * (damageFinePercent / 100m);
        }

        return total;
    }

    public static string DamagedConditionLabel() =>
        EquipmentConditionService.Normalize("Damaged");

    public static decimal BaseRentalAmount(Rental rental) =>
        rental.Items.Sum(i => i.Subtotal);
}
