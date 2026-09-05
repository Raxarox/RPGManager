using System.Text.Json.Serialization;

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
        if (armorClassBonus < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(armorClassBonus), $"Armor '{name}' cannot have a negative Armor Class bonus.");
        }

        ArmorClassBonus = armorClassBonus;
        ArmorType = armorType;
    }
}