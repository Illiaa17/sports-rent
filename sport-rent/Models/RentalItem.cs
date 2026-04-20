namespace sport_rent.Models;

public class RentalItem
{
    public int Id { get; set; }
    public int RentalId { get; set; }
    public int EquipmentId { get; set; }
    public int Quantity { get; set; }
    public decimal DailyRate { get; set; }
    public int Days { get; set; }
    public decimal Subtotal { get; set; }

    public Equipment? Equipment { get; set; }
}