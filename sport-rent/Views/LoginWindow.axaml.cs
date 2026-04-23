using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using sport_rent.ViewModels;

namespace sport_rent.Views;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
        DataContext = new LoginViewModel();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}