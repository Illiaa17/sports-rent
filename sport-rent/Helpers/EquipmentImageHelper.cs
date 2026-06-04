using Avalonia.Media.Imaging;
using System;
using System.IO;

namespace sport_rent.Helpers;

public static class EquipmentImageHelper
{
    private const string DataFolder = "Data";

    public static string ImagesDirectory => Path.Combine(DataFolder, "images", "equipment");

    public static void EnsureImagesDirectory() => Directory.CreateDirectory(ImagesDirectory);

    /// <summary>Повний шлях до файлу на диску (для завантаження в UI).</summary>
    public static string? ResolveFullPath(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return null;

        if (Path.IsPathRooted(path) && File.Exists(path))
            return path;

        var relative = path.Replace('\\', '/');

        foreach (var dataRoot in GetDataRoots())
        {
            var combined = Path.Combine(dataRoot, relative.Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(combined))
                return Path.GetFullPath(combined);

            var byName = Path.Combine(dataRoot, "images", "equipment", Path.GetFileName(relative));
            if (File.Exists(byName))
                return Path.GetFullPath(byName);
        }

        return null;
    }

    /// <summary>Шлях для JSON — відносно папки Data.</summary>
    public static string ToJsonPath(string fullPath)
    {
        foreach (var dataRoot in GetDataRoots())
        {
            var root = Path.GetFullPath(dataRoot);
            var full = Path.GetFullPath(fullPath);
            if (full.StartsWith(root + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                return Path.GetRelativePath(root, full).Replace('\\', '/');
        }

        return $"images/equipment/{Path.GetFileName(fullPath)}";
    }

    public static Bitmap? TryLoadBitmap(string? path)
    {
        var full = ResolveFullPath(path);
        if (full == null)
            return null;

        try
        {
            return new Bitmap(full);
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
        return ToJsonPath(dest);
    }

    private static string[] GetDataRoots() =>
    [
        Path.Combine(Directory.GetCurrentDirectory(), DataFolder),
        Path.Combine(AppContext.BaseDirectory, DataFolder),
    ];
}
