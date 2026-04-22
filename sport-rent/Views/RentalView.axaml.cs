
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace sport_rent.Views;

public partial class RentalView : UserControl
{
    public RentalView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}