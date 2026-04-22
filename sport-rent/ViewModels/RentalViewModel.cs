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

public partial class RentalViewModel : BaseViewModel
{
    private readonly JsonDataService _dataService = new();
    private List<Rental> _allRentals = new();

    public ObservableCollection<Rental> PagedRentals { get; } = new();
    public ObservableCollection<Customer> Customers { get; } = new();
    public ObservableCollection<Equipment> EquipmentList { get; } = new();
    public ObservableCollection<RentalItem> CurrentItems { get; } = new();

    [ObservableProperty] private Rental selectedRental = new();
    [ObservableProperty] private RentalItem newRentalItem = new();
    [ObservableProperty] private Customer? selectedCustomer;
    [ObservableProperty] private Equipment? selectedEquipmentItem;
    [ObservableProperty] private DateTimeOffset newRentalDate = DateTimeOffset.Now;
    [ObservableProperty] private DateTimeOffset newDueDate = DateTimeOffset.Now.AddDays(7);
    [ObservableProperty] private decimal newItemQuantity = 1;
    [ObservableProperty] private decimal newItemDays = 1;
    [ObservableProperty] private string validationError = string.Empty;
    [ObservableProperty] private string searchText = string.Empty;
    [ObservableProperty] private string selectedStatus = "All";
    [ObservableProperty] private int currentPage = 1;
    [ObservableProperty] private int pageSize = 20;
    [ObservableProperty] private int totalPages = 1;

    public List<string> StatusFilters { get; } = new() { "All", "Active", "Returned", "Overdue" };
    public List<int> PageSizes { get; } = new() { 20, 50, 100 };

    public string FormTitle => Loc["NewRentalTitle"];
    public bool HasNoData => PagedRentals.Count == 0;

    public RentalViewModel()
    {
        LoadData();
    }

    [RelayCommand]
    private async Task NewRental()
    {
        var hasData = (SelectedRental.Items?.Count ?? 0) > 0 || SelectedCustomer != null;
        if (hasData && !await DialogHelper.ConfirmClearAsync()) return;

        SelectedRental = new Rental();
        CurrentItems.Clear();
        SelectedCustomer = null;
        SelectedEquipmentItem = null;
        NewRentalDate = DateTimeOffset.Now;
        NewDueDate = DateTimeOffset.Now.AddDays(7);
        NewItemQuantity = 1;
        NewItemDays = 1;
        ValidationError = string.Empty;
    }

    partial void OnSelectedCustomerChanged(Customer? value)
        => SelectedRental.CustomerId = value?.Id ?? 0;

    partial void OnSelectedRentalChanged(Rental value)
    {
        CurrentItems.Clear();
        foreach (var item in value.Items)
            CurrentItems.Add(item);
    }

    partial void OnSearchTextChanged(string value) { CurrentPage = 1; ApplyFilter(); }
    partial void OnSelectedStatusChanged(string value) { CurrentPage = 1; ApplyFilter(); }
    partial void OnCurrentPageChanged(int value) => ApplyPaging();
    partial void OnPageSizeChanged(int value) { CurrentPage = 1; ApplyPaging(); }

    private void LoadData()
    {
        _allRentals = _dataService.Load<Rental>("rentals.json");
        var customers = _dataService.Load<Customer>("customers.json");
        var equipment = _dataService.Load<Equipment>("equipment.json");

        // Overdue auto-detection
        bool changed = false;
        foreach (var r in _allRentals.Where(r => r.Status == "Active" && r.DueDate < DateTime.Now))
        {
            r.Status = "Overdue";
            changed = true;
        }
        if (changed) _dataService.Save("rentals.json", _allRentals);

        // Enrich with navigation properties
        foreach (var r in _allRentals)
        {
            r.Customer = customers.FirstOrDefault(c => c.Id == r.CustomerId);
            foreach (var item in r.Items)
                item.Equipment = equipment.FirstOrDefault(e => e.Id == item.EquipmentId);
        }

        Customers.Clear();
        foreach (var c in customers) Customers.Add(c);

        EquipmentList.Clear();
        foreach (var e in equipment) EquipmentList.Add(e);

        ApplyFilter();
    }

    private List<Rental> GetFiltered()
    {
        var filtered = _allRentals.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(SearchText))
            filtered = filtered.Where(r =>
                (r.Customer?.FullName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true) ||
                r.Id.ToString().Contains(SearchText));

        if (SelectedStatus != "All" && !string.IsNullOrEmpty(SelectedStatus))
            filtered = filtered.Where(r => r.Status == SelectedStatus);

        return filtered.ToList();
    }

    private void ApplyFilter()
    {
        var list = GetFiltered();
        TotalPages = Math.Max(1, (int)Math.Ceiling(list.Count / (double)PageSize));
        if (CurrentPage > TotalPages) CurrentPage = TotalPages;
        ApplyPagingFromList(list);
    }

    private void ApplyPaging()
    {
        var list = GetFiltered();
        ApplyPagingFromList(list);
    }

    private void ApplyPagingFromList(List<Rental> list)
    {
        var paged = list.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
        PagedRentals.Clear();
        foreach (var r in paged) PagedRentals.Add(r);
        OnPropertyChanged(nameof(HasNoData));
    }

    [RelayCommand] private void PrevPage() { if (CurrentPage > 1) CurrentPage--; }
    [RelayCommand] private void NextPage() { if (CurrentPage < TotalPages) CurrentPage++; }

    [RelayCommand]
    private void AddRentalItem()
    {
        var loc = LocalizationService.Instance;
        if (SelectedEquipmentItem == null) { ValidationError = loc["SelectEquipment"]; return; }

        var eq = EquipmentList.FirstOrDefault(e => e.Id == SelectedEquipmentItem.Id);
        if (eq == null || eq.Quantity < (int)NewItemQuantity)
        {
            ValidationError = loc["InsufficientQty"];
            return;
        }

        var item = new RentalItem
        {
            EquipmentId = SelectedEquipmentItem.Id,
            Quantity = (int)NewItemQuantity,
            Days = (int)NewItemDays,
            DailyRate = SelectedEquipmentItem.DailyRate,
            Equipment = SelectedEquipmentItem
        };
        item.Subtotal = item.DailyRate * item.Days * item.Quantity;

        SelectedRental.Items.Add(item);
        CurrentItems.Add(item);
        ValidationError = string.Empty;
        SelectedEquipmentItem = null;
        NewItemQuantity = 1;
        NewItemDays = 1;
    }

    [RelayCommand]
    private async Task SaveRental()
    {
        var loc = LocalizationService.Instance;
        if (SelectedCustomer == null) { ValidationError = loc["SelectCustomer"]; return; }
        if (!SelectedRental.Items.Any()) { ValidationError = loc["AddItemFirst"]; return; }
        if (NewDueDate <= NewRentalDate) { ValidationError = loc["InvalidDateRange"]; return; }

        if (!await DialogHelper.ConfirmSaveAsync()) return;

        SelectedRental.RentalDate = NewRentalDate.DateTime;
        SelectedRental.DueDate = NewDueDate.DateTime;
        SelectedRental.CustomerId = SelectedCustomer.Id;
        SelectedRental.Status = "Active";
        SelectedRental.TotalAmount = SelectedRental.Items.Sum(i => i.Subtotal);

        // Decrement equipment quantities
        var allEquipment = _dataService.Load<Equipment>("equipment.json");
        foreach (var item in SelectedRental.Items)
        {
            var eq = allEquipment.FirstOrDefault(e => e.Id == item.EquipmentId);
            if (eq != null) eq.Quantity = Math.Max(0, eq.Quantity - item.Quantity);
        }
        _dataService.Save("equipment.json", allEquipment);

        if (SelectedRental.Id == 0)
        {
            SelectedRental.Id = _allRentals.Any() ? _allRentals.Max(r => r.Id) + 1 : 1;
            _allRentals.Add(SelectedRental);
        }
        _dataService.Save("rentals.json", _allRentals);

        SelectedRental = new Rental();
        CurrentItems.Clear();
        SelectedCustomer = null;
        NewRentalDate = DateTimeOffset.Now;
        NewDueDate = DateTimeOffset.Now.AddDays(7);
        ValidationError = string.Empty;
        LoadData();
    }

    [RelayCommand]
    private async Task ReturnRental(Rental rental)
    {
        if (!await DialogHelper.ConfirmReturnAsync()) return;

        rental.ReturnDate = DateTime.Now;
        rental.Status = "Returned";

        if (rental.ReturnDate > rental.DueDate)
        {
            var overdueDays = (int)Math.Ceiling((rental.ReturnDate.Value - rental.DueDate).TotalDays);
            rental.TotalAmount += overdueDays * SettingsService.Instance.Settings.FinePerDay;
        }

        // Restore equipment quantities
        var allEquipment = _dataService.Load<Equipment>("equipment.json");
        foreach (var item in rental.Items)
        {
            var eq = allEquipment.FirstOrDefault(e => e.Id == item.EquipmentId);
            if (eq != null) eq.Quantity += item.Quantity;
        }
        _dataService.Save("equipment.json", allEquipment);
        _dataService.Save("rentals.json", _allRentals);
        LoadData();
    }

    [RelayCommand]
    private async Task DeleteRental(Rental? rental)
    {
        var target = rental ?? SelectedRental;
        if (target.Id == 0) return;
        if (!await DialogHelper.ConfirmDeleteAsync()) return;
        _allRentals.RemoveAll(r => r.Id == target.Id);
        _dataService.Save("rentals.json", _allRentals);
        LoadData();
    }

    [RelayCommand]
    private async Task ExportCsv()
    {
        var rows = _allRentals.Select(r => new object?[]
        {
            r.Id,
            r.Customer?.FullName,
            r.RentalDate.ToString("yyyy-MM-dd"),
            r.DueDate.ToString("yyyy-MM-dd"),
            r.ReturnDate?.ToString("yyyy-MM-dd") ?? string.Empty,
            r.Status,
            r.TotalAmount.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)
        });
        var csv = CsvHelper.BuildCsv(
            new[] { "ID", "Customer", "RentalDate", "DueDate", "ReturnDate", "Status", "TotalAmount" },
            rows);
        await CsvHelper.SaveCsvAsync("rentals_export.csv", csv);
    }
}
