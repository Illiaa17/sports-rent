using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using sport_rent.Helpers;
using sport_rent.Models;

namespace sport_rent.ViewModels;

public partial class EquipmentCardViewModel : ObservableObject
{
    public Equipment Equipment { get; }

    [ObservableProperty] private bool isSelected;
    [ObservableProperty] private Bitmap? image;
    [ObservableProperty] private bool hasImage;

    public string Name => Equipment.Name;
    public string Category => Equipment.Category;
    public string DailyRateText => Equipment.DailyRate.ToString("F2");
    public string DepositText => Equipment.DepositAmount.ToString("F2");
    public int Quantity => Equipment.Quantity;
    public string Condition => Equipment.Condition;

    public EquipmentCardViewModel(Equipment equipment)
    {
        Equipment = equipment;
        ReloadImage();
    }

    public void ReloadImage()
    {
        Image = EquipmentImageHelper.TryLoadBitmap(Equipment.ImagePath);
        HasImage = Image != null;
    }
}
