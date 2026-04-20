using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using sport_rent.Services;

namespace sport_rent.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    [ObservableProperty] private object? currentView;

    public MainViewModel()
    {
        CurrentView = new DashboardViewModel();
    }

    [RelayCommand] private void ShowDashboard() => CurrentView = new DashboardViewModel();
    [RelayCommand] private void ShowEquipment() => CurrentView = new EquipmentViewModel();
    [RelayCommand] private void ShowCustomers() => CurrentView = new CustomerViewModel();
    [RelayCommand] private void ShowRentals() => CurrentView = new RentalViewModel();
    [RelayCommand] private void ShowSettings() => CurrentView = new SettingsViewModel();

    [RelayCommand]
    private void Logout()
    {
        var loginWindow = new LoginWindow();
        loginWindow.Show();
        App.Current?.Windows.FirstOrDefault()?.Close();
    }
}