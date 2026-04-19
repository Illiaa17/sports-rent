namespace sport_rent.Models
{
    public class Rental
    {
            public int Id { get; set; }
            public int CustomerId { get; set; }
            public int UserId { get; set; }           
            public DateTime RentalDate { get; set; }
            public DateTime DueDate { get; set; }     
            public DateTime? ReturnDate { get; set; } 
            public RentalStatus Status { get; set; }
            public decimal TotalAmount { get; set; }
            public decimal DepositPaid { get; set; }
            
            public List<RentalItem> Items { get; set; } = new();
    }
}