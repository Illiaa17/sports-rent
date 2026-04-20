using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using sport_rent.Services;
using Avalonia.Controls;

namespace sport_rent.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    [ObservableProperty] private string login = string.Empty;
    [ObservableProperty] private string password = string.Empty;
    [ObservableProperty] private string errorMessage = string.Empty;

    private readonly AuthService _authService = new();

    [RelayCommand]
    private void Login()
    {
        if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = LocalizationService.Instance["FillAllFields"];
            return;
        }

        if (_authService.Login(Login, Password))
        {
            var mainWindow = new MainWindow { DataContext = new MainViewModel() };
            mainWindow.Show();
            
            var currentWindow = App.Current?.Windows.FirstOrDefault(w => w is LoginWindow);
            currentWindow?.Close();
        }
        else
        {
            ErrorMessage = LocalizationService.Instance["InvalidCredentials"];
        }
    }
}