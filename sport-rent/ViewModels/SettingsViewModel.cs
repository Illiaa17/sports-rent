using Avalonia;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using sport_rent.Services;

namespace sport_rent.ViewModels;

public partial class SettingsViewModel : BaseViewModel
{
    [ObservableProperty] private string currentLanguage = LocalizationService.Instance.CurrentLang;

    [RelayCommand]
    private void ChangeLanguage(string lang)
    {
        LocalizationService.Instance.CurrentLang = lang;
        CurrentLanguage = lang;
        // Оновлення UI відбувається через подію
    }

    [RelayCommand]
    private void ToggleTheme()
    {
        var app = Application.Current;
        if (app == null) return;
        app.RequestedThemeVariant = app.RequestedThemeVariant == ThemeVariant.Dark
            ? ThemeVariant.Light
            : ThemeVariant.Dark;
    }
}