using System.Text.Json.Serialization;

namespace RPGManager.GameData.Items;

[JsonDerivedType(typeof(Weapon), "Weapon")]
[JsonDerivedType(typeof(Armor), "Armor")]

public abstract class Item
{
    public string TemplateId { get; init; }
    public string Name { get; init; }
    public decimal Weight { get; init; }
    public int ValueInCopper { get; init; }
    public string Description { get; init; }

    [JsonConstructor]
    protected Item(string templateId, string name, decimal weight, int valueInCopper, string description)
    {
        if (weight < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(weight), $"Item '{name}' cannot have a negative weight.");
        }
        
        if (valueInCopper < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(valueInCopper), $"Item '{name}' cannot have a negative value.");
        }

        TemplateId = templateId ?? throw new ArgumentNullException(nameof(templateId));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Weight = weight;
        ValueInCopper = valueInCopper;
        Description = description ?? string.Empty;
    }
}