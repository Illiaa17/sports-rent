using CommunityToolkit.Mvvm.ComponentModel;
using sport_rent.Models;
using sport_rent.Services;
using System.Linq;

namespace sport_rent.ViewModels;

public partial class DashboardViewModel : BaseViewModel
{
    private readonly JsonDataService _dataService = new();

    [ObservableProperty] private int totalEquipment;
    [ObservableProperty] private int activeRentals;
    [ObservableProperty] private int totalCustomers;
    [ObservableProperty] private decimal todayRevenue;

    public DashboardViewModel()
    {
        LoadStats();
    }

    private void LoadStats()
    {
        TotalEquipment = _dataService.Load<Equipment>("equipment.json").Count;
        var rentals = _dataService.Load<Rental>("rentals.json");
        ActiveRentals = rentals.Count(r => r.Status == "Active" || r.Status == "Overdue");
        TotalCustomers = _dataService.Load<Customer>("customers.json").Count;
        TodayRevenue = rentals
            .Where(r => r.Status == "Returned" && r.ReturnDate?.Date == System.DateTime.Today)
            .Sum(r => r.TotalAmount);
    }
}
