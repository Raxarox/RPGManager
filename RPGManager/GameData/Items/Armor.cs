using System.Text.Json.Serialization;
using RPGManager.System;

namespace RPGManager.GameData.Items;

public enum ArmorType
{
    Light,
    Medium,
    Heavy,
    Shield
}

public class Armor : Item
{
    public int ArmorClassBonus { get; init; }
    public ArmorType ArmorType { get; init; }

    [JsonConstructor]
    public Armor(
        string templateId,
        string name,
        decimal weight,
        int valueInCopper,
        string description,
        int armorClassBonus,
        ArmorType armorType)
        : base(templateId, name, weight, valueInCopper, description)
    {
        ValidationHelper.ValidatePositiveValue(name, "Armor", nameof(armorClassBonus), armorClassBonus);

        ArmorClassBonus = armorClassBonus;
        ArmorType = armorType;
    }
}