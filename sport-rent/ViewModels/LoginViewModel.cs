using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using sport_rent.Services;
using sport_rent.Views;

namespace sport_rent.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    [ObservableProperty] private string loginName = string.Empty;
    [ObservableProperty] private string password = string.Empty;
    [ObservableProperty] private string errorMessage = string.Empty;

    private readonly AuthService _authService = new();

    [RelayCommand]
    private void Login()
    {
        if (string.IsNullOrWhiteSpace(LoginName) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = LocalizationService.Instance["FillAllFields"];
            return;
        }

        if (_authService.Login(LoginName, Password))
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mainWindow = new MainWindow { DataContext = new MainViewModel() };
                desktop.MainWindow = mainWindow;
                mainWindow.Show();
                desktop.Windows.OfType<LoginWindow>().FirstOrDefault()?.Close();
            }
        }
        else
        {
            ErrorMessage = LocalizationService.Instance["InvalidCredentials"];
        }
    }
}