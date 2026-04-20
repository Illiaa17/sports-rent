

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
        // Avalonia Fluent підтримує Light/Dark — можна розширити пізніше
    }
}