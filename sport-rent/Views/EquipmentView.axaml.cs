using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace sport_rent.Views;

public partial class EquipmentView : UserControl
{
    public EquipmentView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}