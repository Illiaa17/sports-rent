using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using System.Collections;

namespace sport_rent.Controls;

public class FloatingLabelAutoComplete : TemplatedControl
{
    public static readonly StyledProperty<string> LabelProperty =
        AvaloniaProperty.Register<FloatingLabelAutoComplete, string>(nameof(Label), defaultValue: string.Empty);

    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<FloatingLabelAutoComplete, string?>(
            nameof(Text), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<IEnumerable?> ItemsSourceProperty =
        AvaloniaProperty.Register<FloatingLabelAutoComplete, IEnumerable?>(nameof(ItemsSource));

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

    public IEnumerable? ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property != TextProperty)
            return;

        if (!string.IsNullOrEmpty(Text))
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
