using System.Text.Json;
namespace RPGManager;

public static class SaveManager
{
    private const string SaveDirectory = "Saves";

    public static void Save(Campaign campaign, string saveName)
    {
        Directory.CreateDirectory(SaveDirectory);
        var filePath = GetSavePath(saveName);
        var json = JsonSerializer.Serialize(campaign);
        File.WriteAllText(filePath, json);
    }

    public static Campaign Load(string saveName)
    {
        var filePath = GetSavePath(saveName);
        return JsonSerializer.Deserialize<Campaign>(File.ReadAllText(filePath))
               ?? throw new InvalidOperationException("The campaign file was empty or could not be deserialized.");
    }

    public static bool SaveExists(string saveName)
    {
        return File.Exists(GetSavePath(saveName));
    }

    public static string[] GetSaveNames()
    {
        if (!Directory.Exists(SaveDirectory)) return [];

        return Directory.GetFiles(SaveDirectory, "*.json")
            .Select(Path.GetFileNameWithoutExtension)
            .OfType<string>()
            .ToArray();
    }

    private static string GetSavePath(string saveName) =>
        Path.Combine(SaveDirectory, saveName + ".json");
}
