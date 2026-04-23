using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using sport_rent.Helpers;
using sport_rent.Models;
using sport_rent.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace sport_rent.ViewModels;

public partial class CustomerViewModel : BaseViewModel
{
    private readonly JsonDataService _dataService = new();
    private List<Customer> _allCustomers = new();

    public ObservableCollection<Customer> Customers { get; } = new();

    [ObservableProperty] private Customer selectedCustomer = new();
    [ObservableProperty] private string searchText = string.Empty;
    [ObservableProperty] private string validationError = string.Empty;

    public string FormTitle => SelectedCustomer.Id == 0
        ? Loc["AddCustomerTitle"]
        : string.Format(Loc["EditCustomerTitle"], SelectedCustomer.Id);

    public bool HasNoData => Customers.Count == 0;

    public CustomerViewModel()
    {
        LoadCustomers();
    }

    partial void OnSelectedCustomerChanged(Customer value)
    {
        OnPropertyChanged(nameof(FormTitle));
    }

    [RelayCommand]
    private void NewCustomer()
    {
        SelectedCustomer = new Customer();
        ValidationError = string.Empty;
    }

    private void LoadCustomers()
    {
        _allCustomers = _dataService.Load<Customer>("customers.json");
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        Customers.Clear();
        var filtered = string.IsNullOrWhiteSpace(SearchText)
            ? _allCustomers
            : _allCustomers.Where(c =>
                c.FullName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                c.Phone.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList();
        foreach (var c in filtered) Customers.Add(c);
        OnPropertyChanged(nameof(HasNoData));
    }

    partial void OnSearchTextChanged(string value) => ApplyFilter();

    private bool Validate()
    {
        var loc = LocalizationService.Instance;
        if (!Regex.IsMatch(SelectedCustomer.Phone, @"^\d{10}$"))
        {
            ValidationError = loc["InvalidPhone"];
            return false;
        }
        if (!Regex.IsMatch(SelectedCustomer.PassportNumber, @"^[\p{L}]{2}\d{6}$"))
        {
            ValidationError = loc["InvalidPassport"];
            return false;
        }
        ValidationError = string.Empty;
        return true;
    }

    [RelayCommand]
    private async Task SaveCustomer()
    {
        if (!Validate()) return;
        if (!await DialogHelper.ConfirmSaveAsync()) return;

        if (SelectedCustomer.Id == 0)
        {
            SelectedCustomer.Id = _allCustomers.Any() ? _allCustomers.Max(c => c.Id) + 1 : 1;
            _allCustomers.Add(SelectedCustomer);
        }
        else
        {
            var idx = _allCustomers.FindIndex(c => c.Id == SelectedCustomer.Id);
            if (idx >= 0) _allCustomers[idx] = SelectedCustomer;
        }
        _dataService.Save("customers.json", _allCustomers);
        LoadCustomers();
        SelectedCustomer = new Customer();
        ValidationError = string.Empty;
    }

    [RelayCommand]
    private async Task DeleteCustomer()
    {
        if (SelectedCustomer.Id == 0) return;
        if (!await DialogHelper.ConfirmDeleteAsync()) return;
        _allCustomers.RemoveAll(c => c.Id == SelectedCustomer.Id);
        _dataService.Save("customers.json", _allCustomers);
        LoadCustomers();
        SelectedCustomer = new Customer();
    }
}
