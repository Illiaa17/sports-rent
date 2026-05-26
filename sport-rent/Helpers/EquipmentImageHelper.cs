using Avalonia.Media.Imaging;
using System;
using System.IO;

namespace sport_rent.Helpers;

public static class EquipmentImageHelper
{
    public static string ImagesDirectory =>
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images", "equipment");

    public static void EnsureImagesDirectory() => Directory.CreateDirectory(ImagesDirectory);

    public static Bitmap? TryLoadBitmap(string? path)
    {
        if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            return null;

        try
        {
            return new Bitmap(path);
        }
        catch
        {
            return null;
        }
    }

    public static string PersistImage(string sourcePath, int equipmentId)
    {
        EnsureImagesDirectory();
        var ext = Path.GetExtension(sourcePath);
        if (string.IsNullOrEmpty(ext))
            ext = ".jpg";

        var dest = Path.Combine(ImagesDirectory, $"equipment_{equipmentId}{ext}");
        File.Copy(sourcePath, dest, overwrite: true);
        return dest;
    }

    public static bool IsStoredInApp(string path) =>
        !string.IsNullOrWhiteSpace(path) &&
        path.StartsWith(ImagesDirectory, StringComparison.OrdinalIgnoreCase);
}
