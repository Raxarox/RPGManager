using System.Text.Json;
using RPGManager.GameData.Campaigns;

namespace RPGManager.System;

public static class SaveManager
{
    private const string SaveDirectory = "Saves";

    public static void Save(Campaign campaign, string saveName)
    {
        try
        {
            Directory.CreateDirectory(SaveDirectory);
            var filePath = GetSavePath(saveName);
            var json = JsonSerializer.Serialize(campaign);
            File.WriteAllText(filePath, json);
        }
        catch (UnauthorizedAccessException)
        {
            throw new InvalidOperationException($"Permission denied. Cannot write save file to disk.");
        }
        catch (PathTooLongException)
        {
            throw new InvalidOperationException("The save file path is too long for the system to handle.");
        }
        catch (DirectoryNotFoundException)
        {
            throw new InvalidOperationException("The target drive or folder path could not be found.");
        }
        catch (IOException ex)
        {
            throw new InvalidOperationException($"Disk error while saving campaign: {ex.Message}", ex);
        }
        catch (NotSupportedException ex)
        {
            throw new InvalidOperationException($"Failed to serialize campaign data: {ex.Message}", ex);
        }
        
    }

    public static Campaign Load(string saveName)
    {
        try
        {
            var filePath = GetSavePath(saveName);
            return JsonSerializer.Deserialize<Campaign>(File.ReadAllText(filePath))
                   ?? throw new InvalidOperationException("The campaign file was empty or could not be deserialized.");
        }
        catch (FileNotFoundException)
        {
            throw new InvalidOperationException($"Save file '{saveName}' could not be found.");
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Save file '{saveName}' is corrupted or invalid. ({ex.Message})", ex);
        }
        catch (IOException ex)
        {
            throw new InvalidOperationException($"Could not read save file: {ex.Message}", ex);
        }
    }

    public static bool SaveExists(string saveName)
    {
        return File.Exists(GetSavePath(saveName));
    }

    public static string[] GetSaveNames()
    {
        try
        {
            if (!Directory.Exists(SaveDirectory)) return [];

            return Directory.GetFiles(SaveDirectory, "*.json")
                .Select(Path.GetFileNameWithoutExtension)
                .OfType<string>()
                .ToArray();
        }
        catch (UnauthorizedAccessException)
        {
            throw new InvalidOperationException("Permission denied. Cannot read the save directory.");
        }
        catch (PathTooLongException)
        {
            throw new InvalidOperationException("The save directory path is too long.");
        }
        catch (IOException ex)
        {
            throw new InvalidOperationException($"Disk error while reading save files: {ex.Message}");
        }
    }

    private static string GetSavePath(string saveName) =>
        Path.Combine(SaveDirectory, saveName + ".json");
}
