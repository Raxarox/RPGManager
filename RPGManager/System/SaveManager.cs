using System.Text.Json;
using RPGManager.GameData.Campaigns;
using RPGManager.GameData.Characters;

namespace RPGManager.System;

public static class SaveManager
{
    private const string SaveDirectory = "Saved Campaigns";

    public static void Save(Campaign campaign, string saveName)
    {
        try
        {
            var campaignFolder = Path.Combine(SaveDirectory, saveName);
            Directory.CreateDirectory(campaignFolder);

            var options = new JsonSerializerOptions { WriteIndented = true };
            var characterFileNames = new List<string>();

            // 1. Save individual character files inside the campaign subdirectory
            foreach (var character in campaign.Characters)
            {
                string primaryClassId = character.Classes.FirstOrDefault()?.ClassAsset?.ClassId ?? "unknown";
                string characterFileName = $"character_{character.Name}_{primaryClassId}.json";
                string characterPath = Path.Combine(campaignFolder, characterFileName);

                string characterJson = JsonSerializer.Serialize(character, options);
                File.WriteAllText(characterPath, characterJson);
                characterFileNames.Add(characterFileName);
            }

            // 2. Save a simplified campaign file containing metadata and character file references
            var campaignSaveModel = new CampaignSaveModel
            {
                CampaignName = campaign.CampaignName,
                AvailableClasses = campaign.AvailableClasses.ToList(),
                CharacterFiles = characterFileNames
            };

            var campaignPath = GetCampaignFilePath(saveName);
            var campaignJson = JsonSerializer.Serialize(campaignSaveModel, options);
            File.WriteAllText(campaignPath, campaignJson);
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

    public static Campaign Load(string saveName, GameAssetRegistry assetRegistry)
    {
        try
        {
            var campaignPath = GetCampaignFilePath(saveName);
            if (!File.Exists(campaignPath))
                throw new FileNotFoundException($"Campaign file for '{saveName}' could not be found.");

            var campaignJson = File.ReadAllText(campaignPath);
            var campaignSaveModel = JsonSerializer.Deserialize<CampaignSaveModel>(campaignJson)
                   ?? throw new InvalidOperationException("The campaign file was empty or could not be deserialized.");

            var campaignFolder = Path.Combine(SaveDirectory, saveName);
            var characters = new List<Character>();

            // Load each individual character file and re-link assets via registry
            foreach (var charFileName in campaignSaveModel.CharacterFiles)
            {
                var charPath = Path.Combine(campaignFolder, charFileName);
                if (File.Exists(charPath))
                {
                    var charJson = File.ReadAllText(charPath);
                    var character = JsonSerializer.Deserialize<Character>(charJson);
                    
                    if (character != null)
                    {
                        foreach (var classProgress in character.Classes)
                        {
                            if (classProgress.ClassAsset == null &&
                                assetRegistry.Classes.TryGetValue(classProgress.ClassId, out var foundClass))
                            {
                                classProgress.ClassAsset = foundClass;
                            }

                            if (classProgress.ArchetypeAsset == null &&
                                !string.IsNullOrEmpty(classProgress.ArchetypeId) &&
                                assetRegistry.Archetypes.TryGetValue(classProgress.ArchetypeId, out var foundArchetype))
                            {
                                classProgress.ArchetypeAsset = foundArchetype;
                            }
                        }
                        characters.Add(character);
                    }
                }
            }

            return new Campaign(campaignSaveModel.CampaignName, characters, campaignSaveModel.AvailableClasses);
        }
        catch (FileNotFoundException)
        {
            throw new InvalidOperationException($"Save folder or file for '{saveName}' could not be found.");
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Save files for '{saveName}' are corrupted or invalid. ({ex.Message})", ex);
        }
        catch (IOException ex)
        {
            throw new InvalidOperationException($"Could not read save files: {ex.Message}", ex);
        }
    }

    public static bool SaveExists(string saveName)
    {
        return File.Exists(GetCampaignFilePath(saveName));
    }

    public static string[] GetSaveNames()
    {
        try
        {
            if (!Directory.Exists(SaveDirectory)) return [];

            return Directory.GetDirectories(SaveDirectory)
                .Select(Path.GetFileName)
                .OfType<string>()
                .Where(saveName => File.Exists(GetCampaignFilePath(saveName)))
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

    private static string GetCampaignFilePath(string saveName) =>
        Path.Combine(SaveDirectory, saveName, $"campaign_{saveName}.json");
}

public class CampaignSaveModel
{
    public string CampaignName { get; set; } = string.Empty;
    public List<string> AvailableClasses { get; set; } = new();
    public List<string> CharacterFiles { get; set; } = new();
}