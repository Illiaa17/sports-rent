

using Avalonia;


using Avalonia.Markup.Xaml;

namespace sport_rent;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (Design.IsDesignMode)
            return;

        var loginWindow = new Views.LoginWindow();
        loginWindow.Show();
    }
}