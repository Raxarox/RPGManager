using System.Text.Json.Serialization;
using RPGManager.System;

namespace RPGManager.GameData.Items;

public class StackableItem : Item
{
    // Optional because a gold bar or a coil of rope has nothing to describe here,
    // while a Potion of Healing does.
    public string? EffectDescription { get; init; }

    [JsonConstructor]
    public StackableItem(
        string templateId,
        string name,
        decimal weight,
        int valueInCopper,
        string description,
        ItemCategory itemCategory,
        string? effectDescription)
        : base(templateId, name, weight, valueInCopper, description)
    {
        if (!IsValidStackableCategory(itemCategory))
        {
            throw new ArgumentException(
                $"StackableItem '{name}' cannot use category '{itemCategory}' — " +
                "that category already has its own dedicated Item subclass.");
        }

        ItemCategory = itemCategory;
        EffectDescription = effectDescription;
    }

    private static bool IsValidStackableCategory(ItemCategory category) => category switch
    {
        ItemCategory.Potion or ItemCategory.Scroll or ItemCategory.Ammunition
            or ItemCategory.Material or ItemCategory.Valuable or ItemCategory.UtilityItem => true,
        _ => false
    };
}