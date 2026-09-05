using System.Text.Json.Serialization;
using RPGManager.System;

namespace RPGManager.GameData.Characters;

public class CharacterClass
{
    public string ClassId { get; init; }
    public string Name { get; init; }
    public int HitDieValue { get; init; }
    public bool IsSpellcaster { get; init; }
    public List<string> WeaponProficiencies { get; init; }
    public List<string> ArmorProficiencies { get; init; }

    [JsonConstructor]
    public CharacterClass(
        string classId,
        string name,
        int hitDieValue,
        bool isSpellcaster,
        List<string>? weaponProficiencies,
        List<string>? armorProficiencies)
    {
        ValidationHelper.ValidatePositiveInteger(name, "Class", nameof(hitDieValue), hitDieValue);

        ClassId = classId ?? throw new ArgumentNullException(nameof(classId));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        HitDieValue = hitDieValue;
        IsSpellcaster = isSpellcaster;
        WeaponProficiencies = weaponProficiencies ?? [];
        ArmorProficiencies = armorProficiencies ?? [];
    }

    public override string ToString() => Name;
}