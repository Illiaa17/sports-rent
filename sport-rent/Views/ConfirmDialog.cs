using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;
using sport_rent.Services;

namespace sport_rent.Views;

public class ConfirmDialog : Window
{
    public ConfirmDialog(string title, string message)
    {
        Title = title;
        Width = 460;
        Height = 220;
        WindowStartupLocation = WindowStartupLocation.CenterOwner;
        CanResize = false;
        Padding = new Thickness(0);
        SystemDecorations = SystemDecorations.BorderOnly;

        this[!BackgroundProperty] = new DynamicResourceExtension("PrimaryBg");

        var loc = LocalizationService.Instance;

        var header = new TextBlock
        {
            Text = title,
            FontSize = 18,
            FontWeight = FontWeight.SemiBold,
            Margin = new Thickness(0, 0, 0, 8)
        };
        header[!TextBlock.ForegroundProperty] = new DynamicResourceExtension("TextHi");

        var msg = new TextBlock
        {
            Text = message,
            TextWrapping = TextWrapping.Wrap,
            VerticalAlignment = VerticalAlignment.Center,
            FontSize = 14
        };
        msg[!TextBlock.ForegroundProperty] = new DynamicResourceExtension("TextLo");

        var cancelBtn = new Button
        {
            Content = loc["CancelBtn"],
            Width = 120,
            Height = 38,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            CornerRadius = new CornerRadius(8),
            Classes = { "ghost" }
        };
        cancelBtn.Click += (_, _) => Close(false);

        var confirmBtn = new Button
        {
            Content = loc["ConfirmBtn"],
            Width = 120,
            Height = 38,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            CornerRadius = new CornerRadius(8),
            Classes = { "primary" }
        };
        confirmBtn.Click += (_, _) => Close(true);

        var btnPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 10,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        btnPanel.Children.Add(cancelBtn);
        btnPanel.Children.Add(confirmBtn);

        var layout = new StackPanel { Spacing = 14 };
        layout.Children.Add(header);
        layout.Children.Add(msg);
        layout.Children.Add(btnPanel);

        var card = new Border
        {
            Margin = new Thickness(20),
            Padding = new Thickness(24),
            CornerRadius = new CornerRadius(12),
            BorderThickness = new Thickness(1),
            Classes = { "card" },
            Child = layout
        };
        card[!Border.BackgroundProperty] = new DynamicResourceExtension("SurfaceBg");
        card[!Border.BorderBrushProperty] = new DynamicResourceExtension("Border");

        Content = card;
    }
}
