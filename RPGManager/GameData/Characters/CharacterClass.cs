using System.Text.Json.Serialization;
using RPGManager.System;

namespace RPGManager.GameData.Characters;

public class CharacterClass
{
    public string ClassId { get; init; }
    public string Name { get; init; }
    public int HitDieValue { get; init; }
    public List<string> WeaponProficiencies { get; init; }
    public List<string> ArmorProficiencies { get; init; }
    public List<ClassLevelData> Progression { get; init; }

    // Which Archetype.ArchetypeId values a character of this class may choose from.
    public List<string> AvailableArchetypeIds { get; init; }

    // The level at which an archetype choice becomes available.
    // 0 means this class currently offers no archetype choice.
    public int ArchetypeChoiceLevel { get; init; }

    [JsonConstructor]
    public CharacterClass(
        string classId,
        string name,
        int hitDieValue,
        List<string>? weaponProficiencies,
        List<string>? armorProficiencies,
        List<ClassLevelData>? progression,
        List<string>? availableArchetypeIds,
        int archetypeChoiceLevel)
    {
        ValidationHelper.ValidatePositiveInteger(name, "Class", nameof(hitDieValue), hitDieValue);

        ClassId = classId ?? throw new ArgumentNullException(nameof(classId));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        HitDieValue = hitDieValue;
        WeaponProficiencies = weaponProficiencies ?? [];
        ArmorProficiencies = armorProficiencies ?? [];
        Progression = progression ?? [];
        AvailableArchetypeIds = availableArchetypeIds ?? [];
        ArchetypeChoiceLevel = archetypeChoiceLevel;
    }

    public override string ToString() => Name;
}