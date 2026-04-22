using System.Collections.Generic;
using System.IO;
using System.Text;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;

namespace sport_rent.Helpers;

public static class CsvHelper
{
    public static string Escape(object? value)
    {
        var s = value?.ToString() ?? string.Empty;
        if (s.Contains(',') || s.Contains('"') || s.Contains('\n') || s.Contains('\r'))
        {
            s = "\"" + s.Replace("\"", "\"\"") + "\"";
        }
        return s;
    }

    public static string BuildCsv(IEnumerable<string> headers, IEnumerable<IEnumerable<object?>> rows)
    {
        var sb = new StringBuilder();
        sb.AppendLine(string.Join(",", headers));
        foreach (var row in rows)
        {
            var cells = new List<string>();
            foreach (var cell in row) cells.Add(Escape(cell));
            sb.AppendLine(string.Join(",", cells));
        }
        return sb.ToString();
    }

    public static async System.Threading.Tasks.Task SaveCsvAsync(string suggestedName, string content)
    {
        var lifetime = Avalonia.Application.Current?.ApplicationLifetime
            as IClassicDesktopStyleApplicationLifetime;
        var window = lifetime?.MainWindow;

        if (window?.StorageProvider is { } sp)
        {
            var file = await sp.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                SuggestedFileName = suggestedName,
                DefaultExtension = "csv",
                FileTypeChoices = new[]
                {
                    new FilePickerFileType("CSV")
                    {
                        Patterns = new[] { "*.csv" },
                        MimeTypes = new[] { "text/csv" }
                    }
                }
            });

            if (file is null) return;

            await using var stream = await file.OpenWriteAsync();
            await using var writer = new StreamWriter(stream, new UTF8Encoding(true));
            await writer.WriteAsync(content);
            return;
        }

        await File.WriteAllTextAsync(suggestedName, content, new UTF8Encoding(true));
    }
}
