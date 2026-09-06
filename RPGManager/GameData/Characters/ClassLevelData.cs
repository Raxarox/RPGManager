using System.Text.Json.Serialization;
using RPGManager.System;

namespace RPGManager.GameData.Characters;

public class ClassLevelData
{
    public int Level { get; init; }
    public int ProficiencyBonus { get; init; }
    public List<string> ClassFeatures { get; init; }
    public List<int> SpellSlots { get; init; }
    public Dictionary<string, int> ResourcePools { get; init; }

    [JsonConstructor]
    public ClassLevelData(
        int level,
        int proficiencyBonus,
        List<string>? classFeatures,
        List<int>? spellSlots,
        Dictionary<string, int>? resourcePools)
    {
        Level = level;
        ProficiencyBonus = proficiencyBonus;
        ClassFeatures = classFeatures ?? [];
        SpellSlots = spellSlots ?? [0, 0, 0, 0, 0, 0, 0, 0, 0];
        ResourcePools = resourcePools ?? [];
    }
}