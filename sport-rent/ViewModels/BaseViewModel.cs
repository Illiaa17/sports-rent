using CommunityToolkit.Mvvm.ComponentModel;
using sport_rent.Services;

namespace sport_rent.ViewModels;

public abstract partial class BaseViewModel : ObservableObject
{
    public LocalizationService Loc => LocalizationService.Instance;

    protected BaseViewModel()
    {
        LocalizationService.Instance.LanguageChanged += OnLanguageChanged;
    }

    private void OnLanguageChanged()
    {
        OnPropertyChanged(string.Empty);
    }
}
