using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;
using sport_rent.Models;
using sport_rent.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace sport_rent.Views;

public class ReturnDialog : Window
{
    private readonly Rental _rental;
    private readonly List<CheckBox> _damageChecks = new();
    private readonly DatePicker _returnDatePicker;
    private readonly TextBlock _overdueInfo;
    private readonly CheckBox _applyOverdueFine;
    private readonly CheckBox _applyDamageFine;
    private readonly TextBlock _damageInfo;
    private readonly TextBlock _previewText;
    private readonly List<Equipment> _equipment;

    public ReturnDialogResult? Result { get; private set; }

    public ReturnDialog(Rental rental, List<Equipment> equipment)
    {
        _rental = rental;
        _equipment = equipment;
        var loc = LocalizationService.Instance;
        var settings = SettingsService.Instance.Settings;

        Title = loc["ReturnDialogTitle"];
        Width = 480;
        MinWidth = 480;
        MinHeight = 320;
        SizeToContent = SizeToContent.Height;
        WindowStartupLocation = WindowStartupLocation.CenterOwner;
        CanResize = false;
        Padding = new Thickness(0);
        SystemDecorations = SystemDecorations.BorderOnly;
        this[!BackgroundProperty] = new DynamicResourceExtension("PrimaryBg");

        var header = new TextBlock
        {
            Text = string.Format(loc["ReturnDialogSubtitle"], _rental.Id, _rental.Customer?.FullName ?? "—"),
            FontSize = 16,
            FontWeight = FontWeight.SemiBold,
            Margin = new Thickness(0, 0, 0, 4)
        };
        header[!TextBlock.ForegroundProperty] = new DynamicResourceExtension("TextHi");

        var datesSection = new TextBlock
        {
            Text = loc["ReturnDatesSection"],
            FontSize = 13,
            FontWeight = FontWeight.SemiBold,
            Margin = new Thickness(0, 8, 0, 6)
        };
        datesSection[!TextBlock.ForegroundProperty] = new DynamicResourceExtension("TextHi");

        var dueLabel = new TextBlock
        {
            Text = loc["ReturnDueDateLabel"],
            Classes = { "form-label" }
        };

        var dueValue = new Border
        {
            MinHeight = 38,
            CornerRadius = new CornerRadius(8),
            BorderThickness = new Thickness(1),
            Padding = new Thickness(12, 8),
            Child = new TextBlock
            {
                Text = _rental.DueDate.ToString("dd.MM.yyyy"),
                FontSize = 13,
                FontWeight = FontWeight.SemiBold,
                VerticalAlignment = VerticalAlignment.Center
            }
        };
        dueValue[!Border.BackgroundProperty] = new DynamicResourceExtension("SurfaceAlt");
        dueValue[!Border.BorderBrushProperty] = new DynamicResourceExtension("Border");
        ((TextBlock)dueValue.Child)[!TextBlock.ForegroundProperty] = new DynamicResourceExtension("TextHi");

        var dueField = new StackPanel { Spacing = 2 };
        dueField.Children.Add(dueLabel);
        dueField.Children.Add(dueValue);

        var returnLabel = new TextBlock
        {
            Text = loc["ReturnDateLabel"],
            Classes = { "form-label" }
        };

        _returnDatePicker = new DatePicker
        {
            SelectedDate = DateTimeOffset.Now,
            MinHeight = 38,
            MinWidth = 300,
            HorizontalAlignment = HorizontalAlignment.Stretch
        };
        _returnDatePicker.PropertyChanged += (_, e) =>
        {
            if (e.Property == DatePicker.SelectedDateProperty)
                UpdateOverdueSection();
        };

        returnLabel.TextWrapping = TextWrapping.Wrap;

        var returnField = new StackPanel { Spacing = 2 };
        returnField.Children.Add(returnLabel);
        returnField.Children.Add(_returnDatePicker);

        var datesPanel = new StackPanel { Spacing = 12 };
        datesPanel.Children.Add(dueField);
        datesPanel.Children.Add(returnField);

        var itemsLabel = new TextBlock
        {
            Text = loc["ReturnCheckCondition"],
            FontSize = 13,
            FontWeight = FontWeight.SemiBold,
            Margin = new Thickness(0, 12, 0, 6)
        };
        itemsLabel[!TextBlock.ForegroundProperty] = new DynamicResourceExtension("TextHi");

        var itemsPanel = new StackPanel { Spacing = 8 };
        for (var i = 0; i < _rental.Items.Count; i++)
        {
            var item = _rental.Items[i];
            var eq = _equipment.FirstOrDefault(e => e.Id == item.EquipmentId);
            var name = item.Equipment?.Name ?? eq?.Name ?? item.EquipmentId.ToString();
            var condition = eq?.Condition ?? item.Equipment?.Condition ?? "—";

            var itemInfo = new TextBlock
            {
                Text = string.Format(loc["ReturnItemLine"], name, item.Quantity, condition),
                FontSize = 13,
                VerticalAlignment = VerticalAlignment.Center
            };
            itemInfo[!TextBlock.ForegroundProperty] = new DynamicResourceExtension("TextHi");

            var damageCb = new CheckBox
            {
                Content = loc["ReturnDamagedMark"],
                FontSize = 12,
                Margin = new Thickness(12, 0, 0, 0)
            };
            damageCb[!CheckBox.ForegroundProperty] = new DynamicResourceExtension("TextLo");
            damageCb.IsCheckedChanged += (_, _) => UpdateDamageSection();
            _damageChecks.Add(damageCb);

            var itemRow = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 4 };
            itemRow.Children.Add(itemInfo);
            itemRow.Children.Add(damageCb);
            itemsPanel.Children.Add(itemRow);
        }

        _overdueInfo = new TextBlock
        {
            FontSize = 13,
            Margin = new Thickness(0, 10, 0, 4),
            TextWrapping = TextWrapping.Wrap
        };
        _overdueInfo[!TextBlock.ForegroundProperty] = new DynamicResourceExtension("TextLo");

        _applyOverdueFine = new CheckBox
        {
            FontSize = 13,
            Margin = new Thickness(0, 0, 0, 0),
            IsEnabled = false
        };
        _applyOverdueFine[!CheckBox.ForegroundProperty] = new DynamicResourceExtension("TextHi");
        _applyOverdueFine.IsCheckedChanged += (_, _) => UpdatePreview();

        _damageInfo = new TextBlock
        {
            FontSize = 13,
            Margin = new Thickness(0, 10, 0, 4),
            TextWrapping = TextWrapping.Wrap
        };
        _damageInfo[!TextBlock.ForegroundProperty] = new DynamicResourceExtension("TextLo");

        _applyDamageFine = new CheckBox
        {
            FontSize = 13,
            IsEnabled = false
        };
        _applyDamageFine[!CheckBox.ForegroundProperty] = new DynamicResourceExtension("TextHi");
        _applyDamageFine.IsCheckedChanged += (_, _) => UpdatePreview();

        _previewText = new TextBlock
        {
            FontSize = 13,
            Margin = new Thickness(0, 12, 0, 0),
            TextWrapping = TextWrapping.Wrap
        };
        _previewText[!TextBlock.ForegroundProperty] = new DynamicResourceExtension("TextHi");

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
            Content = loc["ConfirmReturnBtn"],
            Width = 160,
            Height = 38,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            CornerRadius = new CornerRadius(8),
            Classes = { "primary" }
        };
        confirmBtn.Click += (_, _) =>
        {
            var damaged = GetDamagedIndexes();
            var returnDate = _returnDatePicker.SelectedDate?.DateTime ?? DateTime.Now;

            Result = new ReturnDialogResult
            {
                Confirmed = true,
                ReturnDate = returnDate,
                DamagedItemIndexes = damaged,
                ApplyOverdueFine = _applyOverdueFine.IsChecked == true,
                ApplyDamageFine = _applyDamageFine.IsChecked == true
            };
            Close(true);
        };

        var btnPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 10,
            HorizontalAlignment = HorizontalAlignment.Right,
            Margin = new Thickness(0, 16, 0, 0)
        };
        btnPanel.Children.Add(cancelBtn);
        btnPanel.Children.Add(confirmBtn);

        var layout = new StackPanel { Spacing = 4 };
        layout.Children.Add(header);
        layout.Children.Add(datesSection);
        layout.Children.Add(datesPanel);
        layout.Children.Add(itemsLabel);
        layout.Children.Add(itemsPanel);
        layout.Children.Add(_overdueInfo);
        layout.Children.Add(_applyOverdueFine);
        layout.Children.Add(_damageInfo);
        layout.Children.Add(_applyDamageFine);
        layout.Children.Add(_previewText);
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
        UpdateOverdueSection();
        UpdateDamageSection();
    }

    private DateTime GetReturnDate() =>
        (_returnDatePicker.SelectedDate ?? DateTimeOffset.Now).DateTime;

    private void UpdateOverdueSection()
    {
        var loc = LocalizationService.Instance;
        var settings = SettingsService.Instance.Settings;
        var returnDate = GetReturnDate();
        var overdueDays = FineService.CalculateOverdueDays(_rental.DueDate, returnDate);
        var overdueFine = FineService.CalculateOverdueFine(_rental.DueDate, returnDate, settings.FinePerDay);

        if (overdueDays > 0)
        {
            _overdueInfo.Text = string.Format(
                loc["ReturnOverdueInfo"],
                overdueDays,
                settings.FinePerDay.ToString("F2"),
                overdueFine.ToString("F2"));
            _applyOverdueFine.Content = string.Format(loc["ReturnApplyOverdueFine"], overdueFine.ToString("F2"));
            _applyOverdueFine.IsEnabled = true;
        }
        else
        {
            _overdueInfo.Text = loc["ReturnOnTime"];
            _applyOverdueFine.Content = string.Format(loc["ReturnApplyOverdueFine"], "0.00");
            _applyOverdueFine.IsEnabled = false;
            _applyOverdueFine.IsChecked = false;
        }

        UpdatePreview();
    }

    private void UpdateDamageSection()
    {
        var loc = LocalizationService.Instance;
        var settings = SettingsService.Instance.Settings;
        var damaged = GetDamagedIndexes();
        var damageFine = FineService.CalculateDamageFine(
            _rental, damaged, _equipment, settings.DamageFinePercent);

        if (damaged.Count > 0 && damageFine > 0)
        {
            _damageInfo.Text = string.Format(loc["ReturnDamageInfo"], damageFine.ToString("F2"));
            _applyDamageFine.Content = string.Format(loc["ReturnApplyDamageFine"], damageFine.ToString("F2"));
            _applyDamageFine.IsEnabled = true;
        }
        else
        {
            _damageInfo.Text = loc["ReturnNoDamageFine"];
            _applyDamageFine.Content = string.Format(loc["ReturnApplyDamageFine"], "0.00");
            _applyDamageFine.IsEnabled = false;
            _applyDamageFine.IsChecked = false;
        }

        UpdatePreview();
    }

    private void UpdatePreview()
    {
        var loc = LocalizationService.Instance;
        var settings = SettingsService.Instance.Settings;
        var returnDate = GetReturnDate();
        var damaged = GetDamagedIndexes();

        var overdueFine = _applyOverdueFine.IsChecked == true
            ? FineService.CalculateOverdueFine(_rental.DueDate, returnDate, settings.FinePerDay)
            : 0m;

        var damageFine = _applyDamageFine.IsChecked == true
            ? FineService.CalculateDamageFine(_rental, damaged, _equipment, settings.DamageFinePercent)
            : 0m;

        var baseAmount = FineService.BaseRentalAmount(_rental);
        var totalFines = overdueFine + damageFine;
        var grandTotal = baseAmount + totalFines;

        _previewText.Text = string.Format(
            loc["ReturnFinePreview"],
            overdueFine.ToString("F2"),
            damageFine.ToString("F2"),
            totalFines.ToString("F2"),
            grandTotal.ToString("F2"));
    }

    private List<int> GetDamagedIndexes()
    {
        var list = new List<int>();
        for (var i = 0; i < _damageChecks.Count; i++)
        {
            if (_damageChecks[i].IsChecked == true)
                list.Add(i);
        }
        return list;
    }
}
