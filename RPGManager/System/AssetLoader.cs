using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using RPGManager.GameData.Characters;
using RPGManager.GameData.Items;

namespace RPGManager.System;

public static class AssetLoader
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public static IReadOnlyDictionary<string, CharacterClass> LoadClasses(string directoryPath)
    {
        var classes = new Dictionary<string, CharacterClass>(StringComparer.OrdinalIgnoreCase);

        try
        {
            EnsureDataFilesExist<CharacterClass>(
                directoryPath, "Classes.json", c => c.ClassId);

            if (!Directory.Exists(directoryPath))
            {
                return classes; // Return empty registry if directory doesn't exist yet
            }

            var files = Directory.GetFiles(directoryPath, "*.json");
            foreach (var filePath in files)
            {
                var json = File.ReadAllText(filePath);
                var characterClass = JsonSerializer.Deserialize<CharacterClass>(json, JsonOptions)
                                     ?? throw new InvalidOperationException($"Class file '{filePath}' was empty.");

                if (string.IsNullOrWhiteSpace(characterClass.ClassId))
                {
                    throw new InvalidOperationException($"Class file '{filePath}' is missing a valid 'ClassId'.");
                }

                if (!classes.TryAdd(characterClass.ClassId, characterClass))
                {
                    throw new InvalidOperationException($"Duplicate class ID found: '{characterClass.ClassId}'.");
                }
            }
        }
        catch (UnauthorizedAccessException)
        {
            throw new InvalidOperationException(
                $"Permission denied while reading class assets from '{directoryPath}'.");
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Failed to parse class JSON asset: {ex.Message}", ex);
        }
        catch (IOException ex)
        {
            throw new InvalidOperationException($"Disk error while reading class assets: {ex.Message}", ex);
        }

        return classes;
    }

    public static IReadOnlyDictionary<string, Item> LoadItems(string directoryPath)
    {
        var items = new Dictionary<string, Item>(StringComparer.OrdinalIgnoreCase);

        try
        {
            EnsureDataFilesExist<Item>(directoryPath, "Items.json", i => i.TemplateId);

            if (!Directory.Exists(directoryPath))
            {
                return items;
            }

            var files = Directory.GetFiles(directoryPath, "*.json");
            foreach (var filePath in files)
            {
                var json = File.ReadAllText(filePath);
                // System.Text.Json automatically handles polymorphism thanks to [JsonDerivedType] on Item.cs
                var item = JsonSerializer.Deserialize<Item>(json, JsonOptions)
                           ?? throw new InvalidOperationException($"Item file '{filePath}' was empty.");

                if (string.IsNullOrWhiteSpace(item.TemplateId))
                {
                    throw new InvalidOperationException($"Item file '{filePath}' is missing a valid 'TemplateId'.");
                }

                if (!items.TryAdd(item.TemplateId, item))
                {
                    throw new InvalidOperationException($"Duplicate item template ID found: '{item.TemplateId}'.");
                }
            }
        }
        catch (UnauthorizedAccessException)
        {
            throw new InvalidOperationException($"Permission denied while reading item assets from '{directoryPath}'.");
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Failed to parse item JSON asset: {ex.Message}", ex);
        }
        catch (IOException ex)
        {
            throw new InvalidOperationException($"Disk error while reading item assets: {ex.Message}", ex);
        }

        return items;
    }

    // If directoryPath is missing or has no .json files (fresh install, or the
    // files were deleted), extracts the embedded fallback data and writes one
    // file per entry to directoryPath. If files already exist, does nothing.
    private static void EnsureDataFilesExist<T>(
        string directoryPath,
        string embeddedFileName,
        Func<T, string> getFileName)
    {
        if (Directory.Exists(directoryPath) && Directory.GetFiles(directoryPath, "*.json").Length > 0)
            return; // Files already exist — nothing to heal.

        Directory.CreateDirectory(directoryPath);

        using var stream = GetEmbeddedResourceStream(embeddedFileName)
            ?? throw new InvalidOperationException($"Embedded fallback resource '{embeddedFileName}' was not found.");

        var defaults = JsonSerializer.Deserialize<List<T>>(stream, JsonOptions)
            ?? throw new InvalidOperationException(
                $"Embedded fallback resource '{embeddedFileName}' was empty or invalid.");

        foreach (var entry in defaults)
        {
            var filePath = Path.Combine(directoryPath, getFileName(entry) + ".json");
            File.WriteAllText(filePath, JsonSerializer.Serialize(entry, JsonOptions));
        }
    }

    // Looks up an embedded resource by the end of its name (e.g. "Classes.json")
    // rather than the full dotted resource name, so a folder rename won't break this.
    private static Stream? GetEmbeddedResourceStream(string fileName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = assembly.GetManifestResourceNames()
            .FirstOrDefault(name => name.EndsWith(fileName, StringComparison.OrdinalIgnoreCase));

        return resourceName is null ? null : assembly.GetManifestResourceStream(resourceName);
    }
}