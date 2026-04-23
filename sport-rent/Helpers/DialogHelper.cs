using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using sport_rent.Services;
using sport_rent.Views;
using System.Threading.Tasks;

namespace sport_rent.Helpers;

public static class DialogHelper
{
    private static LocalizationService Loc => LocalizationService.Instance;

    public static Task<bool> ConfirmDeleteAsync() =>
        ConfirmAsync(Loc["ConfirmDelete"], Loc["ConfirmDeleteMsg"]);

    public static Task<bool> ConfirmReturnAsync() =>
        ConfirmAsync(Loc["ReturnConfirmTitle"], Loc["ReturnConfirmMsg"]);

    public static Task<bool> ConfirmLogoutAsync() =>
        ConfirmAsync(Loc["ConfirmLogoutTitle"], Loc["ConfirmLogoutMsg"]);

    public static Task<bool> ConfirmClearAsync() =>
        ConfirmAsync(Loc["ConfirmClearTitle"], Loc["ConfirmClearMsg"]);

    public static Task<bool> ConfirmSaveAsync() =>
        ConfirmAsync(Loc["ConfirmSaveTitle"], Loc["ConfirmSaveMsg"]);

    public static async Task<bool> ConfirmAsync(string title, string message)
    {
        var mainWindow = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
        if (mainWindow == null) return true;
        return await new ConfirmDialog(title, message).ShowDialog<bool>(mainWindow) == true;
    }
}
