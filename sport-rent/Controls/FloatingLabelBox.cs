using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Data;

namespace sport_rent.Controls;

public class FloatingLabelBox : TemplatedControl
{
    public static readonly StyledProperty<string> LabelProperty =
        AvaloniaProperty.Register<FloatingLabelBox, string>(nameof(Label), defaultValue: string.Empty);

    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<FloatingLabelBox, string?>(
            nameof(Text), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<char> PasswordCharProperty =
        AvaloniaProperty.Register<FloatingLabelBox, char>(nameof(PasswordChar), defaultValue: '\0');

    public string Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public char PasswordChar
    {
        get => GetValue(PasswordCharProperty);
        set => SetValue(PasswordCharProperty, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == TextProperty)
        {
            var hasText = !string.IsNullOrEmpty(Text);
            if (hasText)
            {
                if (!PseudoClasses.Contains(":hastext"))
                    PseudoClasses.Add(":hastext");
            }
            else
            {
                PseudoClasses.Remove(":hastext");
            }
        }
    }
}
