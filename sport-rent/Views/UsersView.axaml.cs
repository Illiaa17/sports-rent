using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace sport_rent.Views;

public partial class UsersView : UserControl
{
    public UsersView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
