using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using sport_rent.Models;
using sport_rent.Services;
using System.Collections.ObjectModel;
using System.Linq;

namespace sport_rent.ViewModels;

public partial class EquipmentViewModel : BaseViewModel
{
    private readonly JsonDataService _dataService = new();

    public ObservableCollection<Equipment> EquipmentList { get; } = new();

    [ObservableProperty] private Equipment selectedEquipment = new();
    [ObservableProperty] private string searchText = string.Empty;

    public EquipmentViewModel()
    {
        LoadEquipment();
    }

    private void LoadEquipment()
    {
        var list = _dataService.Load<Equipment>("equipment.json");
        EquipmentList.Clear();
        foreach (var item in list) EquipmentList.Add(item);
    }

    [RelayCommand]
    private void SaveEquipment()
    {
        var list = EquipmentList.ToList();
        if (SelectedEquipment.Id == 0)
        {
            SelectedEquipment.Id = list.Any() ? list.Max(e => e.Id) + 1 : 1;
            EquipmentList.Add(SelectedEquipment);
        }
        _dataService.Save("equipment.json", list);
        LoadEquipment();
        SelectedEquipment = new Equipment();
    }

    [RelayCommand]
    private void DeleteEquipment()
    {
        if (SelectedEquipment.Id == 0) return;
        var list = EquipmentList.ToList();
        list.RemoveAll(e => e.Id == SelectedEquipment.Id);
        _dataService.Save("equipment.json", list);
        LoadEquipment();
    }

    partial void OnSearchTextChanged(string value)
    {
        LoadEquipment();
        if (!string.IsNullOrEmpty(value))
        {
            var filtered = EquipmentList.Where(e => 
                e.Name.Contains(value, System.StringComparison.OrdinalIgnoreCase) || 
                e.Category.Contains(value, System.StringComparison.OrdinalIgnoreCase)).ToList();
            
            EquipmentList.Clear();
            foreach (var item in filtered) EquipmentList.Add(item);
        }
    }
}