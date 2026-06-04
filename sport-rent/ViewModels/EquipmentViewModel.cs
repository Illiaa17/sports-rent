using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using sport_rent.Helpers;
using sport_rent.Models;
using sport_rent.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sport_rent.ViewModels;

public partial class EquipmentViewModel : BaseViewModel
{
    private readonly JsonDataService _dataService = new();
    private List<Equipment> _allEquipment = new();

    public ObservableCollection<EquipmentCardViewModel> EquipmentCards { get; } = new();

    [ObservableProperty] private Equipment selectedEquipment = new();
    [ObservableProperty] private string searchText = string.Empty;
    [ObservableProperty] private int currentPage = 1;
    [ObservableProperty] private int pageSize = 20;
    [ObservableProperty] private int totalPages = 1;
    [ObservableProperty] private Bitmap? selectedImage;
    [ObservableProperty] private string validationError = string.Empty;

    public List<int> PageSizes { get; } = new() { 20, 50, 100 };

    public ObservableCollection<string> CategoryOptions { get; } = new();
    public ObservableCollection<string> ConditionOptions { get; } = new();

    public string FormTitle => SelectedEquipment.Id == 0
        ? Loc["AddEquipmentTitle"]
        : string.Format(Loc["EditEquipmentTitle"], SelectedEquipment.Id);

    public bool HasNoData => EquipmentCards.Count == 0;

    public EquipmentViewModel()
    {
        EquipmentImageHelper.EnsureImagesDirectory();
        LocalizationService.Instance.LanguageChanged += OnEquipmentLanguageChanged;
        LoadEquipment();
    }

    private void OnEquipmentLanguageChanged()
    {
        NormalizeEquipmentConditions();
        if (!string.IsNullOrWhiteSpace(SelectedEquipment.Condition))
            SelectedEquipment.Condition = EquipmentConditionService.Normalize(SelectedEquipment.Condition);
        RebuildOptions();
    }

    private void LoadEquipment()
    {
        _allEquipment = _dataService.Load<Equipment>("equipment.json");
        NormalizeEquipmentConditions();
        ApplyFilter();
        RebuildOptions();
    }

    private void NormalizeEquipmentConditions()
    {
        foreach (var item in _allEquipment)
            item.Condition = EquipmentConditionService.Normalize(item.Condition);
    }

    private void RebuildOptions()
    {
        var cats = _allEquipment
            .Select(e => e.Category)
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(s => s);
        CategoryOptions.Clear();
        foreach (var c in cats) CategoryOptions.Add(c);

        var defaults = EquipmentConditionService.GetDefaultOptions();
        var conds = _allEquipment
            .Select(e => EquipmentConditionService.Normalize(e.Condition))
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Concat(defaults)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(s => s);
        ConditionOptions.Clear();
        foreach (var c in conds) ConditionOptions.Add(c);
    }

    private void ApplyFilter()
    {
        var filtered = string.IsNullOrWhiteSpace(SearchText)
            ? _allEquipment
            : _allEquipment.Where(e =>
                e.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                e.Category.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList();

        TotalPages = Math.Max(1, (int)Math.Ceiling(filtered.Count / (double)PageSize));
        if (CurrentPage > TotalPages) CurrentPage = TotalPages;

        var paged = filtered.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
        EquipmentCards.Clear();
        foreach (var item in paged)
            EquipmentCards.Add(new EquipmentCardViewModel(item));
        SyncCardSelection();
        OnPropertyChanged(nameof(HasNoData));
    }

    private void SyncCardSelection()
    {
        foreach (var card in EquipmentCards)
            card.IsSelected = SelectedEquipment.Id != 0 && card.Equipment.Id == SelectedEquipment.Id;
    }

    partial void OnSearchTextChanged(string value)
    {
        CurrentPage = 1;
        ApplyFilter();
    }

    partial void OnCurrentPageChanged(int value) => ApplyFilter();

    partial void OnPageSizeChanged(int value)
    {
        CurrentPage = 1;
        ApplyFilter();
    }

    partial void OnSelectedEquipmentChanged(Equipment value)
    {
        SelectedImage = EquipmentImageHelper.TryLoadBitmap(value.ImagePath);
        SyncCardSelection();
        OnPropertyChanged(nameof(FormTitle));
    }

    [RelayCommand]
    private void SelectEquipment(Equipment? equipment)
    {
        if (equipment == null) return;
        SelectedEquipment = equipment;
    }

    [RelayCommand]
    private void NewEquipment()
    {
        SelectedEquipment = new Equipment();
        SelectedImage = null;
        ValidationError = string.Empty;
    }

    public void SetImagePath(string path)
    {
        if (!File.Exists(path)) return;

        if (SelectedEquipment.Id > 0)
            SelectedEquipment.ImagePath = EquipmentImageHelper.PersistImage(path, SelectedEquipment.Id);
        else
            SelectedEquipment.ImagePath = path;

        SelectedImage = EquipmentImageHelper.TryLoadBitmap(SelectedEquipment.ImagePath);
        RefreshSelectedCardImage();
    }

    private void RefreshSelectedCardImage()
    {
        var card = EquipmentCards.FirstOrDefault(c => c.Equipment.Id == SelectedEquipment.Id);
        card?.ReloadImage();
    }

    private void PersistEquipmentImageIfNeeded()
    {
        var full = EquipmentImageHelper.ResolveFullPath(SelectedEquipment.ImagePath);
        if (full == null)
            return;

        var expectedJson = $"images/equipment/equipment_{SelectedEquipment.Id}{Path.GetExtension(full)}";
        if (string.Equals(SelectedEquipment.ImagePath, expectedJson, StringComparison.OrdinalIgnoreCase))
            return;

        SelectedEquipment.ImagePath = EquipmentImageHelper.PersistImage(full, SelectedEquipment.Id);
        SelectedImage = EquipmentImageHelper.TryLoadBitmap(SelectedEquipment.ImagePath);
    }

    [RelayCommand]
    private async Task SaveEquipment()
    {
        if (!await DialogHelper.ConfirmSaveAsync()) return;

        if (SelectedEquipment.Id == 0)
        {
            SelectedEquipment.Id = _allEquipment.Any() ? _allEquipment.Max(e => e.Id) + 1 : 1;
            _allEquipment.Add(SelectedEquipment);
        }
        else
        {
            var idx = _allEquipment.FindIndex(e => e.Id == SelectedEquipment.Id);
            if (idx >= 0) _allEquipment[idx] = SelectedEquipment;
        }

        PersistEquipmentImageIfNeeded();
        _dataService.Save("equipment.json", _allEquipment);
        LoadEquipment();
        SelectedEquipment = new Equipment();
        SelectedImage = null;
        ValidationError = string.Empty;
    }

    [RelayCommand]
    private async Task DeleteEquipment()
    {
        if (SelectedEquipment.Id == 0) return;
        if (!await DialogHelper.ConfirmDeleteAsync()) return;
        _allEquipment.RemoveAll(e => e.Id == SelectedEquipment.Id);
        _dataService.Save("equipment.json", _allEquipment);
        LoadEquipment();
        SelectedEquipment = new Equipment();
        SelectedImage = null;
    }

    [RelayCommand]
    private void PrevPage()
    {
        if (CurrentPage > 1) CurrentPage--;
    }

    [RelayCommand]
    private void NextPage()
    {
        if (CurrentPage < TotalPages) CurrentPage++;
    }

    [RelayCommand]
    private async Task ExportCsv()
    {
        var rows = _allEquipment.Select(e => new object?[]
        {
            e.Id, e.Name, e.Category,
            e.DailyRate.ToString("F2", System.Globalization.CultureInfo.InvariantCulture),
            e.DepositAmount.ToString("F2", System.Globalization.CultureInfo.InvariantCulture),
            e.Quantity, e.Condition
        });
        var csv = CsvHelper.BuildCsv(
            new[] { "ID", "Name", "Category", "DailyRate", "Deposit", "Quantity", "Condition" },
            rows);
        await CsvHelper.SaveCsvAsync("equipment_export.csv", csv);
    }
}
