using Avalonia;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using sport_rent.Services;

namespace sport_rent.ViewModels;

public partial class SettingsViewModel : BaseViewModel
{
    [ObservableProperty] private string currentLanguage = LocalizationService.Instance.CurrentLang;
    [ObservableProperty] private decimal finePerDay = SettingsService.Instance.Settings.FinePerDay;

    partial void OnFinePerDayChanged(decimal value)
    {
        SettingsService.Instance.Settings.FinePerDay = value;
        SettingsService.Instance.Save();
    }

    [RelayCommand]
    private void ChangeLanguage(string lang)
    {
        LocalizationService.Instance.CurrentLang = lang;
        CurrentLanguage = lang;
        SettingsService.Instance.Save();
    }

    [RelayCommand]
    private void ToggleTheme()
    {
        var app = Application.Current;
        if (app == null) return;
        var next = app.RequestedThemeVariant == ThemeVariant.Dark
            ? ThemeVariant.Light
            : ThemeVariant.Dark;
        app.RequestedThemeVariant = next;
        SettingsService.Instance.Settings.Theme = next == ThemeVariant.Dark ? "Dark" : "Light";
        SettingsService.Instance.Save();
    }
}
