using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using sport_rent.Helpers;
using sport_rent.Models;
using sport_rent.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace sport_rent.ViewModels;

public partial class UsersViewModel : BaseViewModel
{
    private readonly JsonDataService _dataService = new();
    private List<User> _allUsers = new();

    public ObservableCollection<User> Users { get; } = new();

    [ObservableProperty] private User selectedUser = new();
    [ObservableProperty] private string newPassword = string.Empty;
    [ObservableProperty] private string validationError = string.Empty;

    public List<string> Roles { get; } = new() { "Admin", "Operator" };

    public string FormTitle => SelectedUser.Id == 0
        ? Loc["AddUserTitle"]
        : string.Format(Loc["EditUserTitle"], SelectedUser.Id);

    public bool HasNoData => Users.Count == 0;

    public UsersViewModel()
    {
        LoadUsers();
    }

    partial void OnSelectedUserChanged(User value)
    {
        NewPassword = string.Empty;
        OnPropertyChanged(nameof(FormTitle));
    }

    [RelayCommand]
    private void NewUser()
    {
        SelectedUser = new User();
        NewPassword = string.Empty;
        ValidationError = string.Empty;
    }

    private void LoadUsers()
    {
        _allUsers = _dataService.Load<User>("users.json");
        Users.Clear();
        foreach (var u in _allUsers) Users.Add(u);
        OnPropertyChanged(nameof(HasNoData));
    }

    [RelayCommand]
    private async Task SaveUser()
    {
        if (string.IsNullOrWhiteSpace(SelectedUser.Login))
        {
            ValidationError = Loc["LoginRequired"];
            return;
        }

        if (SelectedUser.Id == 0 && string.IsNullOrWhiteSpace(NewPassword))
        {
            ValidationError = Loc["PasswordRequired"];
            return;
        }

        if (!await DialogHelper.ConfirmSaveAsync()) return;

        if (SelectedUser.Id == 0)
        {
            SelectedUser.Id = _allUsers.Any() ? _allUsers.Max(u => u.Id) + 1 : 1;
            SelectedUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(NewPassword);
            _allUsers.Add(SelectedUser);
        }
        else
        {
            var idx = _allUsers.FindIndex(u => u.Id == SelectedUser.Id);
            if (idx >= 0)
            {
                if (!string.IsNullOrWhiteSpace(NewPassword))
                    SelectedUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(NewPassword);
                _allUsers[idx] = SelectedUser;
            }
        }

        _dataService.Save("users.json", _allUsers);
        LoadUsers();
        SelectedUser = new User();
        NewPassword = string.Empty;
        ValidationError = string.Empty;
    }

    [RelayCommand]
    private async Task DeleteUser()
    {
        if (SelectedUser.Id == 0) return;
        if (!await DialogHelper.ConfirmDeleteAsync()) return;
        _allUsers.RemoveAll(u => u.Id == SelectedUser.Id);
        _dataService.Save("users.json", _allUsers);
        LoadUsers();
        SelectedUser = new User();
    }
}
