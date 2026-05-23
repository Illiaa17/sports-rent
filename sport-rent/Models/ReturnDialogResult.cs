using System;
using System.Collections.Generic;

namespace sport_rent.Models;

public class ReturnDialogResult
{
    public bool Confirmed { get; init; }
    public DateTime ReturnDate { get; init; }
    public List<int> DamagedItemIndexes { get; init; } = new();
    public bool ApplyOverdueFine { get; init; }
    public bool ApplyDamageFine { get; init; }
}
