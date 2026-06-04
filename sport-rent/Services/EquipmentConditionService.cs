using System;
using System.Collections.Generic;
using System.Linq;

namespace sport_rent.Services;

public static class EquipmentConditionService
{
    private static readonly Dictionary<string, string> EnToUk = new(StringComparer.OrdinalIgnoreCase)
    {
        ["New"] = "Новий",
        ["Excellent"] = "Відмінний",
        ["Good"] = "Добрий",
        ["Fair"] = "Задовільний",
        ["Poor"] = "Поганий",
        ["Damaged"] = "Пошкоджено",
    };

    private static readonly Dictionary<string, string> UkToEn =
        EnToUk.ToDictionary(kv => kv.Value, kv => kv.Key, StringComparer.OrdinalIgnoreCase);

    public static bool IsEnglish => LocalizationService.Instance.CurrentLang == "en";

    public static string[] GetDefaultOptions() =>
        IsEnglish
            ? new[] { "New", "Excellent", "Good", "Fair", "Poor", "Damaged" }
            : new[] { "Новий", "Відмінний", "Добрий", "Задовільний", "Поганий", "Пошкоджено" };

    public static string DefaultCondition() => IsEnglish ? "Good" : "Добрий";

    public static string Normalize(string? condition)
    {
        if (string.IsNullOrWhiteSpace(condition))
            return DefaultCondition();

        if (IsEnglish)
        {
            if (UkToEn.TryGetValue(condition, out var en))
                return en;
            if (EnToUk.ContainsKey(condition))
                return condition;
            return condition;
        }

        if (EnToUk.TryGetValue(condition, out var uk))
            return uk;
        return condition;
    }
}
