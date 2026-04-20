using CommunityToolkit.Mvvm.ComponentModel;
using sport_rent.Services;
using System.Collections.Generic;

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
        TotalEquipment = _dataService.Load<Models.Equipment>("equipment.json").Count;
        ActiveRentals = _dataService.Load<Models.Rental>("rentals.json").Count(r => r.Status == "Active");
        TotalCustomers = _dataService.Load<Models.Customer>("customers.json").Count;
    }
}