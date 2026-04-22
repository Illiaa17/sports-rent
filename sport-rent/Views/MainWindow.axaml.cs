using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using sport_rent.ViewModels;

namespace sport_rent.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}