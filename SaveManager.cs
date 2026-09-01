using System.Text.Json;
namespace RPGManager;

public static class SaveManager
{
    private const string SaveDirectory = "Saves";

    public static void Serialize(Campaign campaign, string fileName)
    {
        
        Directory.CreateDirectory(SaveDirectory);
        var filePath = Path.Combine(SaveDirectory, fileName + ".json");
        var json = JsonSerializer.Serialize(campaign);
        File.WriteAllText(filePath, json);
    }

    public static Campaign Deserialize(string fileName)
    {
        return JsonSerializer.Deserialize<Campaign>(File.ReadAllText(fileName)) 
               ?? throw new InvalidOperationException("The campaign file was empty or could not be deserialized.");
    }

    public static string GetFileNamed(string fileName)
    {
        return Path.Combine(SaveDirectory, fileName + ".json");
    }

    public static string[] GetSaves()
    {
                    return Directory.GetFiles("Saves", "*.json");
    }
}