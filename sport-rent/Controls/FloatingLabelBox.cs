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

    public static readonly StyledProperty<bool> CompactProperty =
        AvaloniaProperty.Register<FloatingLabelBox, bool>(nameof(Compact));

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

    public bool Compact
    {
        get => GetValue(CompactProperty);
        set => SetValue(CompactProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        UpdateCompactPseudoClass();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == CompactProperty)
            UpdateCompactPseudoClass();

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

    private void UpdateCompactPseudoClass()
    {
        if (Compact)
        {
            if (!PseudoClasses.Contains(":compact"))
                PseudoClasses.Add(":compact");
        }
        else
        {
            PseudoClasses.Remove(":compact");
        }
    }
}
