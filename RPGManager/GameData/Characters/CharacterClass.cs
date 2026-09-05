using System.Text.Json.Serialization;

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
        List<string> weaponProficiencies,
        List<string> armorProficiencies)
    {
        if (hitDieValue <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(hitDieValue), $"Class '{name}' must have a valid positive hit die value.");
        }

        ClassId = classId ?? throw new ArgumentNullException(nameof(classId));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        HitDieValue = hitDieValue;
        IsSpellcaster = isSpellcaster;
        WeaponProficiencies = weaponProficiencies ?? [];
        ArmorProficiencies = armorProficiencies ?? [];
    }

    public override string ToString() => Name;
}