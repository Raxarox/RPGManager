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

    public static IReadOnlyDictionary<string, CharacterClass> LoadClasses(string directoryPath) =>
        LoadAssets<CharacterClass>(directoryPath, "Classes.json", c => c.ClassId, "Class");

    public static IReadOnlyDictionary<string, Item> LoadItems(string directoryPath) =>
        LoadAssets<Item>(directoryPath, "Items.json", i => i.TemplateId, "Item");

    private static IReadOnlyDictionary<string, T> LoadAssets<T>(
        string directoryPath,
        string embeddedFileName,
        Func<T, string> getId,
        string assetType)
    {
        var assets = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);

        try
        {
            EnsureDataFilesExist<T>(directoryPath, embeddedFileName, getId);

            if (!Directory.Exists(directoryPath))
                return assets;

            foreach (var filePath in Directory.GetFiles(directoryPath, "*.json"))
            {
                var json = File.ReadAllText(filePath);
                var asset = JsonSerializer.Deserialize<T>(json, JsonOptions)
                           ?? throw new InvalidOperationException($"{assetType} file '{filePath}' was empty.");

                var id = getId(asset);
                if (string.IsNullOrWhiteSpace(id))
                    throw new InvalidOperationException($"{assetType} file '{filePath}' is missing a valid ID.");

                if (!assets.TryAdd(id, asset))
                    throw new InvalidOperationException($"Duplicate {assetType.ToLower()} ID found: '{id}'.");
            }
        }
        catch (UnauthorizedAccessException)
        {
            throw new InvalidOperationException($"Permission denied while reading {assetType.ToLower()} assets from '{directoryPath}'.");
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Failed to parse {assetType.ToLower()} JSON asset: {ex.Message}", ex);
        }
        catch (IOException ex)
        {
            throw new InvalidOperationException($"Disk error while reading {assetType.ToLower()} assets: {ex.Message}", ex);
        }

        return assets;
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