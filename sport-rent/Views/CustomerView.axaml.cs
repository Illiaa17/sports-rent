
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace sport_rent.Views;

public partial class CustomerView : UserControl
{
    public CustomerView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}