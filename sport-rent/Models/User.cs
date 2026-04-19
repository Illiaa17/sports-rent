namespace sport_rent.Models
{
    public class User
    {
            public int Id { get; set; }
            public string Login { get; set; } = string.Empty;
            public string PasswordHash { get; set; } = string.Empty;
            public UserRole Role { get; set; }
            public string FullName { get; set; } = string.Empty;
            public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}