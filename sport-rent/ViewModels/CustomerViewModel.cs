using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using sport_rent.Models;
using sport_rent.Services;
using System.Collections.ObjectModel;
using System.Linq;

namespace sport_rent.ViewModels;

public partial class CustomerViewModel : BaseViewModel
{
    private readonly JsonDataService _dataService = new();

    public ObservableCollection<Customer> Customers { get; } = new();

    [ObservableProperty] private Customer selectedCustomer = new();
    [ObservableProperty] private string searchText = string.Empty;

    public CustomerViewModel()
    {
        LoadCustomers();
    }

    private void LoadCustomers()
    {
        var list = _dataService.Load<Customer>("customers.json");
        Customers.Clear();
        foreach (var c in list) Customers.Add(c);
    }

    [RelayCommand]
    private void SaveCustomer()
    {
        var list = Customers.ToList();
        if (SelectedCustomer.Id == 0)
        {
            SelectedCustomer.Id = list.Any() ? list.Max(c => c.Id) + 1 : 1;
            Customers.Add(SelectedCustomer);
        }
        _dataService.Save("customers.json", list);
        LoadCustomers();
        SelectedCustomer = new Customer();
    }

    [RelayCommand]
    private void DeleteCustomer()
    {
        if (SelectedCustomer.Id == 0) return;
        var list = Customers.ToList();
        list.RemoveAll(c => c.Id == SelectedCustomer.Id);
        _dataService.Save("customers.json", list);
        LoadCustomers();
    }
}