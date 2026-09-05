using System.Text.Json.Serialization;

namespace RPGManager.GameData.Items;

public enum WeaponCategory
{
    Simple,
    Martial
}

public enum WeaponAttackType
{
    Melee,
    Ranged
}

public enum WeaponHandedness
{
    SingleHanded,
    TwoHanded
}

public class Weapon : Item
{
    public string DamageDice { get; init; }
    public string DamageType { get; init; }
    public WeaponCategory Category { get; init; }
    public WeaponAttackType AttackType { get; init; }
    public WeaponHandedness Handedness { get; init; }
    public bool IsLight { get; init; }

    [JsonConstructor]
    public Weapon(
        string templateId,
        string name,
        decimal weight,
        int valueInCopper,
        string description,
        string damageDice,
        string damageType,
        WeaponCategory category,
        WeaponAttackType attackType,
        WeaponHandedness handedness,
        bool isLight)
        : base(templateId, name, weight, valueInCopper, description)
    {
        if (isLight && handedness != WeaponHandedness.SingleHanded)
        {
            throw new ArgumentException($"Weapon '{name}' cannot be light because it is not single-handed.");
        }

        if (string.IsNullOrWhiteSpace(damageDice))
        {
            throw new ArgumentException($"Weapon '{name}' must have valid damage dice.", nameof(damageDice));
        }

        DamageDice = damageDice;
        DamageType = damageType ?? "Slashing";
        Category = category;
        AttackType = attackType;
        Handedness = handedness;
        IsLight = isLight;
    }
}