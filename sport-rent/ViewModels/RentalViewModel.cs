using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using sport_rent.Models;
using sport_rent.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace sport_rent.ViewModels;

public partial class RentalViewModel : BaseViewModel
{
    private readonly JsonDataService _dataService = new();

    public ObservableCollection<Rental> Rentals { get; } = new();
    public ObservableCollection<Customer> Customers { get; } = new();
    public ObservableCollection<Equipment> EquipmentList { get; } = new();

    [ObservableProperty] private Rental selectedRental = new();
    [ObservableProperty] private RentalItem newRentalItem = new();
    [ObservableProperty] private Customer? selectedCustomer;
    [ObservableProperty] private Equipment? selectedEquipmentItem;
    [ObservableProperty] private DateTimeOffset newRentalDate = DateTimeOffset.Now;
    [ObservableProperty] private DateTimeOffset newDueDate = DateTimeOffset.Now.AddDays(7);
    [ObservableProperty] private decimal newItemQuantity = 1;
    [ObservableProperty] private decimal newItemDays = 1;

    public RentalViewModel()
    {
        LoadData();
    }

    partial void OnSelectedCustomerChanged(Customer? value)
    {
        SelectedRental.CustomerId = value?.Id ?? 0;
    }

    partial void OnSelectedEquipmentItemChanged(Equipment? value)
    {
        NewRentalItem.EquipmentId = value?.Id ?? 0;
    }

    private void LoadData()
    {
        var rentals = _dataService.Load<Rental>("rentals.json");
        Rentals.Clear();
        foreach (var r in rentals) Rentals.Add(r);

        Customers.Clear();
        foreach (var c in _dataService.Load<Customer>("customers.json")) Customers.Add(c);

        EquipmentList.Clear();
        foreach (var e in _dataService.Load<Equipment>("equipment.json")) EquipmentList.Add(e);
    }

    [RelayCommand]
    private void AddRentalItem()
    {
        if (SelectedEquipmentItem == null) return;

        NewRentalItem.EquipmentId = SelectedEquipmentItem.Id;
        NewRentalItem.Quantity = (int)NewItemQuantity;
        NewRentalItem.Days = (int)NewItemDays;
        NewRentalItem.DailyRate = SelectedEquipmentItem.DailyRate;
        NewRentalItem.Subtotal = NewRentalItem.DailyRate * NewRentalItem.Days * NewRentalItem.Quantity;

        SelectedRental.Items.Add(NewRentalItem);
        NewRentalItem = new RentalItem();
        SelectedEquipmentItem = null;
        NewItemQuantity = 1;
        NewItemDays = 1;
        OnPropertyChanged(nameof(SelectedRental));
    }

    [RelayCommand]
    private void SaveRental()
    {
        SelectedRental.RentalDate = NewRentalDate.DateTime;
        SelectedRental.DueDate = NewDueDate.DateTime;
        SelectedRental.TotalAmount = SelectedRental.Items.Sum(i => i.Subtotal);
        var list = Rentals.ToList();
        if (SelectedRental.Id == 0)
        {
            SelectedRental.Id = list.Any() ? list.Max(r => r.Id) + 1 : 1;
            Rentals.Add(SelectedRental);
        }
        _dataService.Save("rentals.json", list);
        SelectedRental = new Rental();
        SelectedCustomer = null;
        NewRentalDate = DateTimeOffset.Now;
        NewDueDate = DateTimeOffset.Now.AddDays(7);
        LoadData();
    }
}
