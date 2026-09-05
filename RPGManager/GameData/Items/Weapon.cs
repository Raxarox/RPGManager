using System.Text.Json.Serialization;
using RPGManager.System;

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
    public bool IsHeavy { get; init; }
    public bool IsFinesse { get; init; }
    public bool IsThrown { get; init; }
    public bool IsVersatile { get; init; }
    public string? VersatileDamageDice { get; init; }

    [JsonConstructor]
    public Weapon(
        string templateId,
        string name,
        decimal weight,
        int valueInCopper,
        string description,
        string damageDice,
        string? damageType,
        WeaponCategory category,
        WeaponAttackType attackType,
        WeaponHandedness handedness,
        bool isLight = false,
        bool isHeavy = false,
        bool isFinesse = false,
        bool isThrown = false,
        bool isVersatile = false,
        string? versatileDamageDice = null)
        : base(templateId, name, weight, valueInCopper, description)
    {
        ValidateSingleHandedRequirement(name, isLight, handedness, "light");
        ValidateSingleHandedRequirement(name, isVersatile, handedness, "versatile");
        ValidationHelper.ValidateRequiredString(name, "Weapon", nameof(damageDice), damageDice);

        DamageDice = damageDice;
        DamageType = damageType ?? DamageTypes.Slashing;
        Category = category;
        AttackType = attackType;
        Handedness = handedness;
        IsLight = isLight;
        IsHeavy = isHeavy;
        IsFinesse = isFinesse;
        IsThrown = isThrown;
        IsVersatile = isVersatile;
        VersatileDamageDice = versatileDamageDice;
    }

    private static void ValidateSingleHandedRequirement(string name, bool requiresSingleHanded, WeaponHandedness handedness, string propertyName)
    {
        if (requiresSingleHanded && handedness != WeaponHandedness.SingleHanded)
        {
            throw new ArgumentException($"Weapon '{name}' cannot be {propertyName} because it is not single-handed.");
        }
    }
}