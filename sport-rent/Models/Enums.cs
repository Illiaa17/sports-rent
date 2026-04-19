namespace sport_rent.Models
{
    public class Enums
    {
        public enum UserRole
        {
            Admin,
            Operator
        }

        public enum EquipmentCondition
        {
            Good,       // Добре
            Damaged,    // Пошкоджено
            UnderRepair // На ремонті
        }

        public enum RentalStatus
        {
            Active,     // Активна
            Returned,   // Повернена
            Overdue     // Прострочена
        }
    }
}