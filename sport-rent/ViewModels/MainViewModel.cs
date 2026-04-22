using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using sport_rent.Helpers;
using sport_rent.Services;
using sport_rent.Views;

namespace sport_rent.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    [ObservableProperty] private object? currentView;

    public bool IsAdmin => AuthService.Instance.IsAdmin;

    public MainViewModel()
    {
        CurrentView = new DashboardViewModel();
    }

    [RelayCommand] private void ShowDashboard() => CurrentView = new DashboardViewModel();
    [RelayCommand] private void ShowEquipment() => CurrentView = new EquipmentViewModel();
    [RelayCommand] private void ShowCustomers() => CurrentView = new CustomerViewModel();
    [RelayCommand] private void ShowRentals() => CurrentView = new RentalViewModel();
    [RelayCommand] private void ShowSettings() => CurrentView = new SettingsViewModel();
    [RelayCommand] private void ShowUsers() => CurrentView = new UsersViewModel();

    [RelayCommand]
    private async Task Logout()
    {
        if (!await DialogHelper.ConfirmLogoutAsync()) return;

        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var loginWindow = new LoginWindow();
            desktop.MainWindow = loginWindow;
            loginWindow.Show();
            desktop.Windows.OfType<MainWindow>().FirstOrDefault()?.Close();
        }
    }
}
