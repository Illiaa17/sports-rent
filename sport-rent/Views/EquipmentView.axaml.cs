using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using sport_rent.ViewModels;
using System;
using System.IO;

namespace sport_rent.Views;

public partial class EquipmentView : UserControl
{
    public EquipmentView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private async void BrowseButton_Click(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null) return;

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Select Image",
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType("Images")
                {
                    Patterns = new[] { "*.jpg", "*.jpeg", "*.png", "*.webp", "*.bmp" }
                }
            }
        });

        if (files.Count == 0 || DataContext is not EquipmentViewModel vm) return;

        var file = files[0];

        // Use the real local path directly — no copy needed, works on macOS sandbox
        var localPath = file.TryGetLocalPath();
        if (!string.IsNullOrEmpty(localPath) && File.Exists(localPath))
        {
            vm.SetImagePath(localPath);
            return;
        }

        // Fallback: copy to absolute local directory when local path is unavailable
        var imagesDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images", "equipment");
        Directory.CreateDirectory(imagesDir);
        var dest = Path.Combine(imagesDir, file.Name);

        try
        {
            await using var readStream = await file.OpenReadAsync();
            await using var writeStream = File.Create(dest);
            await readStream.CopyToAsync(writeStream);
            vm.SetImagePath(dest);
        }
        catch
        {
            // File copy failed — image will not be set
        }
    }
}
