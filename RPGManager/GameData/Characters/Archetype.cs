using System.Text.Json.Serialization;
using RPGManager.System;

namespace RPGManager.GameData.Characters;

public class Archetype
{
    public string ArchetypeId { get; init; }
    public string Name { get; init; }

    // Which class this archetype belongs to (e.g. "fighter"). Prevents a
    // Wizard's Arcane Tradition from being attached to a Fighter by mistake.
    public string ParentClassId { get; init; }

    public List<ClassLevelData> Progression { get; init; }

    [JsonConstructor]
    public Archetype(
        string archetypeId,
        string name,
        string parentClassId,
        List<ClassLevelData>? progression)
    {
        ValidationHelper.ValidateRequiredString(name, "Archetype", nameof(archetypeId), archetypeId);
        ValidationHelper.ValidateRequiredString(name, "Archetype", nameof(parentClassId), parentClassId);

        ArchetypeId = archetypeId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        ParentClassId = parentClassId;
        Progression = progression ?? [];
    }

    public override string ToString() => Name;
}